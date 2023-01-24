﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public record User
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        public IEnumerable<string>? Roles { get; set; }
    }
}
