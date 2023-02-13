using System.ComponentModel.DataAnnotations;

namespace Models
{
    public record Role
    {
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
