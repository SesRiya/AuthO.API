using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public record UserRole
    {

        public Guid UserId { get; set; }
        public List<string> RoleName { get; set; }
       
    }
}
