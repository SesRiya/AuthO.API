using Microsoft.AspNetCore.Authorization;

namespace Authorization.Authorization
{
    public class PaymentsOfficer : IAuthorizationRequirement
    {
        public PaymentsOfficer()
        {

        }

    }

    public class IsAllowedAccessToPaymentsPage : AuthorizationHandler<PaymentsOfficer>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                     PaymentsOfficer payments)
        {
            if (context.User.HasClaim(claim => claim.Value == "payments"))
            {
                context.Succeed(payments);
            }
            return Task.CompletedTask;
        }
    }
}
