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

        public static async Task<List<string>> GetRolesAsync()
        {
            HttpClient client = new HttpClient();
            List<string> roles = new List<string>();


            //hardcoded token from login request
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZiM2UwMzBiLTY2NWItNDgxZS1iNDU5LTZiOGZmNjc5ODQ5YyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6IkFkbWluQG1haWwuY29tIiwibmJmIjoxNjc1MzE4NzM1LCJleHAiOjE2NzUzMTkzMzUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MjY4In0.pH8FLBjOiFhHKDblNwOOjhLli_QrFT6Fcp5597SETvg";


            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7268/api/AuthO");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            //HttpResponseMessage response = await client.GetAsync("https://localhost:7248/Home");
            if (response.IsSuccessStatusCode)
            {
                roles = await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return (roles);
        }


        //public static async Task<List<string>> GetRoleAsync(string path)
        //{
        //    HttpClient client = new HttpClient();
        //    List<string>? roles = null;
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

                //List<string> roles = await _userRoleRepository.GetAllRoles(Guid.Parse(idClaim.Value));


                List<string> roles = await GetRolesAsync();

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

