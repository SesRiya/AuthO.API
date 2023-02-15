using ApiCore.Interfaces;
using Repository.Interfaces;
using Services.Interfaces;
using Models.Requests;
using Models.Responses;
using Models;

namespace ApiCore.Refresh
{
    public class RefreshTokenVerification : IRefreshTokenVerification
    {
        #region fields
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        #endregion

        #region constructor
        public RefreshTokenVerification
                (
                IRefreshTokenValidator refreshTokenValidator,
                IRefreshTokenRepository refreshTokenRepository,
                IUserRepository userRepository
                )
        {
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region methods
        public async Task<User> UserExists(RefreshRequest refreshRequest)
        {
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            return user;
        }

        public async Task<ErrorResponse> VerifyRefreshToken(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return new ErrorResponse("Invalid Token");
            }
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return new ErrorResponse("Invalid Token");
            }

            return null;
        }
        #endregion
    }
}

