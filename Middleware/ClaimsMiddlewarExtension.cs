using Microsoft.AspNetCore.Builder;
using Middleware.Interface;

namespace Middleware
{

    public static class ClaimsMiddlewareExtension
        {
            public static IApplicationBuilder UseClaimsAugmentation(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ClaimsAugmentation>();
            }
        }

    
}
