using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.Authenticators;
using AuthenticationServer.API.Services.ControllerMethod;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenGenerator;
using AuthenticationServer.API.Services.TokenValidators;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace AuthenticationServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITempUserRepository _userRepository;
        private readonly Authenticator _authenticator;
        private readonly IRegisterUser _registerUser;
        private readonly ILoginAuthentication _loginAuthentication;
        private readonly IRefreshTokenVerification _refreshTokenVerification;

        public AuthenticationController(
            ITempUserRepository userRepository,
            Authenticator authenticator,
            IRegisterUser registerUser,
            ILoginAuthentication loginAuthentication,
            IRefreshTokenVerification refreshTokenVerification)
        {
            _userRepository = userRepository;
            _authenticator = authenticator;
            _registerUser = registerUser;
            _loginAuthentication = loginAuthentication;
            _refreshTokenVerification = refreshTokenVerification;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool passwordMatch = _registerUser.IsPasswordMatching(registerRequest);
            if (!passwordMatch)
            {
                return BadRequest(new ErrorResponse("Password does not match"));
            }

            bool userExists = await _registerUser.UserExists(registerRequest);
            if (!userExists)
            {
                return Conflict(new ErrorResponse("Username or email already exists"));
            }

            User registrationUser = _registerUser.CreateUser(registerRequest);
            await _userRepository.Create(registrationUser);

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

            bool isValidRefreshToken = await _refreshTokenVerification.IsValidRefreshToken(refreshRequest);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
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



    }
}

