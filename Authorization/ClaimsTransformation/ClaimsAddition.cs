using Authorization.ClaimsUserService;
using Microsoft.AspNetCore.Authentication;
using Repository.Interfaces;
using System.Security.Claims;

namespace Authorization.ClaimsTransformation
{
    public class ClaimsAddition : IClaimsTransformation
    {
        private readonly IUserService _usersService;
        private readonly IUserRepository _userRepository;

        public ClaimsAddition(IUserService usersService, IUserRepository userRepository)
        {
            _usersService = usersService;
            _userRepository = userRepository;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            // User is not authenticated so just return right away
            if (principal.Identity?.IsAuthenticated is false)
            {
                return principal;
            }

            // To be able to find the roles assigned to an user we need to use an unique identifier for this person.

            var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                return principal;
            }

            // Sample roles to attach to the user
            var roles = await _userRepository.GetAllRoles(Guid.Parse(idClaim.Value)); 

            // Clone the principal
            var clonedPrincipal = principal.Clone();
            var clonedIdentity = (ClaimsIdentity)clonedPrincipal.Identity;

            foreach (var role in roles)
            {
                // Here we add each role as a Role Claim type.
                clonedIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            }

            return clonedPrincipal;
        }
    }
}
