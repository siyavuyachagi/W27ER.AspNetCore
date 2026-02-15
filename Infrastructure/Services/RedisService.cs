using Application.IServices;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisSettings _redisSettings;
        private readonly ILogger<RedisService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RedisService(
            IDistributedCache cache,
            IOptions<RedisSettings> redisSettings,
            ILogger<RedisService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _redisSettings = redisSettings.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private int? CurrentUserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return null;

                return int.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        private string GetKey(string key) => $"{_redisSettings.InstanceName}{(CurrentUserId.HasValue ? $":user:{CurrentUserId.Value}" : "")}:{key}";

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(GetKey(key));
                if (string.IsNullOrEmpty(value))
                    return default;

                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return default;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                if (value is null) return false; // Do not cache null

                var options = new DistributedCacheEntryOptions();
                if (expiry.HasValue)
                    options.AbsoluteExpirationRelativeToNow = expiry;

                var json = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(GetKey(key), json, options);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(GetKey(key));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
        {
            try
            {
                var cached = await GetAsync<T>(key);
                if (cached != null) return cached;

                var value = await factory();

                await SetAsync(key, value, expiry);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return default;
            }
        }
    }
}
