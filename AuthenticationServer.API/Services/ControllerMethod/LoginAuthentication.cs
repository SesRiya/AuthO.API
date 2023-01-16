using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Services.PasswordHasher;
using AuthenticationServer.API.Services.UserRepository;
using Repository.Interfaces;
using WebModels;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public class LoginAuthentication : ILoginAuthentication
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;

        public LoginAuthentication(
            IUserRepository userRepository,
            IPasswordHash passwordHasher
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> IsUserAuthenticated(LoginRequest loginRequest)
        {
            User user = await _userRepository.GetByUsername(loginRequest.Username);
            if (user == null)
            {
                return null;
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

            if (!isCorrectPassword)
            {
                return null;
            }

            return user;
        }

    }
}
