using Microsoft.AspNetCore.Authentication;
using Repository.Interfaces;
using System.Security.Claims;

namespace Authorization.ClaimsTransformation
{
    public class ClaimsAddition : IClaimsTransformation
    {
        private readonly IUserRepository _userRepository;

        public ClaimsAddition(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            // User is not authenticated so just return right away
            if (principal.Identity?.IsAuthenticated is false)
            {
                return principal;
            }

           //get User identifier
            var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                return principal;
            }

            // Roles to attach to the user
            var roles = await _userRepository.GetAllRoles(Guid.Parse(idClaim.Value)); 

            var clonedPrincipal = principal.Clone();
            var clonedIdentity = (ClaimsIdentity)clonedPrincipal.Identity;

            foreach (var role in roles)
            {
                clonedIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            }

            return clonedPrincipal;
        }
    }
}
