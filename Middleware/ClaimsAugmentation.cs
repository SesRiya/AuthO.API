﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using WebModels;

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

        #region methods

        public async Task<List<string>> GetRolesAsync()
        {
            List<string> roles = new List<string>();

            //hardcoded token from login request
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZiM2UwMzBiLTY2NWItNDgxZS1iNDU5LTZiOGZmNjc5ODQ5YyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6IkFkbWluQG1haWwuY29tIiwibmJmIjoxNjc1NzMyNjQxLCJleHAiOjE2NzU3MzMyNDEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MjY4In0.bYKDphYzuoouycegL9lnwQ9PxI6lcZsRKCCi9wzuaqE";

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7268/api/AuthO");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                roles = await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return (roles);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ClaimsPrincipal principal = context.User;

            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                Claim idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

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

