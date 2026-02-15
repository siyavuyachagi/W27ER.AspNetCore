using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace Domain.Entities
{
    /// <summary>
    /// Represents an image resource (jpg, png, gif, webp, etc.)
    /// Inherits all Cloudinary properties from Resource
    /// </summary>
    [Table("Images")]
    public class Image : Resource
    {
    }
}
