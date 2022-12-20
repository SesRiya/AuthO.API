using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        //private readonly ITempUserRepository _userRepository;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly Logger<AuthorizationController> _logger;

       
        //public AuthorizationController(
        //    ITempUserRepository userRepository,
        //    RoleManager<IdentityRole> roleManager,
        //    Logger<AuthorizationController> logger
        //    )
        //{
        //    _userRepository = userRepository;
        //    _roleManager = roleManager;
        //    _logger = logger;
        //}

        [HttpGet]
        [Authorize(Policy = "admin")]
        public IActionResult GetAllRoles()
        {
          
            return Ok();
        }




    }
}
