using Application.IRepositories;
using Application.IServices;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Infrastructure.Services;
using StackExchange.Redis;

namespace Ward27EmploymentRegistry.Dependencies
{
    public static class IServices
    {
        public static IServiceCollection AddIServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IRedisService, RedisService>();
            //services.AddScoped<IUserRepository, UserRepository>();

            var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>() 
                ?? throw new InvalidOperationException("Error mapping RedisSettings configuration.");

            var redisConfig = ConfigurationOptions.Parse(redisSettings.Connection, true);
            redisConfig.AbortOnConnectFail = redisSettings.AbortOnConnectFail;
            redisConfig.ConnectTimeout = redisSettings.ConnectionTimeout;
            redisConfig.AsyncTimeout = redisSettings.AsyncTimeout;
            redisConfig.SyncTimeout = redisSettings.SyncTimeout;
            redisConfig.ConnectRetry = redisSettings.ConnectRetry;
            redisConfig.ReconnectRetryPolicy = new ExponentialRetry(5000, 10000);
            redisConfig.KeepAlive = 60;

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = redisSettings.InstanceName;
                options.ConfigurationOptions = redisConfig;
            });

            return services;
        }
    }
}
