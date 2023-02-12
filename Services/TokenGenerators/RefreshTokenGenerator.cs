using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfig _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfig configuration, TokenGenerator tokenGenerator)
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
