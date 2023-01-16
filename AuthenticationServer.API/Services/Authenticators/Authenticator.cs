﻿using AuthenticationServer.API.Models.Responses;
using AuthenticationServer.API.Services.RefreshTokenRepository;
using AuthenticationServer.API.Services.TokenGenerator;
using Microsoft.IdentityModel.Tokens;
using WebModels;

namespace AuthenticationServer.API.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITempRefreshTokenRepository _refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            ITempRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            Models.RefreshToken refreshTokenDTO = new Models.RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.CreateRefreshToken(refreshTokenDTO);

            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
