using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Authorization.ClaimsTransformation
{
    public class AddCustomClaimsToIdentity : IClaimsTransformation
    {

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // User is not authenticated so just return right away
            if (principal.Identity?.IsAuthenticated is false)
            {
                return Task.FromResult(principal);
            }

            // To be able to find the roles assigned to an user we need to use an unique identifier for this person.

            var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                return Task.FromResult(principal);
            }

            // Sample roles to attach to the user
            var roles = new List<string> { "Admin", "User" };

            // Clone the principal
            var clonedPrincipal = principal.Clone();
            var clonedIdentity = (ClaimsIdentity)clonedPrincipal.Identity;

            foreach (var role in roles)
            {
                // Here we add each role as a Role Claim type.
                clonedIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            }

            return Task.FromResult(clonedPrincipal);
        }
    }
}
