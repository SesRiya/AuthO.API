﻿using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IPasswordHash, PasswordHash>();
            services.AddScoped<RefreshTokenValidator>();
            services.AddScoped<TokenGenerator>();
            services.AddScoped<AccessTokenGenerator>();
            services.AddScoped<RefreshTokenGenerator>();
            services.AddScoped<Authenticator>();

        }
    }
}

