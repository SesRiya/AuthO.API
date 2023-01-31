using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authorization.Authorization
{
    public class AdminOrTester: IAuthorizationRequirement
    {
        public AdminOrTester()
        {

        }

    }

    public class AdminOrTesterAccess : AuthorizationHandler<AdminOrTester>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     AdminOrTester payments)
        {
            if ((context.User.HasClaim(claim => claim.Value == "Tester" && claim.Type == ClaimTypes.Role) )
                || context.User.HasClaim(claim => claim.Value == "Administrator" && claim.Type == ClaimTypes.Role))
            {
                context.Succeed(payments);
            }
            return Task.CompletedTask;
        }
    }
}
