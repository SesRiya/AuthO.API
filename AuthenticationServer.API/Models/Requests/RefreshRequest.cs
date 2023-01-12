using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.API.Models.Requests
{
    public record RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
