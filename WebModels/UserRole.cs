using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record UserRole
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string RoleName { get; set; }

    }
}
