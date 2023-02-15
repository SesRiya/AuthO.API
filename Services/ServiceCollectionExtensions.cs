using Microsoft.Extensions.DependencyInjection;
using Services.Authenticators;
using Services.Interfaces;
using Services.PasswordHasher;
using Services.TokenGenerators;
using Services.TokenValidators;

namespace Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHash, PasswordHash>();
            services.AddTransient<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddTransient<TokenGenerator>();
            services.AddTransient<AccessTokenGenerator>();
            services.AddTransient<RefreshTokenGenerator>();
            services.AddTransient<IAuthenticator, Authenticator>();
        }
    }
}

