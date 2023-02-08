using System.ComponentModel.DataAnnotations;

namespace WebModels.Requests
{
    public record LoginRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
