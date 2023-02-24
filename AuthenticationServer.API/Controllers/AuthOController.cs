using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Security.Claims;

namespace AuthenticationServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthOController : ControllerBase
    {
        #region fields
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region constructor
        public AuthOController(
            IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        #endregion


        /// <summary>
        /// List all the roles stored in db during registration
        /// </summary>
        /// <returns></returns>
        #region methods
        [HttpGet]
        public  async Task<List<string>> GetRoles()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            List<string> roles = await _userRoleRepository.GetAllRolesByUserID(Guid.Parse(idClaim.Value));
            return (roles.ToList());
        }
        #endregion
    }
}


