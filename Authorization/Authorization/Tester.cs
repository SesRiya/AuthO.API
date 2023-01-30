using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authorization.Authorization
{

    public class Tester : IAuthorizationRequirement
    {
        public Tester()
        {

        }
    }
    public class TesterAccess : AuthorizationHandler<Tester>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     Tester returns)
        {
            if (context.User.HasClaim(claim => claim.Value == "Tester" && claim.Type == ClaimTypes.Role))
            {
                context.Succeed(returns);
            }
            return Task.CompletedTask;
        }
    }

}

