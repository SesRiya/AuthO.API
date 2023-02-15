using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Repository;
using ApiCore;
using Services;
using AuthenticationConfig = Models.AuthenticationConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Authorization.Authorization;
using Middleware;

namespace AuthenticationServer.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container: dependency injection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient();

            //instantiate and bind authentication values to authen config object(appsettings.json)
            AuthenticationConfig authenticationConfiguration = new();
            Configuration.Bind("Authentication", authenticationConfiguration);

            services.AddSingleton(authenticationConfiguration);

            services.AddRepository();
            services.AddApiCore();
            services.AddServices();
            services.AddDbContext();

            services.AddAuthentication(option =>
             {
                 option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(options =>
             {
                 AuthenticationConfig authenticationConfiguration = new();
                 Configuration.Bind("Authentication", authenticationConfiguration);
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

            services.AddHttpContextAccessor();

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

            //app.UseCors();

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