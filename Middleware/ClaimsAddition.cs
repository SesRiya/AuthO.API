using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Middleware
{
    public class ClaimsAddition
    {
        #region fields
        private readonly RequestDelegate _next;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region constructor
        public ClaimsAddition(
            RequestDelegate next,
            IUserRoleRepository userRoleRepository
            )
        {
            _next = next;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        #region methods

        //public static async Task<List<string>> GetRoleAsync(string path)
        //{
        //    HttpClient client = new HttpClient();
        //    List<string> roles = null;
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        roles = await response.Content.ReadFromJsonAsync<List<string>>();
        //    }
        //    return roles;
        //}

        public async Task InvokeAsync(HttpContext context)
        {
            ClaimsPrincipal principal = context.User;

            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                Claim idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

                List<string> roles = await _userRoleRepository.GetAllRoles(Guid.Parse(idClaim.Value));

                //List<string> roles = await GetRoleAsync("https://localhost:7268/api/AuthO");

                ClaimsPrincipal clonedPrincipal = principal.Clone();
                ClaimsIdentity clonedIdentity = (ClaimsIdentity)clonedPrincipal.Identity;

                foreach (string role in roles)
                {
                    clonedIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
                }
            }
            await _next(context);
        }
        #endregion
    }


    #region static methods
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseClaimsAddition(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsAddition>();
        }
    }
    #endregion

   
}

