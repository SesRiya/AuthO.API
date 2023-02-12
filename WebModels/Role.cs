using System.ComponentModel.DataAnnotations;

namespace WebModels
{
    public record Role
    {
        [Key]
        public int RoleId { get; set; }
        public string ? RoleName { get; set; }
    }
}
