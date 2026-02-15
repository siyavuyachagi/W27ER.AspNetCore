using Application.Shared;
using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;
using Video = Domain.Entities.Video;

namespace Application.IServices
{
    public interface ICloudinaryService
    {
        // ============================================
        // IMAGE UPLOAD
        // ============================================
        Task<GenericResult<List<Image>>> UploadImagesAsync(
            List<IFormFile> files,
            Guid userId,
            string folder = "w27er/images",
            CancellationToken cancellationToken = default);

        Task<GenericResult<Image>> UploadImageAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/images",
            CancellationToken cancellationToken = default);

        // ============================================
        // VIDEO UPLOAD
        // ============================================
        Task<GenericResult<List<Video>>> UploadVideosAsync(
            List<IFormFile> files,
            Guid userId,
            string folder = "w27er/videos",
            CancellationToken cancellationToken = default);

        Task<GenericResult<Video>> UploadVideoAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/videos",
            CancellationToken cancellationToken = default);

        // ============================================
        // DOCUMENT UPLOAD
        // ============================================
        Task<GenericResult<Document>> UploadDocumentAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/documents",
            CancellationToken cancellationToken = default);

        // ============================================
        // IMAGE TRANSFORMATION METHODS
        // ============================================

        /// <summary>
        /// Gets Cloudinary URL with specific transformations
        /// </summary>
        string GetTransformedUrl(string secureUrl, string transformation);

        /// <summary>
        /// Gets thumbnail URL with specific size
        /// </summary>
        string GetThumbnailUrl(string secureUrl, int width = 200, int height = 200);

        /// <summary>
        /// Gets optimized image URL for web display
        /// </summary>
        string GetOptimizedImageUrl(string secureUrl, int maxWidth = 1200, string quality = "auto");

        /// <summary>
        /// Gets responsive image URLs for different screen sizes
        /// </summary>
        Dictionary<string, string> GetResponsiveImageUrls(string secureUrl);

        // ============================================
        // VIDEO TRANSFORMATION METHODS
        // ============================================

        /// <summary>
        /// Gets video poster/thumbnail image URL
        /// </summary>
        string GetVideoPosterUrl(string secureUrl, int width = 800, int height = 450);

        /// <summary>
        /// Gets video URL with quality optimization
        /// </summary>
        string GetOptimizedVideoUrl(string secureUrl, string quality = "auto");

        /// <summary>
        /// Gets adaptive streaming URLs for different qualities
        /// </summary>
        Dictionary<string, string> GetStreamingUrls(string secureUrl);

        /// <summary>
        /// Gets video preview (first few seconds)
        /// </summary>
        string GetVideoPreviewUrl(string secureUrl, int durationSeconds = 5);

        // ============================================
        // DOCUMENT TRANSFORMATION METHODS
        // ============================================

        /// <summary>
        /// Gets document preview image (for PDFs)
        /// </summary>
        string GetDocumentPreviewImageUrl(string secureUrl, int page = 1, int width = 600);

        /// <summary>
        /// Gets all page preview URLs (for PDFs)
        /// </summary>
        List<string> GetAllDocumentPagePreviews(string secureUrl, int pageCount, int width = 600);

        /// <summary>
        /// Gets downloadable URL with original filename
        /// </summary>
        string GetDownloadUrl(string secureUrl, string fileName);

        // ============================================
        // UTILITY METHODS
        // ============================================

        /// <summary>
        /// Format file size in human-readable format
        /// </summary>
        string FormatFileSize(long fileSizeBytes);

        /// <summary>
        /// Format video duration in human-readable format
        /// </summary>
        string FormatDuration(double durationSeconds);

        // ============================================
        // DELETE MEDIA
        // ============================================
        Task<GenericResult<bool>> DeleteMediaAsync(
            string publicId,
            ResourceType resourceType,
            CancellationToken cancellationToken = default);
    }
}