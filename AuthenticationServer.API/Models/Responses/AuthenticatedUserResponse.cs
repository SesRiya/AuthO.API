namespace AuthenticationServer.API.Models.Responses
{
    public record AuthenticatedUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
