using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceApplication.Controllers
{
    public class LoginController : Controller
    {
        [Authorize]
        [HttpGet("login")]
        public async Task<string> Index()
        {

            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            var accessToken = Request.Headers["Authorization"];
            return accessToken;
        }
    }
}
