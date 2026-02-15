using Application.IServices;
using Application.Shared;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;
using Video = Domain.Entities.Video;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private readonly ApplicationDbContext _db;

        public CloudinaryService(
            Cloudinary cloudinary,
            ILogger<CloudinaryService> logger,
            ApplicationDbContext db)
        {
            _cloudinary = cloudinary;
            _logger = logger;
            _db = db;
        }

        #region Image Upload

        public async Task<GenericResult<List<Image>>> UploadImagesAsync(
            List<IFormFile> files,
            Guid userId,
            string folder = "w27er/images",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var images = new List<Image>();

                foreach (var file in files)
                {
                    var result = await UploadImageAsync(file, userId, folder, cancellationToken);
                    if (result.Succeeded && result.Data != null)
                    {
                        images.Add(result.Data);
                    }
                    else
                    {
                        _logger.LogError("Image upload failed: {Errors}", string.Join(", ", result.Errors));
                    }
                }

                return GenericResult<List<Image>>.Success(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading images");
                return GenericResult<List<Image>>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<Image>> UploadImageAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/images",
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    Transformation = new Transformation()
                        .Width(1200).Height(800).Crop("limit")
                        .Quality("auto")
                };

                var cloudinaryResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

                if (cloudinaryResult.Error != null)
                {
                    return GenericResult<Image>.Failure(cloudinaryResult.Error.Message);
                }

                // Create Image entity
                var image = new Image
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = Path.GetExtension(file.FileName).TrimStart('.'),
                    MimeType = file.ContentType,
                    FileSizeBytes = cloudinaryResult.Bytes,

                    CloudinaryPublicId = cloudinaryResult.PublicId,
                    CloudinaryUrl = cloudinaryResult.Url.ToString(),
                    CloudinarySecureUrl = cloudinaryResult.SecureUrl.ToString(),
                    CloudinaryResourceType = cloudinaryResult.ResourceType,
                    CloudinaryFormat = cloudinaryResult.Format,
                    CloudinaryVersion = cloudinaryResult.Version,
                    CloudinaryAssetId = cloudinaryResult.AssetId,
                    CloudinaryFolder = folder,

                    Width = cloudinaryResult.Width,
                    Height = cloudinaryResult.Height,
                    AspectRatio = cloudinaryResult.Height > 0
                        ? (double)cloudinaryResult.Width / cloudinaryResult.Height
                        : null,

                    UploadedByUserId = userId,
                    ThumbnailUrl = GetThumbnailUrl(cloudinaryResult.SecureUrl.ToString(), 200, 200)
                };

                _db.Images.Add(image);
                await _db.SaveChangesAsync(cancellationToken);

                return GenericResult<Image>.Success(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return GenericResult<Image>.Failure(ex.Message);
            }
        }

        #endregion

        #region Video Upload

        public async Task<GenericResult<List<Video>>> UploadVideosAsync(
            List<IFormFile> files,
            Guid userId,
            string folder = "w27er/videos",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var videos = new List<Video>();

                foreach (var file in files)
                {
                    var result = await UploadVideoAsync(file, userId, folder, cancellationToken);
                    if (result.Succeeded && result.Data != null)
                    {
                        videos.Add(result.Data);
                    }
                    else
                    {
                        _logger.LogError("Video upload failed: {Errors}", string.Join(", ", result.Errors));
                    }
                }

                return GenericResult<List<Video>>.Success(videos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading videos");
                return GenericResult<List<Video>>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<Video>> UploadVideoAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/videos",
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    Transformation = new Transformation()
                        .Width(1920).Height(1080).Crop("limit")
                        .Quality("auto")
                };

                var cloudinaryResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

                if (cloudinaryResult.Error != null)
                {
                    return GenericResult<Video>.Failure(cloudinaryResult.Error.Message);
                }

                var video = new Video
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = Path.GetExtension(file.FileName).TrimStart('.'),
                    MimeType = file.ContentType,
                    FileSizeBytes = cloudinaryResult.Bytes,

                    CloudinaryPublicId = cloudinaryResult.PublicId,
                    CloudinaryUrl = cloudinaryResult.Url.ToString(),
                    CloudinarySecureUrl = cloudinaryResult.SecureUrl.ToString(),
                    CloudinaryResourceType = cloudinaryResult.ResourceType,
                    CloudinaryFormat = cloudinaryResult.Format,
                    CloudinaryVersion = cloudinaryResult.Version,
                    CloudinaryAssetId = cloudinaryResult.AssetId,
                    CloudinaryFolder = folder,

                    Width = cloudinaryResult.Width,
                    Height = cloudinaryResult.Height,
                    DurationSeconds = cloudinaryResult.Duration,
                    //VideoCodec = cloudinaryResult.VideoCodec,
                    //AudioCodec = cloudinaryResult.AudioCodec,
                    BitRate = cloudinaryResult.BitRate,
                    FrameRate = cloudinaryResult.FrameRate,

                    UploadedByUserId = userId,
                    ThumbnailUrl = GetVideoPosterUrl(cloudinaryResult.SecureUrl.ToString(), 800, 450)
                };

                _db.Videos.Add(video);
                await _db.SaveChangesAsync(cancellationToken);

                return GenericResult<Video>.Success(video);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video");
                return GenericResult<Video>.Failure(ex.Message);
            }
        }

        #endregion

        #region Document Upload

        public async Task<GenericResult<Document>> UploadDocumentAsync(
            IFormFile file,
            Guid userId,
            string folder = "w27er/documents",
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder
                };

                var cloudinaryResult = await _cloudinary.UploadAsync(uploadParams, default, cancellationToken);

                if (cloudinaryResult.Error != null)
                {
                    return GenericResult<Document>.Failure(cloudinaryResult.Error.Message);
                }

                var document = new Document
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = Path.GetExtension(file.FileName).TrimStart('.'),
                    MimeType = file.ContentType,
                    FileSizeBytes = cloudinaryResult.Bytes,

                    CloudinaryPublicId = cloudinaryResult.PublicId,
                    CloudinaryUrl = cloudinaryResult.Url.ToString(),
                    CloudinarySecureUrl = cloudinaryResult.SecureUrl.ToString(),
                    CloudinaryResourceType = "raw",
                    CloudinaryFormat = cloudinaryResult.Format,
                    CloudinaryVersion = cloudinaryResult.Version,
                    CloudinaryAssetId = cloudinaryResult.AssetId,
                    CloudinaryFolder = folder,

                    //PageCount = cloudinaryResult.Pages,

                    UploadedByUserId = userId
                };

                _db.Documents.Add(document);
                await _db.SaveChangesAsync(cancellationToken);

                return GenericResult<Document>.Success(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                return GenericResult<Document>.Failure(ex.Message);
            }
        }

        #endregion

        #region Image Transformation Methods

        /// <summary>
        /// Gets Cloudinary URL with specific transformations
        /// </summary>
        public string GetTransformedUrl(string secureUrl, string transformation)
        {
            if (string.IsNullOrWhiteSpace(transformation))
                return secureUrl;

            var urlParts = secureUrl.Split(new[] { "/upload/" }, StringSplitOptions.None);
            if (urlParts.Length == 2)
                return $"{urlParts[0]}/upload/{transformation}/{urlParts[1]}";

            return secureUrl;
        }

        /// <summary>
        /// Gets thumbnail URL with specific size
        /// </summary>
        public string GetThumbnailUrl(string secureUrl, int width = 200, int height = 200)
        {
            return GetTransformedUrl(secureUrl, $"w_{width},h_{height},c_fill,q_auto");
        }

        /// <summary>
        /// Gets optimized image URL for web display
        /// </summary>
        public string GetOptimizedImageUrl(string secureUrl, int maxWidth = 1200, string quality = "auto")
        {
            return GetTransformedUrl(secureUrl, $"w_{maxWidth},c_limit,q_{quality},f_auto");
        }

        /// <summary>
        /// Gets responsive image URLs for different screen sizes
        /// </summary>
        public Dictionary<string, string> GetResponsiveImageUrls(string secureUrl)
        {
            return new Dictionary<string, string>
            {
                { "thumbnail", GetThumbnailUrl(secureUrl, 200, 200) },
                { "small", GetTransformedUrl(secureUrl, "w_400,c_limit,q_auto") },
                { "medium", GetTransformedUrl(secureUrl, "w_800,c_limit,q_auto") },
                { "large", GetTransformedUrl(secureUrl, "w_1200,c_limit,q_auto") },
                { "original", secureUrl }
            };
        }

        #endregion

        #region Video Transformation Methods

        /// <summary>
        /// Gets video poster/thumbnail image URL
        /// </summary>
        public string GetVideoPosterUrl(string secureUrl, int width = 800, int height = 450)
        {
            return GetTransformedUrl(secureUrl, $"w_{width},h_{height},c_fill,so_0,f_jpg");
        }

        /// <summary>
        /// Gets video URL with quality optimization
        /// </summary>
        public string GetOptimizedVideoUrl(string secureUrl, string quality = "auto")
        {
            return GetTransformedUrl(secureUrl, $"q_{quality},vc_auto");
        }

        /// <summary>
        /// Gets adaptive streaming URLs for different qualities
        /// </summary>
        public Dictionary<string, string> GetStreamingUrls(string secureUrl)
        {
            return new Dictionary<string, string>
            {
                { "low", GetTransformedUrl(secureUrl, "q_auto:low,vc_auto") },
                { "medium", GetTransformedUrl(secureUrl, "q_auto:good,vc_auto") },
                { "high", GetTransformedUrl(secureUrl, "q_auto:best,vc_auto") },
                { "original", secureUrl }
            };
        }

        /// <summary>
        /// Gets video preview (first few seconds)
        /// </summary>
        public string GetVideoPreviewUrl(string secureUrl, int durationSeconds = 5)
        {
            return GetTransformedUrl(secureUrl, $"du_{durationSeconds},q_auto");
        }

        #endregion

        #region Document Transformation Methods

        /// <summary>
        /// Gets document preview image (for PDFs)
        /// </summary>
        public string GetDocumentPreviewImageUrl(string secureUrl, int page = 1, int width = 600)
        {
            return GetTransformedUrl(secureUrl, $"pg_{page},w_{width},f_jpg");
        }

        /// <summary>
        /// Gets all page preview URLs (for PDFs)
        /// </summary>
        public List<string> GetAllDocumentPagePreviews(string secureUrl, int pageCount, int width = 600)
        {
            var previews = new List<string>();
            for (int i = 1; i <= pageCount; i++)
            {
                previews.Add(GetDocumentPreviewImageUrl(secureUrl, i, width));
            }
            return previews;
        }

        /// <summary>
        /// Gets downloadable URL with original filename
        /// </summary>
        public string GetDownloadUrl(string secureUrl, string fileName)
        {
            return GetTransformedUrl(secureUrl, $"fl_attachment:{fileName}");
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Format file size in human-readable format
        /// </summary>
        public string FormatFileSize(long fileSizeBytes)
        {
            if (fileSizeBytes < 1024)
                return $"{fileSizeBytes} B";
            if (fileSizeBytes < 1024 * 1024)
                return $"{fileSizeBytes / 1024.0:F2} KB";
            if (fileSizeBytes < 1024 * 1024 * 1024)
                return $"{fileSizeBytes / (1024.0 * 1024.0):F2} MB";
            return $"{fileSizeBytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
        }

        /// <summary>
        /// Format video duration in human-readable format
        /// </summary>
        public string FormatDuration(double durationSeconds)
        {
            var duration = TimeSpan.FromSeconds(durationSeconds);
            if (duration.TotalHours >= 1)
                return duration.ToString(@"hh\:mm\:ss");
            return duration.ToString(@"mm\:ss");
        }

        #endregion

        #region Delete Media

        public async Task<GenericResult<bool>> DeleteMediaAsync(
            string publicId,
            ResourceType resourceType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId)
                {
                    ResourceType = resourceType
                };

                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Error != null)
                {
                    return GenericResult<bool>.Failure(result.Error.Message);
                }

                return GenericResult<bool>.Success(true, "Media deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting media");
                return GenericResult<bool>.Failure(ex.Message);
            }
        }

        #endregion
    }
}