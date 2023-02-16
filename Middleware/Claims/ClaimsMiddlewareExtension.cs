using Microsoft.AspNetCore.Builder;

namespace Middleware.Claims
{

    public static class ClaimsMiddlewareExtension
    {
        public static IApplicationBuilder UseClaimsAugmentation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsAugmentation>();
        }
    }
}
