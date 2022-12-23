using AuthenticationServer.API.Services.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using static System.Net.WebRequestMethods;

namespace DataServer.API
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                  {
                    AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
                    Configuration.Bind("Authentication", authenticationConfiguration);
                      options.SaveToken = true;
                      options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessToken)),
                        ValidIssuer = authenticationConfiguration.Issuer,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                  });

            services.AddScoped<IAuthorizationHandler, IsAllowedAccessToAll>();
            services.AddScoped<IAuthorizationHandler, IsAllowedAccessToReturnsPage>();
            services.AddScoped<IAuthorizationHandler, IsAllowedAccessToPaymentsPage>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    policyBuilder =>
                        policyBuilder.AddRequirements(
                            new Administrator()
                        ));
                options.AddPolicy("returns",
                    policyBuilder =>
                        policyBuilder.AddRequirements(
                            new ReturnsOfficer()    
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

        private class AuthenticationConfiguration
        {
            public string AccessToken { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
        }
    }



}

