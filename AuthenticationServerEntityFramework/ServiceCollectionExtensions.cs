using AuthenticationServerEntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationServerDbContext>();
          

        }
    }
}

