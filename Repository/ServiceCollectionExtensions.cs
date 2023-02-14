using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace Repository
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();   
        }
    }
}
