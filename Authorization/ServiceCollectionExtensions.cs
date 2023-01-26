using Authorization.ClaimsTransformation;
using Authorization.ClaimsUserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{

    public static class ServiceCollectionExtensions
    {
        public static void AddCustomClaimstoIdentity(this IServiceCollection services)
        {
            services.AddScoped<IClaimsTransformation, ClaimsAddition>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}

