using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Cookie
{
    public static class CookieMiddlewareExtension
    {
        public static IApplicationBuilder UseCookieAsBearerToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieAsBearerToken>();
        }
    }
}
