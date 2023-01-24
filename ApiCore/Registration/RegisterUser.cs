using ApiCore.Interfaces;
using Repository.Interfaces;
using Services.Interfaces;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

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
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
                Roles = registerRequest.Role
            };
            return registrationUser;
        }
        public async Task<ErrorResponse?> UserVerification(RegisterRequest registerRequest)
        {
            bool isPasswordMatching = IsPasswordMatching(registerRequest);
            if (!isPasswordMatching)
            {
                return new ErrorResponse("Password does not match");
            }

            bool isEmailRegistered = await IsEmailRegistered(registerRequest);
            if (isEmailRegistered)
            {
                return new ErrorResponse("Email already exists");
            }

            bool isUsernameRegistered = await IsUserRegistered(registerRequest);
            if (isUsernameRegistered)
            {
                return new ErrorResponse("User already exists");
            }
            return null;
        }

        public bool IsPasswordMatching(RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsEmailRegistered(RegisterRequest registerRequest)
        {
            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> IsUserRegistered(RegisterRequest registerRequest)
        {
            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return true;
            }
            return false;
        }


        #endregion
    }
}
