using System.Security.Claims;

namespace AuthenticationServer.API.Services.TokenGenerator
{
    public interface ITokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null);
    }
}
