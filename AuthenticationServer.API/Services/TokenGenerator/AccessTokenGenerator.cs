using AuthenticationServer.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationServer.API.Services.TokenGenerator
{
    public class AccessTokenGenerator
    {

        private readonly AuthenticationConfig _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfig configuration, ITokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                //new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            
            };

            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenKey,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.AccessTokenExpirationMinutes,
                claims);   
        }
    }
}
