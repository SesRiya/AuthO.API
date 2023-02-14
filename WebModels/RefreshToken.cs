using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public Guid UserId { get; set; }
    }
}
