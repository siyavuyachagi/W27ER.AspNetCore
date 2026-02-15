using Application.IServices;
using Application.Shared;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace Infrastructure.Services
{
    public class CloudnaryService : ICloudnaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudnaryService> _logger;
        public CloudnaryService(
            Cloudinary cloudinary, 
            ILogger<CloudnaryService> logger)
        {
            _cloudinary = cloudinary;
            _logger = logger;
        }


        public async Task<GenericResult<List<ImageUploadResult>>> UploadImagesAsync(List<IFormFile> files, CancellationToken cancellationToken = default)
        {
            try
            {
                List<ImageUploadResult> result = new List<ImageUploadResult>();
                foreach (IFormFile fileItem in files)
                {
                    var uploadResult = await UploadImageAsync(fileItem, cancellationToken);
                    if (!uploadResult.Succeeded)
                        _logger.LogError("File upload failed: {Errors}", string.Join(", ", uploadResult.Errors));
                    else
                        result.Add(uploadResult.Data);
                }
                return GenericResult<List<ImageUploadResult>>.Success(data: result);
            }
            catch (Exception ex)
            {
                return GenericResult<List<ImageUploadResult>>.Failure(errors: new List<string> { ex.Message });
            }
        }

        private async Task<GenericResult<ImageUploadResult>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "w27er/images",
                    Transformation = new Transformation()
                        .Width(1200).Height(800).Crop("limit")
                        .Quality("auto")
                };
                var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

                if (result.Error != null)
                {
                    return GenericResult<ImageUploadResult>.Failure(errors: new List<string> { result.Error.Message });
                }

                return GenericResult<ImageUploadResult>.Success(data: result);
            }
            catch (Exception ex)
            {
                return GenericResult<ImageUploadResult>.Failure(errors: new List<string> { ex.Message });
            }
        }

        public async Task<GenericResult<List<VideoUploadResult>>> UploadVideosAsync(List<IFormFile> files, CancellationToken cancellationToken = default)
        {
            try
            {
                List<VideoUploadResult> result = new List<VideoUploadResult>();
                foreach (IFormFile fileItem in files)
                {
                    var uploadResult = await UploadVideoAsync(fileItem, cancellationToken);
                    if (!uploadResult.Succeeded)
                        _logger.LogError("File upload failed: {Errors}", string.Join(", ", uploadResult.Errors));
                    else
                        result.Add(uploadResult.Data);
                }
                return GenericResult<List<VideoUploadResult>>.Success(data: result);
            }
            catch (Exception ex)
            {
                return GenericResult<List<VideoUploadResult>>.Failure(errors: new List<string> { ex.Message });
            }
        }

        private async Task<GenericResult<VideoUploadResult>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                using var stream = file.OpenReadStream();

                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "w27er/videos",
                    Transformation = new Transformation()
                        .Width(1920).Height(1080).Crop("limit")
                        .Quality("auto")
                };
                var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

                if (result.Error != null)
                {
                    return GenericResult<VideoUploadResult>.Failure(errors: new List<string> { result.Error.Message });
                }

                return GenericResult<VideoUploadResult>.Success(data: result);
            }
            catch (Exception ex)
            {
                return GenericResult<VideoUploadResult>.Failure(errors: new List<string> { ex.Message });
            }
        }
        public async Task<GenericResult<GetResourceResult>> GetMediaAsync(string publicId, ResourceType resourceType, CancellationToken cancellationToken)
        {
            try
            {
                var getResourceParams = new GetResourceParams(publicId)
                {
                    ResourceType = resourceType
                };

                var result = await _cloudinary.GetResourceAsync(getResourceParams, cancellationToken);

                if (result.Error != null)
                {
                    return GenericResult<GetResourceResult>.Failure(errors: [result.Error.Message]);
                }

                return GenericResult<GetResourceResult>.Success(data: result);
            }
            catch (Exception ex)
            {
                return GenericResult<GetResourceResult>.Failure(errors: new List<string> { ex.Message });
            }
        }

        public async Task<GenericResult<List<CloudinaryDotNet.Actions.Resource>>> GetMediaByTagAsync(string tag, ResourceType resourceType, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _cloudinary.ListResourcesByTagAsync(tag, null, cancellationToken);

                if (result.Error != null)
                {
                    return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { result.Error.Message });
                }

                if (result.Resources == null || !result.Resources.Any())
                {
                    return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { "No media found with the specified tag" });
                }

                return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Success(data: result.Resources.ToList());
            }
            catch (Exception ex)
            {
                return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { ex.Message });
            }
        }

        public Task<GenericResult<string>> GetMediaUrl(
            string publicId,
            ResourceType resourceType)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return Task.FromResult(
                    GenericResult<string>.Failure("PublicId cannot be empty."));

            string url = resourceType == ResourceType.Video
                ? _cloudinary.Api.UrlVideoUp.BuildUrl(publicId)
                : _cloudinary.Api.UrlImgUp.BuildUrl(publicId);

            return Task.FromResult(GenericResult<string>.Success(url));
        }

        public async Task<GenericResult<List<CloudinaryDotNet.Actions.Resource>>> ListMediaAsync(ResourceType resourceType, int maxResults = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var listParams = new ListResourcesParams
                {
                    ResourceType = resourceType,
                    MaxResults = maxResults
                };

                var result = await _cloudinary.ListResourcesAsync(listParams, cancellationToken);

                if (result.Error != null)
                {
                    return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { result.Error.Message });
                }

                if (result.Resources == null || !result.Resources.Any())
                {
                    return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { "No media found" });
                }

                return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Success(data: result.Resources.ToList());
            }
            catch (Exception ex)
            {
                return GenericResult<List<CloudinaryDotNet.Actions.Resource>>.Failure(errors: new List<string> { ex.Message });
            }
        }
    }
}
