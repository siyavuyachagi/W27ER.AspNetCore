using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Configuration settings for sending emails using SMTP.
    /// Bound from configuration (e.g., appsettings.json or environment variables).
    /// </summary>
    public sealed class SmtpSettings
    {
        /// <summary>
        /// The SMTP server host (e.g., "smtp.gmail.com").
        /// </summary>
        public string Host { get; set; } = default!;

        /// <summary>
        /// The port used by the SMTP server.
        /// Common values:
        /// - 587: TLS/STARTTLS
        /// - 465: SSL
        /// - 25: Non-secure
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Username for authenticating with the SMTP server.
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        /// Password for authenticating with the SMTP server.
        /// This should be kept secret and not committed to source control.
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// The "From" email address that appears on sent emails.
        /// </summary>
        public string From { get; set; } = default!;

        /// <summary>
        /// Optional display name that appears alongside the "From" address.
        /// Example: "NetSolutions Support"
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Whether to enable SSL/TLS for the SMTP connection.
        /// Default: true
        /// </summary>
        public bool EnableSsl { get; set; } = true;
    }

}
