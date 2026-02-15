using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Represents an image resource (jpg, png, gif, webp, etc.)
    /// Inherits all Cloudinary properties from Resource
    /// </summary>
    [Table("Images")]
    public class Image : Resource
    {
        // Image-specific helper methods can be added here

        /// <summary>
        /// Gets optimized image URL for web display
        /// </summary>
        public string GetOptimizedUrl(int maxWidth = 1200, string quality = "auto")
        {
            return GetTransformedUrl($"w_{maxWidth},c_limit,q_{quality},f_auto");
        }

        /// <summary>
        /// Gets responsive image URLs for different screen sizes
        /// </summary>
        public Dictionary<string, string> GetResponsiveUrls()
        {
            return new Dictionary<string, string>
            {
                { "thumbnail", GetThumbnailUrl(200, 200) },
                { "small", GetTransformedUrl("w_400,c_limit,q_auto") },
                { "medium", GetTransformedUrl("w_800,c_limit,q_auto") },
                { "large", GetTransformedUrl("w_1200,c_limit,q_auto") },
                { "original", CloudinarySecureUrl }
            };
        }
    }
}
