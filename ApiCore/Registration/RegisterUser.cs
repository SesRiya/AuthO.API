using ApiCore.Interfaces;
using Models;
using Models.Requests;
using Models.Responses;
using Repository.Interfaces;
using Services.Interfaces;

namespace ApiCore.Registration
{
    public class RegisterUser : IRegisterUser
    {
        #region Fields
        private readonly IPasswordHash _passwordHasher;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructor
        public RegisterUser(
            IPasswordHash passwordHasher,
            IUserRepository userRepository
            )
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }
        #endregion

        #region Actions
        public User CreateUser(RegisterRequest registerRequest)
        {
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password ?? throw new ArgumentNullException(nameof(registerRequest.Password)));

            User registrationUser = new()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };
            return registrationUser;
        }
        public async Task<ErrorResponse> UserVerification(RegisterRequest registerRequest)
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
        #endregion
    }
}
