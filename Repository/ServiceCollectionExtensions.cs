using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace Repository
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }
}
