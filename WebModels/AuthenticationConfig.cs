using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public class AuthenticationConfig
    {
        public string AccessTokenKey { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenKey { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}
