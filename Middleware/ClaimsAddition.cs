using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public class ClaimsAddition
    {
        private readonly RequestDelegate _next;
        private readonly IUserRoleRepository _userRoleRepository;

        public ClaimsAddition(
            RequestDelegate next,
            IUserRoleRepository userRoleRepository
            )
        {
            _next = next;
            _userRoleRepository = userRoleRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            ClaimsPrincipal principal = context.User as ClaimsPrincipal;

            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                Claim idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

                List<string> roles = await _userRoleRepository.GetAllRoles(Guid.Parse(idClaim.Value));

                ClaimsPrincipal clonedPrincipal = principal.Clone();
                ClaimsIdentity clonedIdentity = (ClaimsIdentity)clonedPrincipal.Identity;

                foreach (string role in roles)
                {
                    clonedIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
                }
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);

        }
    }

    public static class MyAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseClaimsAddition(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsAddition>();
        }
    }

}

