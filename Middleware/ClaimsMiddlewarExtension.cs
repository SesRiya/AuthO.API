using Microsoft.AspNetCore.Builder;
using Middleware.Interface;

namespace Middleware
{

    public static class ClaimsMiddlewareExtension
        {
            public static IApplicationBuilder UseClaimsAddition(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ClaimsAugmentation>();
            }
        }

    
}
