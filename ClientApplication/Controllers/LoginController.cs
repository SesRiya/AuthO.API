using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientApplication.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("login")]
        [Authorize(Policy = "Tester")]

        public IActionResult Index()
        {

            var accessToken = Request.Headers["Authorization"];
            return Ok(accessToken + "\n Tester's Page");
        }
    }
}
