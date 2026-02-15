using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Configuration settings for Cloudinary cloud storage.
    /// Bound from configuration (e.g., appsettings.json or environment variables).
    /// </summary>
    public sealed class CloudinaryStorageSettings
    {
        /// <summary>
        /// Your Cloudinary account cloud name.
        /// Example: "myapp-cloud"
        /// </summary>
        public string CloudName { get; set; } = default!;

        /// <summary>
        /// API key for authenticating with Cloudinary.
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// API secret used to sign requests and authenticate securely.
        /// This should be kept secret and not committed to source control.
        /// </summary>
        public string Secret { get; set; } = default!;
    }

}
