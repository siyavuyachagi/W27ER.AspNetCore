using Application.DataTransferModels.Auth;
using Application.Shared;
using Domain.Entities;

namespace Application.IServices
{
    /// <summary>
    /// Defines authentication and authorization operations for user management.
    /// Handles user login, token refresh, and retrieval of user roles and permissions.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using a username and password and issues access and refresh tokens.
        /// </summary>
        /// <param name="username">
        /// The user's username or identifier used for authentication.
        /// </param>
        /// <param name="password">
        /// The user's plaintext password to be validated against stored credentials.
        /// </param>
        /// <param name="rememberMe">
        /// Boolean value indicating whether to persist session or not
        /// </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing an <see cref="AuthResponse"/> with access token,
        /// refresh token, and user details when authentication succeeds; otherwise a failure result
        /// with validation or authentication errors.
        /// </returns>
        Task<GenericResult<AuthResponse>> LoginAsync(string username, string password, bool rememberMe, CancellationToken cancellationToken = default);


        /// <summary>
        /// Logout the user's current session
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result> LogoutAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Validates a refresh token and issues a new access token and refresh token pair.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a new <see cref="AuthResponse"/> when the refresh
        /// token is valid; otherwise a failure result describing why the token is invalid.
        /// </returns>   
        Task<GenericResult<AuthResponse>> RefreshTokenAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Revokes a specific refresh token.
        /// </summary>
        /// <returns>True if the token was successfully revoked; otherwise, false.</returns>
        Task<Result> RevokeRefreshTokenAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes all active refresh tokens for the specified user.
        /// </summary>
        /// <returns>True if any tokens were successfully revoked; otherwise, false.</returns>
        Task<Result> RevokeAllTokensAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all role names assigned to a specific user.
        /// </summary>
        /// <returns>A list of role names assigned to the user.</returns>
        Task<GenericResult<List<string>>> GetUserRolesAsync();
    }
}
