using Application.DataTransferModels;
using Application.DataTransferModels.Auth;
using Application.DataTransferModels.Serializers;
using Application.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ward27EmploymentRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService, 
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns access and refresh tokens.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<GenericApiResponse<AuthResponse>> LoginAsync([FromBody] LoginDto model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return GenericApiResponse<AuthResponse>.UnprocessableEntity(ModelState);

            try
            {
                var result = await _authService.LoginAsync(model.Username, model.Password, model.RememberMe, cancellationToken);
                if (!result.Succeeded)
                    return GenericApiResponse<AuthResponse>.Unauthorized(result.Message);

                return GenericApiResponse<AuthResponse>.Ok(result.Data, result.Message);
            }
            catch (Exception ex)
            {
                return GenericApiResponse<AuthResponse>.InternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<GenericApiResponse<AuthResponse>> RefreshAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var result = await _authService.RefreshTokenAsync(cancellationToken);

                if (!result.Succeeded) return GenericApiResponse<AuthResponse>.BadRequest();

                return GenericApiResponse<AuthResponse>.Ok(result.Data, result.Message);
            }
            catch (Exception ex)
            {
                return GenericApiResponse<AuthResponse>.InternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<GenericApiResponse<string>> RefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var result = await _authService.RefreshTokenAsync(cancellationToken);

                if (!result.Succeeded) return GenericApiResponse<string>.BadRequest();

                return GenericApiResponse<string>.Ok(result.Data.AccessToken, result.Message);
            }
            catch (Exception ex)
            {
                return GenericApiResponse<string>.InternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Logs out the current user by revoking their refresh token.
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ApiResponse> LogoutAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                // Revoke refresh token
                await _authService.LogoutAsync(cancellationToken);

                if (HttpContext.User.Identity.IsAuthenticated)
                    await HttpContext.SignOutAsync(HttpContext.User.Identity.Name);

                return ApiResponse.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ApiResponse.InternalServerError(ex.Message);
            }
        }


        /// <summary>
        /// Logs out the current user from all devices by revoking all their refresh tokens.
        /// </summary>
        [HttpPost("logout-all")]
        public async Task<ApiResponse> LogoutAllDevicesAsync()
        {
            try
            {
                // Get userId from JWT claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return ApiResponse.Unauthorized();

                var revoked = await _authService.RevokeAllTokensAsync();

                return ApiResponse.Ok();
            }
            catch (Exception ex)
            {
                return ApiResponse.InternalServerError();
            }
        }

        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
