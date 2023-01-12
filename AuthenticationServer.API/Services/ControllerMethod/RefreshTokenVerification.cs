using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenValidators;
using AuthenticationServer.API.Services.UserRepository;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public class RefreshTokenVerification : IRefreshTokenVerification
    {
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly ITempRefreshTokenRepository _refreshTokenRepository;
        private readonly ITempUserRepository _userRepository;

        public RefreshTokenVerification
            (
            RefreshTokenValidator refreshTokenValidator,
            ITempRefreshTokenRepository refreshTokenRepository,
            ITempUserRepository userRepository
            )
        {
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }

        private bool IsValidRefToken(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            return isValidRefreshToken;

        }

        public async Task<bool> IsValidRefreshToken(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (isValidRefreshToken)
            {
                return true;
            }
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO != null)
            {
                return true;
            }

            return false;

        }
        public async Task<User> UserExists(RefreshRequest refreshRequest)
        {
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            return user;
        }
    }
}
