using Repository.Interfaces;
using Services.Interfaces;
using ApiCore.Interfaces;
using Models.Requests;
using Models;
using System.Xml.Linq;

namespace ApiCore.Login
{
    public class LoginAuthentication : ILoginAuthentication
    {
        #region fields
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHash _passwordHasher;
        #endregion

        #region constructor
        public LoginAuthentication(
            IUserRepository userRepository,
            IPasswordHash passwordHasher
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region methods
        public async Task<User> IsUserAuthenticated(LoginRequest loginRequest)
        {
            User user = await _userRepository.GetByUsername(loginRequest.Username ?? throw new ArgumentNullException(nameof(loginRequest.Username)));
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
        #endregion
    }
}
