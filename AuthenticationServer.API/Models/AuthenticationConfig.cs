namespace AuthenticationServer.API.Models
{
    public record AuthenticationConfig
    {
        public string AccessTokenKey { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenKey { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}

