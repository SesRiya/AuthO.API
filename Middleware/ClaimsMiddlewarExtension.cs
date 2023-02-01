using Microsoft.AspNetCore.Builder;
using Middleware.ClaimsAugmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    
        public static class ClaimsMiddlewareExtension
        {
            public static IApplicationBuilder UseClaimsAddition(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<IClaimsAugmentation>();
            }
        }

    
}
