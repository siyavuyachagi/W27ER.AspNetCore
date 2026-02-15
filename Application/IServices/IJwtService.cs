using Application.Shared;
using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.IServices
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates a new JWT access token for the specified user with their roles and permissions.
        /// </summary>
        /// <param name="applicationUser">The application user to generate the token for.</param>
        /// <param name="roles">List of role names assigned to the user.</param>
        /// <returns>A <see cref="GenericResult{string}"/> containing the JWT access token.</returns>
        Task<GenericResult<string>> GenerateAccessTokenAsync(
            ApplicationUser applicationUser,
            List<string> roles);

        /// <summary>
        /// Generates a cryptographically secure refresh token for the specified user and persists it to the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to generate the refresh token for.</param>
        /// <returns>A <see cref="GenericResult{string}"/> containing refresh string token.</returns>
        Task<GenericResult<string>> GenerateRefreshTokenAsync(Guid userId);
    }
}
