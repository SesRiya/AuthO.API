using AuthenticationServer.API.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace AuthenticationServer.API.Services.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfig _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfig configuration, ITokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {

            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenKey,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.RefreshTokenExpirationMinutes
                );
        }
    }

}

