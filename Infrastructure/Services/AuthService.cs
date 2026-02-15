using Application.DataTransferModels.Auth;
using Application.DataTransferModels.Serializers;
using Application.IServices;
using Application.Shared;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRedisService _redisService;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            IRedisService redisService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _redisService = redisService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        // Fixed: Safe nullable int parsing
        private int? _currentUserId
        {
            get
            {
                if (_httpContextAccessor?.HttpContext?.User.Identity is null)
                    return null;

                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    return null;

                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return null;

                return int.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        public async Task<GenericResult<List<string>>> GetUserRolesAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (user is null)
                    return GenericResult<List<string>>.Failure("User not authorozed");

                var cachedRoles = await _redisService.GetAsync<List<string>>($"{user.Id}:roles");
                if (cachedRoles is not null)
                    return GenericResult<List<string>>.Success(cachedRoles);

                var roles = (await _userManager.GetRolesAsync(user)).ToList();

                await _redisService.SetAsync($"{user.Id}:roles", roles, _cacheExpiry);

                return GenericResult<List<string>>.Success(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GenericResult<List<string>>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<AuthResponse>> LoginAsync(string username, string password, bool rememberMe, CancellationToken cancellationToken = default)
        {
            try
            {
                // Find user by username and include tenant information
                var user = await _userManager.FindByEmailAsync(username)
                    ?? await _userManager.FindByNameAsync(username);

                if (user == null)
                    return GenericResult<AuthResponse>.Failure("Invalid username or password");

                // Verify password
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (!result.Succeeded) return GenericResult<AuthResponse>.Failure("Invalid username or password");

                // Get roles + permissions using cache
                var roles = (await GetUserRolesAsync()).Data ?? [];

                // Generate access and refresh tokens
                var accessTokenResult = await _jwtService.GenerateAccessTokenAsync(user, roles);
                if(!accessTokenResult.Succeeded || string.IsNullOrEmpty(accessTokenResult.Data))
                    return GenericResult<AuthResponse>.Failure($"Login failure: {accessTokenResult.Message}");

                var refreshTokenResult = await _jwtService.GenerateRefreshTokenAsync(user.Id);
                if (!refreshTokenResult.Succeeded || string.IsNullOrEmpty(refreshTokenResult.Data))
                    return GenericResult<AuthResponse>.Failure($"Login failure: {refreshTokenResult.Message}");

                await _httpContextAccessor?.HttpContext?.AuthenticateAsync();

                var authResponse = new AuthResponse
                {
                    AccessToken = accessTokenResult.Data,
                    RefreshToken = refreshTokenResult.Data,
                    User = new AuthUser(user, roles)
                };
                return GenericResult<AuthResponse>.Success(authResponse, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GenericResult<AuthResponse>.Failure(ex.Message);
            }
        }

        public Task<Result> LogoutAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult<AuthResponse>> RefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RevokeAllTokensAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RevokeRefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
