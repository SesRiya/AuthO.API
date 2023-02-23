using Microsoft.AspNetCore.Http;

namespace Middleware.Cookie
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

        /// <summary>
        /// Add Cookie (AccessToken) from AuthAPI as  a Bearer Token
        /// Authorization for all request if cookie is not null
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

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
