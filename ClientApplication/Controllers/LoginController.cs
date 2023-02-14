using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientApplication.Controllers
{
    public class LoginController : Controller
    {
        [Authorize]
        [HttpGet("login")]
        public string Index()
        {

            var accessToken = Request.Headers["Authorization"];
            return accessToken;
        }
    }
}
