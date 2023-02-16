using AuthenticationServerEntityFramework;
using Authorization.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddClaimsBasedAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AdminAccess>();
            services.AddScoped<IAuthorizationHandler, TesterAccess>();
            services.AddScoped<IAuthorizationHandler, AdminOrTesterAccess>();
        }
    }
}

