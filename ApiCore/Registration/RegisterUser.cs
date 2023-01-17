using ApiCore.Interfaces;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace ApiCore.Registration
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
