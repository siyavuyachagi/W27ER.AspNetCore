using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a video resource (mp4, mov, avi, etc.)
    /// Inherits all Cloudinary properties from Resource
    /// </summary>
    public class Video : Resource
    {
    }
}
