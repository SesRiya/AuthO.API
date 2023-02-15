using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public class CookieAsBearerToken
    {
        #region fields
        private readonly RequestDelegate _next;
        #endregion

        #region constructor
        public CookieAsBearerToken(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region method
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["AccessToken"];
            if (token != null)
                context.Request.Headers["Authorization"] = "Bearer " + token.ToString();

            await _next(context);
        }
        #endregion
    }
}
