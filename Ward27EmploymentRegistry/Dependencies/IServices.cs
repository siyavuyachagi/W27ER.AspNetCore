using Application.IServices;
using Infrastructure.Services;

namespace Ward27EmploymentRegistry.Dependencies
{
    public static class IServices
    {
        public static IServiceCollection AddIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
