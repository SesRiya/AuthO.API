using Repository.Interfaces;
using WebModels;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = new List<Role>
        {
            new Role
            {
                RoleId = 1,
                RoleName = "Administrator"
            },
            new Role
            {
                RoleId = 2,
                RoleName = "Tester"
            },
            new Role
            {
                RoleId = 3,
                RoleName = "Dev"
            }
        };

          public Task<Role> Create(Role role)
        {
            _roles.Add(role);
            return Task.FromResult(role);
        }

  
    }
}
