﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.API.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string ? Email { get; set; }
        [Required]
        public string ? Username { get; set; }
        [Required]
        public string ? Password { get; set; }
        [Required]
        public string ? ConfirmPassword { get; set; }
        public string ? Role { get; set; }

        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
