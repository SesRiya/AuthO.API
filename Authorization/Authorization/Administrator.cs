using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Authorization.Authorization
{
    public class Administrator : IAuthorizationRequirement
    {
        public Administrator()
        {

        }
    }

    public class IsAllowedAccessToAll : AuthorizationHandler<Administrator>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     Administrator admin)
        {
            if (context.User.HasClaim(claim => claim.Value == "Admin"))
            {
                context.Succeed(admin);
            }
            return Task.CompletedTask;
        }
    }
}


