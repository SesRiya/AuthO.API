using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Security.Claims;
using WebModels;

namespace AuthenticationServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private User user;

        public AuthorizationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("getUserDetails")]
        public async Task<IActionResult> Index()
        {

            string id = HttpContext.User.FindFirstValue("id");
            Guid guid = new Guid(id);
            user = await _userRepository.GetById(guid);
            List<string> roles = await _userRepository.GetAllRoles(guid);
            if (user == null)
            {
                return BadRequest(guid);

            }
            else
            {
                return Ok(roles.Count);
            }
        }
    }
}

