using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenValidators;
using Repository.Interfaces;
using WebModels;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public class RefreshTokenVerification : IRefreshTokenVerification
    {
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public RefreshTokenVerification
            (
            RefreshTokenValidator refreshTokenValidator,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository
            )
        {
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }


        public async Task<User> UserExists(RefreshRequest refreshRequest)
        {
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            return user;
        }

        public async Task<ErrorResponse?> VerifyRefreshToken(RefreshRequest refreshRequest)
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
    }
}
