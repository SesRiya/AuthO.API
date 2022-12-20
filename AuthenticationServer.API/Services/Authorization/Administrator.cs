using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthenticationServer.API.Services.Authorization
{
    public class Administrator : IAuthorizationRequirement
    {
       public Administrator()
        {

        }
    }

    public class IsAllowedToGetData:AuthorizationHandler<Administrator>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     Administrator admin)
        {
            if (context.User.HasClaim(claim => claim.Value =="string"))
            {
                context.Succeed(admin);
            }
            return Task.CompletedTask;
        }
    }
}
