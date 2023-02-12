using ApiCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Services.Interfaces;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace AuthServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IAuthenticator _authenticator;
        private readonly IRegisterUser _registerUser;
        private readonly IRoleAdditionToUser _roleAdditionToUser;
        private readonly ILoginAuthentication _loginAuthentication;
        private readonly IRefreshTokenVerification _refreshTokenVerification;
        #endregion

        #region Constructor
        public AuthenticationController(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IAuthenticator authenticator,
            IRegisterUser registerUser,
            IRoleAdditionToUser roleAdditionToUser,
            ILoginAuthentication loginAuthentication,
            IRefreshTokenVerification refreshTokenVerification)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _authenticator = authenticator;
            _registerUser = registerUser;
            _roleAdditionToUser = roleAdditionToUser;
            _loginAuthentication = loginAuthentication;
            _refreshTokenVerification = refreshTokenVerification;
        }
        #endregion

        #region Actions
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            
            //Creating user
            ErrorResponse errorResponse = await _registerUser.UserVerification(registerRequest);
            if (errorResponse != null)
            {
                return BadRequest(errorResponse);
            }

            User registrationUser = _registerUser.CreateUser(registerRequest);
            await _userRepository.Create(registrationUser);


            UserRole addUserToRole = _roleAdditionToUser.AddRolesToUser(registerRequest, registrationUser);
            await _userRoleRepository.AddUserToRole(addUserToRole);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequest);
            if (user == null)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            ErrorResponse errorResponse = await _refreshTokenVerification.VerifyRefreshToken(refreshRequest);
            if (errorResponse != null)
            {
                return BadRequest(errorResponse);
            }

            User user = await _refreshTokenVerification.UserExists(refreshRequest);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);
            return Ok(response);
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
        #endregion

    }
}

