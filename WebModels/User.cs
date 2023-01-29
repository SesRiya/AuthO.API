using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public record User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        //public List<string>? Roles { get; set; }
        public List<Role>? Roles { get; set; }   
    }
}
