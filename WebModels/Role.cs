using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public record Role
    {
        [Key]
        public int RoleId { get; set; }
        public string ? RoleName { get; set; }
    }
}
