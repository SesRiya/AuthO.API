using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.Authenticators;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenGenerator;
using AuthenticationServer.API.Services.TokenValidators;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AuthenticationServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITempUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly ITempRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;

        public AuthenticationController(
            ITempUserRepository userRepository, 
            IPasswordHash passwordHasher, 
            AccessTokenGenerator accessToken, 
            RefreshTokenGenerator refreshTokenGenerator, 
            RefreshTokenValidator refreshTokenValidator, 
            ITempRefreshTokenRepository refreshTokenRepository, 
            Authenticator authenticator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessToken;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not match"));
            }

            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists"));
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists"));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {

                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
                Role = registerRequest.Role
            };

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

            User user = await _userRepository.GetByUsername(loginRequest.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            string accessToken = _accessTokenGenerator.GenerateToken(user);
            //string refreshToken = _refreshTokenGenerator.GenerateToken();

            //RefreshToken refreshTokenDTO = new RefreshToken()
            //{
            //    RefToken = refreshToken,
            //    UserId = user.Id,
            //};

            //await _refreshTokenRepository.CreateRefreshToken(refreshTokenDTO);

            //return Ok(new AuthenticatedUserResponse()
            //{
            //    AccessToken = accessToken,
            //    RefreshToken = refreshToken
            //});

            AuthenticatedUserResponse response= await _authenticator.Authenticate(user);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token."));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByRefreshToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token."));
            }



            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
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

