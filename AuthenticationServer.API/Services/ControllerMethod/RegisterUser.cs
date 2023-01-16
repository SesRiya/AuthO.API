using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using WebModels;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public class RegisterUser : IRegisterUser
    {
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;

        public RegisterUser(
            IPasswordHash passwordHasher,
            IUserRepository userRepository
            )
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public User CreateUser(RegisterRequest registerRequest)
        {
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {

                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
                Role = registerRequest.Role
            };
            return registrationUser;
        }
        public async Task<ErrorResponse?> UserVerification(RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return new ErrorResponse("Password does not match");
            }

            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return new ErrorResponse("Email already exists");
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return new ErrorResponse("User already exists");
            }

            return null;

        }

    }
}
