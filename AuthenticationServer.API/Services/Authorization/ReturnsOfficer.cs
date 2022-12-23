using Microsoft.AspNetCore.Authorization;

namespace AuthenticationServer.API.Services.Authorization
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
            if (context.User.HasClaim(claim => claim.Value == "returns"))
            {
                context.Succeed(returns);
            }
            return Task.CompletedTask;
        }
    }

}

