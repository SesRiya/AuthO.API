using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authorization.Authorization
{
    public class Administrator : IAuthorizationRequirement
    {
        public Administrator()
        {

        }
    }

    public class AdminAccess : AuthorizationHandler<Administrator>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     Administrator admin)
        {

            if (context.User.HasClaim(claim => claim.Value == "Administrator" && claim.Type == ClaimTypes.Role))
            {
                context.Succeed(admin);
            }
            return Task.CompletedTask;
        }
    }
}


