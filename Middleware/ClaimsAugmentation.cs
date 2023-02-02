using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Middleware.Interface;
using Repository.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Middleware
{
    public class ClaimsAugmentation
    {
        #region fields
        private readonly RequestDelegate _next;
        #endregion

        #region constructor
        public ClaimsAugmentation(
            RequestDelegate next
            )
        {
            _next = next;
        }
        #endregion


        //get roles from request
        //public static async Task<string> Token()
        //{
        //    HttpClient client = new HttpClient();
        //    string token;
        //    var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7248/WeatherForecast");
        //    HttpResponseMessage response = await client.SendAsync(request);

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        token = await response.Content.ReadAsStringAsync();
        //    }
        //    else
        //    {
        //        token = null;
        //    }

        //    return (token);
        //}



        #region methods

        //public static async Task<List<string>> GetRolesAsync()
        //{
        //    HttpClient client = new HttpClient();
        //    List<string> roles = new List<string>();
        //    //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7268/api/AuthO");
        //    //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

        //    //HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        //    HttpResponseMessage response = await client.GetAsync("https://localhost:7248/Home");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        roles = await response.Content.ReadFromJsonAsync<List<string>>();
        //    }
        //    return (roles);
        //}


        public static async Task<List<string>> GetRoleAsync(string path)
        {
            HttpClient client = new HttpClient();
            List<string>? roles = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                roles = await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return roles;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ClaimsPrincipal principal = context.User;

            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                Claim idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

                //List<string> roles = await _userRoleRepository.GetAllRoles(Guid.Parse(idClaim.Value));


                List<string> roles = await GetRoleAsync("https://localhost:7268/api/AuthO");

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


}

