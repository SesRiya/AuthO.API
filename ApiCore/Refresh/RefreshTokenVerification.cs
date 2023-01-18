using ApiCore.Interfaces;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;
using WebModels.Responses;
using WebModels;
using Services.TokenValidators;

namespace ApiCore.Refresh
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

