using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration Configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfig configuration, IConfiguration config, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            Configuration = config;
            _tokenGenerator = tokenGenerator;
        }
        public string GenerateToken()
        {
            //adding keyVault for the refreshtoken secret
            SecretClient keyVaultClient = new(
                new Uri(Configuration["KeyVaultUri"]),
                new DefaultAzureCredential());
            _configuration.RefreshTokenKey = keyVaultClient.GetSecret("refresh-token-secret").Value.Value;

            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenKey,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.RefreshTokenExpirationMinutes
                );
        }
    }
}
