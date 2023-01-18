﻿using ApiCore.Interfaces;
using ApiCore.Login;
using ApiCore.Refresh;
using ApiCore.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiCore(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddScoped<ILoginAuthentication, LoginAuthentication>();
            services.AddScoped<IRefreshTokenVerification, RefreshTokenVerification>();

        }
    }
}

