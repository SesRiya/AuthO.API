using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Security.Claims;
using WebModels;

namespace AuthenticationServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthOController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public AuthOController(
            IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public  async Task<List<string>> GetRoles()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            List<string> roles = await _userRoleRepository.GetAllRoles(Guid.Parse(idClaim.Value));
            return (roles.ToList());
        }
    }
}


