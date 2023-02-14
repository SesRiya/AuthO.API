using ApiCore.Interfaces;
using ApiCore.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Models.Responses;
using Repository.Interfaces;
using Services.Interfaces;
using System.Security.Claims;

namespace AuthServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthenticator _authenticator;
        private readonly IRegisterUser _registerUser;
        private readonly IRoleAdditionToUser _roleAdditionToUser;
        private readonly ILoginAuthentication _loginAuthentication;
        private readonly IRefreshTokenVerification _refreshTokenVerification;
        private readonly CookieStorage _cookieStorage;
        #endregion

        #region Constructor
        public AuthenticationController(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthenticator authenticator,
            IRegisterUser registerUser,
            IRoleAdditionToUser roleAdditionToUser,
            ILoginAuthentication loginAuthentication,
            IRefreshTokenVerification refreshTokenVerification,
            CookieStorage cookieStorage)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
            _registerUser = registerUser;
            _roleAdditionToUser = roleAdditionToUser;
            _loginAuthentication = loginAuthentication;
            _refreshTokenVerification = refreshTokenVerification;
            _cookieStorage = cookieStorage;
        }
        #endregion

        #region Actions
        [HttpPost("register")]
        [AllowAnonymous]
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

            //add guid to user
            await _userRepository.Create(registrationUser);

            //add roles to user
            await _roleAdditionToUser.AddRolesToUser(registerRequest, registrationUser);

            return Ok();
        }


        [HttpPost("login")]
        [AllowAnonymous]
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

            _cookieStorage.StoreJwtokensInCookies(user, response, Response);

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


        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            Response.Cookies.Delete("AccessToken");

            await _refreshTokenRepository.DeleteAllRefreshToken(userId);

            return NoContent();
        }
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
        #endregion
    }
}

