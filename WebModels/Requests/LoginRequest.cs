﻿using System.ComponentModel.DataAnnotations;

namespace WebModels.Requests
{
    public record LoginRequest
    {
        [Required]
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
