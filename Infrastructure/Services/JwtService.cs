using Application.IServices;
using Application.Shared;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Identity;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// Provides JWT token generation and refresh functionality for user authentication.
    /// Handles access token creation with user claims, roles, and permissions, as well as refresh token management.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtService> _logger;

        public JwtService(
            ApplicationDbContext db,
            IOptions<JwtSettings> jwtSettings,
            ILogger<JwtService> logger)
        {
            _db = db;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Generates a new JWT access token for the specified user with their roles and permissions.
        /// </summary>
        /// <param name="applicationUser">The application user to generate the token for.</param>
        /// <param name="roles">List of role names assigned to the user.</param>
        /// <param name="permissions">List of permission names granted to the user.</param>
        /// <returns>An access token result containing the JWT token and its expiration time.</returns>
        public async Task<GenericResult<string>> GenerateAccessTokenAsync(
            ApplicationUser applicationUser,
            List<string> roles)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                DateTimeOffset now = DateTimeOffset.UtcNow;

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
                    new(ClaimTypes.Email, applicationUser.Email!),
                    new(ClaimTypes.Name, applicationUser.FirstName!),
                    new(ClaimTypes.Surname, applicationUser.LastName!),

                    new(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
                    // Only add profile if it exists (Claim does NOT allow null)
                    new(JwtRegisteredClaimNames.Profile, applicationUser.ProfileImageUrl ?? string.Empty),
                    // iat MUST be Unix time (numeric date)
                    new(JwtRegisteredClaimNames.Iat,
                        now.ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64),
                };

                // Roles (multiple claims is fine in ASP.NET)
                claims.AddRange(
                    roles.Select(r => new Claim(CustomClaimTypes.Roles, r))
                );

                // Token expiration (DO NOT add exp as a claim)
                var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes);

                var jsToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audiences.FirstOrDefault(),
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                string token = new JwtSecurityTokenHandler().WriteToken(jsToken);

                return GenericResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GenericResult<string>.Failure("An error has occured.");
            }
        }

        /// <summary>
        /// Generates a cryptographically secure refresh token for the specified user and persists it to the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to generate the refresh token for.</param>
        /// <returns>A refresh token result containing the token string and its expiration time.</returns>
        public async Task<GenericResult<string>> GenerateRefreshTokenAsync(Guid userId)
        {
            try
            {
                var randomBytes = new byte[64];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomBytes);
                var refreshToken = Convert.ToBase64String(randomBytes);

                var expires = DateTime.UtcNow.AddDays(
                    Convert.ToInt32(_jwtSettings.RefreshTokenExpiryDays)
                );

                var refreshTokenEntity = new RefreshToken
                {
                    ApplicationUserId = userId,
                    EncodedToken = EncodeToken(refreshToken),
                    IsRevoked = false,
                    //ReplacedByTokenHash = null,
                    Device = null,
                    IpAddress = null,
                    ExpiresAt = expires,
                    CreatedAt = DateTime.UtcNow,
                };

                _db.RefreshTokens.Add(refreshTokenEntity);
                await _db.SaveChangesAsync();

                return GenericResult<string>.Success(refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GenericResult<string>.Failure("An error has occured.");
            }
        }


        /// <summary>
        /// Revokes a specific refresh token by marking it as revoked in the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token string to revoke.</param>
        /// <returns>Success if revoke was successful, otherwise Failure</returns>
        /// <exception cref="Exception">Thrown when the refresh token is not found.</exception>
        public async Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var refreshTokenEntity = await _db.RefreshTokens
                    .FirstOrDefaultAsync(x => x.EncodedToken == EncodeToken(refreshToken));

                if (refreshTokenEntity is null)
                    throw new Exception("Refresh token not found.");

                if (refreshTokenEntity.IsRevoked)
                    return Result.Success(); // Already revoked

                refreshTokenEntity.IsRevoked = true;
                refreshTokenEntity.RevokedAt = DateTime.UtcNow;
                //refreshTokenEntity.ReplacedByTokenHash = EncodeToken(refreshToken);
                await _db.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure("An error has occured.");
            }
        }



        /// <summary>
        /// Encodes a plain text string to Base64 format for safe transmission or storage.
        /// </summary>
        /// <param name="token">The plain text string to encode (e.g., "user@example.com", "mytoken123")</param>
        /// <returns>Base64-encoded string representation of the input</returns>
        /// <remarks>
        /// Use this when you need to encode text data that may contain special characters
        /// for safe transmission (e.g., in URLs or headers). This is NOT for hashing - 
        /// it's reversible using DecodeToken().
        /// </remarks>
        /// <example>
        /// var encoded = EncodeToken("hello@world.com"); // Returns: "aGVsbG9Ad29ybGQuY29t"
        /// </example>
        public static string EncodeToken(string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }

        /// <summary>
        /// Decodes a Base64-encoded string back to its original plain text format.
        /// </summary>
        /// <param name="encodedToken">The Base64-encoded string to decode</param>
        /// <returns>Original plain text string</returns>
        /// <remarks>
        /// This is the reverse operation of EncodeToken(). Use this to retrieve the 
        /// original string from a Base64-encoded value.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the encoded token is not valid Base64</exception>
        /// <example>
        /// var decoded = DecodeToken("aGVsbG9Ad29ybGQuY29t"); // Returns: "hello@world.com"
        /// </example>
        public static string DecodeToken(string encodedToken)
        {
            var tokenBytes = Convert.FromBase64String(encodedToken);
            return Encoding.UTF8.GetString(tokenBytes);
        }

        /// <summary>
        /// Decodes a URL-safe Base64 token to standard Base64 format.
        /// </summary>
        /// <param name="urlSafeToken">URL-safe Base64 token (generated by WebEncoders.Base64UrlEncode)</param>
        /// <returns>Standard Base64 string representation of the token bytes</returns>
        /// <remarks>
        /// URL-safe Base64 encoding replaces '+' with '-', '/' with '_', and removes '=' padding,
        /// making tokens safe for use in URLs and query strings. This method converts back to 
        /// standard Base64 format, which is useful for consistent hashing or storage operations.
        /// 
        /// This does NOT return the original random bytes or plain text - it converts from 
        /// URL-safe Base64 to standard Base64 encoding.
        /// </remarks>
        /// <example>
        /// // If urlSafeToken = "K7X9M-2P_4" (URL-safe)
        /// var standardBase64 = DecodeUrlSafeToken(urlSafeToken); // Returns: "K7X9M+2P/4" (standard Base64)
        /// </example>
        public static string DecodeUrlSafeToken(string urlSafeToken)
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(urlSafeToken);
            return Convert.ToBase64String(tokenBytes);
        }

        /// <summary>
        /// Decodes a URL-safe token and generates a SHA256 hash for secure database storage.
        /// </summary>
        /// <param name="urlSafeToken">URL-safe Base64 token to hash (typically sent to users in emails or URLs)</param>
        /// <returns>Base64-encoded SHA256 hash of the token suitable for database storage</returns>
        /// <remarks>
        /// This method is used when storing tokens securely in a database. The URL-safe token 
        /// should be sent to the user (e.g., in an email confirmation link), while the hash 
        /// returned by this method should be stored in the database. This ensures that even if 
        /// the database is compromised, the actual tokens cannot be retrieved.
        /// 
        /// When validating a token later, hash the user-provided token using this method and 
        /// compare it with the stored hash.
        /// 
        /// Use this for:
        /// - Email confirmation tokens
        /// - Password reset tokens
        /// - Any token sent to users that needs secure storage
        /// </remarks>
        /// <example>
        /// // Generate and store token
        /// var token = TokenHelper.GenerateUrlSafeToken();
        /// var hash = EncodeUrlSafeToken(token); // Store this hash in database
        /// // Send 'token' to user via email
        /// 
        /// // Later, when user provides token for validation
        /// var userToken = Request.Query["token"];
        /// var userTokenHash = EncodeUrlSafeToken(userToken);
        /// // Compare userTokenHash with stored hash in database
        /// </example>
        public static string EncodeUrlSafeToken(string urlSafeToken)
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(urlSafeToken);
            var hashBytes = SHA256.HashData(tokenBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
