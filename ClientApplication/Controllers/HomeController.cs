using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DataServer.API.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize(Policy ="admin")]

        public IActionResult Index()
        {
            string id = HttpContext.User.FindFirstValue("id");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return Ok(role);
        }
    }
}

