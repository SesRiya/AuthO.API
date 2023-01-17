using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authorization.Authorization
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }

        public string Scope { get; }

        public ScopeRequirement(string issuer, string scope)
        {
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }

    public class RequireScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            //// The scope must have originated from our issuer.
            //var scopeClaim = context.User.FindFirst(
            //    claim => claim.Type == ClaimTypes.Role && 
            //    claim.Issuer == requirement.Issuer);
            //if (scopeClaim == null || String.IsNullOrEmpty(scopeClaim.Value))
            //    return Task.CompletedTask;

            //// A token can contain multiple scopes and we need at least one exact match.
            //if (scopeClaim.Value.Split(' ').Any(s => s == requirement.Scope))
            //    context.Succeed(requirement);
            //return Task.CompletedTask;
            if (context.User.HasClaim(f => f.Type == "string"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}