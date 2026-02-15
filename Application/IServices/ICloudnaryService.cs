using Application.Shared;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace Application.IServices
{
    public interface ICloudnaryService
    {
        Task<GenericResult<List<ImageUploadResult>>> UploadImagesAsync(List<IFormFile> media, CancellationToken cancellationToken);
        Task<GenericResult<List<VideoUploadResult>>> UploadVideosAsync(List<IFormFile> media, CancellationToken cancellationToken);
        Task<GenericResult<GetResourceResult>> GetMediaAsync(string publicId, ResourceType resourceType, CancellationToken cancellationToken);
        Task<GenericResult<List<CloudinaryDotNet.Actions.Resource>>> GetMediaByTagAsync(string tag, ResourceType resourceType, CancellationToken cancellationToken);
        Task<GenericResult<List<CloudinaryDotNet.Actions.Resource>>> ListMediaAsync(ResourceType resourceType, int maxResults = 10, CancellationToken cancellationToken = default);
        /// <summary>
        /// Synchronous - just generates URL
        /// </summary>
        /// <param name="publicId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        Task<GenericResult<string>> GetMediaUrl(string publicId, ResourceType resourceType);
    }
}
