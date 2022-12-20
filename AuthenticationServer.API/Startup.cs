using AuthenticationServer.API.Models;
using AuthenticationServer.API.Services.Authenticators;
using AuthenticationServer.API.Services.Authorization;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenGenerator;
using AuthenticationServer.API.Services.TokenValidators;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;

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

            services.AddScoped<TokenGenerator>();
            services.AddScoped<AccessTokenGenerator>();
            services.AddScoped<RefreshTokenGenerator>();
            services.AddScoped<RefreshTokenValidator>();
            services.AddScoped<ITempRefreshTokenRepository, TempRefreshTokenRepository>();
            services.AddScoped<Authenticator>();


            //instantiate and bind authentication values to authen config object(appsettings.json)
            AuthenticationConfig authenticationConfiguration = new();
            _configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddScoped<IPasswordHash, PasswordHash>();
            services.AddSingleton<ITempUserRepository, TempUserRepository>();

            //for authorization controller to get access to the generated tokens
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //AuthenticationConfig authenticationConfiguration = new AuthenticationConfig();
                _configuration.Bind("Authentication", authenticationConfiguration);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenKey)),
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudience = authenticationConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


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

            //services.AddAuthorization(options =>
            //{
            //    var scopes = new[] {
            //    "administrator",
            //    "returnstaxofficer",
            //    "paymentofficer",
            //    "string"
            //};

            //    Array.ForEach(scopes, scope =>
            //      options.AddPolicy(scope,
            //        policy => policy.Requirements.Add(
            //          new ScopeRequirement(authenticationConfiguration.Issuer, scope)
            //        )
            //      )
            //    );
            //});


            services.AddSingleton<IAuthorizationHandler, IsAllowedToGetData>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    policyBuilder =>
                        policyBuilder.AddRequirements(
                            new Administrator()
                        ));
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
