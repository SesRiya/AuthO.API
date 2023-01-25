using Microsoft.AspNetCore.Authorization;

namespace Authorization.Authorization
{

    public class ReturnsOfficer : IAuthorizationRequirement
    {
        public ReturnsOfficer()
        {

        }
    }
    public class IsAllowedAccessToReturnsPage : AuthorizationHandler<ReturnsOfficer>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     ReturnsOfficer returns)
        {
            if (context.User.HasClaim(claim => claim.Value == "User"))
            {
                context.Succeed(returns);
            }
            return Task.CompletedTask;
        }
    }

}

