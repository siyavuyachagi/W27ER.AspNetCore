namespace Infrastructure.Configurations
{
    /// <summary>
    /// Configuration settings used for generating and validating JWT tokens.
    /// Bound from configuration (e.g., appsettings.json).
    /// </summary>
    public sealed class JwtSettings
    {
        /// <summary>
        /// Secret key used to sign JWT tokens.
        /// 
        /// Must be sufficiently long (recommended: at least 64 characters)
        /// and stored securely (e.g., environment variables or secure vault).
        /// 
        /// This value should never be exposed publicly.
        /// </summary>
        public string Secret { get; set; } = null!;

        /// <summary>
        /// The issuer (iss claim) of the token.
        /// 
        /// Typically the base URL of your application
        /// (e.g., "https://myapp.com").
        /// </summary>
        public string Issuer { get; set; } = null!;

        /// <summary>
        /// The valid audiences (aud claim) for the token.
        /// 
        /// Represents the intended recipients of the token.
        /// Example:
        /// [
        ///   "https://myapp.com",
        ///   "https://myapp.com/api"
        /// ]
        /// </summary>
        public string[] Audiences { get; set; } = null!;

        /// <summary>
        /// Access token expiration time in minutes.
        /// Determines how long the issued JWT remains valid.
        /// </summary>
        public int TokenExpiryMinutes { get; set; }

        /// <summary>
        /// Refresh token expiration time in days.
        /// Determines how long a refresh token remains valid
        /// before requiring re-authentication.
        /// </summary>
        public int RefreshTokenExpiryDays { get; set; }
    }

}
