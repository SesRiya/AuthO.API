using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public record RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public List<Role> Roles { get; set; }

        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
