using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace Domain.Entities
{
    /// <summary>
    /// Base entity for all media resources (Images, Videos, Files) with Cloudinary integration
    /// </summary>
    public class Resource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Original filename
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// File extension without dot (e.g., "jpg", "pdf", "mp4")
        /// </summary>
        public string Extension { get; set; } = default!;

        /// <summary>
        /// Type of resource (Image, Video, Document, Audio)
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// MIME type (e.g., "image/jpeg", "video/mp4", "application/pdf")
        /// </summary>
        public string MimeType { get; set; } = default!;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSizeBytes { get; set; }

        // ============================================
        // CLOUDINARY INTEGRATION PROPERTIES
        // ============================================

        /// <summary>
        /// Cloudinary Public ID (used for all operations)
        /// </summary>
        public string CloudinaryPublicId { get; set; } = default!;

        /// <summary>
        /// Full Cloudinary URL
        /// </summary>
        public string CloudinaryUrl { get; set; } = default!;

        /// <summary>
        /// Cloudinary Secure URL (HTTPS)
        /// </summary>
        public string CloudinarySecureUrl { get; set; } = default!;

        /// <summary>
        /// Cloudinary resource type (image, video, raw)
        /// </summary>
        public string CloudinaryResourceType { get; set; } = default!;

        /// <summary>
        /// Cloudinary format (jpg, png, mp4, pdf, etc.)
        /// </summary>
        public string CloudinaryFormat { get; set; } = default!;

        /// <summary>
        /// Cloudinary version number
        /// </summary>
        public string? CloudinaryVersion { get; set; }

        /// <summary>
        /// Cloudinary asset ID
        /// </summary>
        public string? CloudinaryAssetId { get; set; }

        /// <summary>
        /// Cloudinary folder path
        /// </summary>
        public string? CloudinaryFolder { get; set; }

        // ============================================
        // IMAGE-SPECIFIC PROPERTIES
        // ============================================

        /// <summary>
        /// Image width in pixels (for images only)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels (for images only)
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Aspect ratio (for images/videos)
        /// </summary>
        public double? AspectRatio { get; set; }

        /// <summary>
        /// Thumbnail URL (Cloudinary transformation)
        /// </summary>
        public string? ThumbnailUrl { get; set; }

        // ============================================
        // VIDEO-SPECIFIC PROPERTIES
        // ============================================

        /// <summary>
        /// Video duration in seconds (for videos only)
        /// </summary>
        public double? DurationSeconds { get; set; }

        /// <summary>
        /// Video bitrate (for videos only)
        /// </summary>
        public int? BitRate { get; set; }

        /// <summary>
        /// Video codec (e.g., "h264", "vp9")
        /// </summary>
        public string? VideoCodec { get; set; }

        /// <summary>
        /// Audio codec (e.g., "aac", "mp3")
        /// </summary>
        public string? AudioCodec { get; set; }

        /// <summary>
        /// Frame rate (for videos)
        /// </summary>
        public double? FrameRate { get; set; }

        // ============================================
        // METADATA & TAGS
        // ============================================

        /// <summary>
        /// Tags for categorization (comma-separated)
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// Alt text for accessibility
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// Description of the resource
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Cloudinary transformation applied (if any)
        /// </summary>
        public string? Transformation { get; set; }

        // ============================================
        // OWNERSHIP & RELATIONSHIPS
        // ============================================

        /// <summary>
        /// User who uploaded the resource
        /// </summary>
        public Guid UploadedByUserId { get; set; }
        [ForeignKey(nameof(UploadedByUserId))]
        public virtual ApplicationUser UploadedByUser { get; set; } = default!;

        /// <summary>
        /// Related entity ID (e.g., ResidentProfileId, WorkOpportunityId)
        /// </summary>
        public Guid? RelatedEntityId { get; set; }

        /// <summary>
        /// Related entity type (e.g., "ResidentProfile", "WorkOpportunity")
        /// </summary>
        public string? RelatedEntityType { get; set; }

        // ============================================
        // STATUS & SECURITY
        // ============================================

        /// <summary>
        /// Whether resource is publicly accessible
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Whether resource has been verified/approved
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Date resource was verified
        /// </summary>
        public DateTime? VerifiedAt { get; set; }

        /// <summary>
        /// User who verified the resource
        /// </summary>
        public Guid? VerifiedByUserId { get; set; }
        [ForeignKey(nameof(VerifiedByUserId))]
        public virtual ApplicationUser? VerifiedByUser { get; set; }

        // ============================================
        // AUDIT FIELDS
        // ============================================

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // ============================================
        // COMPUTED PROPERTIES
        // ============================================

        /// <summary>
        /// Full filename with extension
        /// </summary>
        [NotMapped]
        public string FullFileName => $"{Name}.{Extension}";

        /// <summary>
        /// File size in human-readable format
        /// </summary>
        [NotMapped]
        public string FileSizeFormatted
        {
            get
            {
                if (FileSizeBytes < 1024)
                    return $"{FileSizeBytes} B";
                if (FileSizeBytes < 1024 * 1024)
                    return $"{FileSizeBytes / 1024.0:F2} KB";
                if (FileSizeBytes < 1024 * 1024 * 1024)
                    return $"{FileSizeBytes / (1024.0 * 1024.0):F2} MB";
                return $"{FileSizeBytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
            }
        }

        /// <summary>
        /// Video duration in human-readable format
        /// </summary>
        [NotMapped]
        public string? DurationFormatted
        {
            get
            {
                if (!DurationSeconds.HasValue) return null;

                var duration = TimeSpan.FromSeconds(DurationSeconds.Value);
                if (duration.TotalHours >= 1)
                    return duration.ToString(@"hh\:mm\:ss");
                return duration.ToString(@"mm\:ss");
            }
        }
    }
}
