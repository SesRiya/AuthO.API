using ApiCore;
using Authorization.Authorization;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware.Claims;
using Middleware.Cookie;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace ClientApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDistributedMemoryCache();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                AuthenticationConfiguration authenticationConfiguration = new();

                Configuration.Bind("Authentication", authenticationConfiguration);

                //adding keyVault for the accesstoken secret
                SecretClient keyVaultClient = new(
                    new Uri(Configuration.GetValue<string>("KeyVaultUri")),
                    new DefaultAzureCredential());
                authenticationConfiguration.AccessTokenKey = keyVaultClient.GetSecret("access-token-secret").Value.Value;


                options.SaveToken = true;
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

            // Register our claims based authorization handler.
            services.AddClaimsBasedAuthorization();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    policyBuilder =>
                        policyBuilder.AddRequirements(
                            new Administrator()
                        ));
                options.AddPolicy("Tester",
                    policyBuilder =>
                        policyBuilder.AddRequirements(
                            new Tester()
                            ));
            });

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
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //if cookies are in the serverside pass them as authentication
            app.UseCookieAsBearerToken();

            app.UseAuthentication();

            app.UseClaimsAugmentation();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }

        private class AuthenticationConfiguration
        {
            public string? AccessTokenKey { get; set; }
            public string? Issuer { get; set; }
            public string? Audience { get; set; }
        }
    }



}

