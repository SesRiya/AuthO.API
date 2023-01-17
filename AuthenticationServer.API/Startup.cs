using AuthenticationServer.API.Models;
using AuthenticationServer.API.Services.Authenticators;
using AuthenticationServer.API.Services.ControllerMethod;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenGenerator;
using AuthenticationServer.API.Services.TokenValidators;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebModels;
using Repository;
using ApiCore;
using Services;
using AuthenticationConfig = WebModels.AuthenticationConfig;

namespace AuthenticationServer.API
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container: dependency injection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddScoped<ITokenGenerator, TokenGenerator>();
            //services.AddScoped<AccessTokenGenerator>();
            //services.AddScoped<RefreshTokenGenerator>();
            //services.AddScoped<RefreshTokenValidator>();
            //services.AddScoped<Authenticator>();
            //services.AddScoped<IRegisterUser, RegisterUser>();
            //services.AddScoped<ILoginAuthentication, LoginAuthentication>();
            //services.AddScoped<IRefreshTokenVerification, RefreshTokenVerification>();

            //instantiate and bind authentication values to authen config object(appsettings.json)
            AuthenticationConfig authenticationConfiguration = new();
            _configuration.Bind("Authentication", authenticationConfiguration);

            services.AddSingleton(authenticationConfiguration);
            services.AddScoped<IPasswordHash, PasswordHash>();

            //services.AddSingleton<ITempRefreshTokenRepository, TempRefreshTokenRepository>();
            //services.AddSingleton<ITempUserRepository, TempUserRepository>();

            services.AddRepository();
            services.AddApiCore();
            services.AddServices();

            services.AddHttpContextAccessor();
            // Register our authorization handler.


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using bearer scheme(\"bearer {token}",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build();
                });
            });

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRouting();
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
