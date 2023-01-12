using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public class RegisterUser : IRegisterUser
    {
        private readonly IPasswordHash _passwordHasher;
        private readonly ITempUserRepository _userRepository;

        public RegisterUser(
            IPasswordHash passwordHasher,
            ITempUserRepository userRepository
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

        public async Task<bool> UserExists(RegisterRequest registerRequest)
        {
            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return false;
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return false;
            }

            return true;
        }

        public bool IsPasswordMatching(RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return false;
            }

            return true;
        }

    }
}
