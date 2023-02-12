using Models;
using Repository.Interfaces;
using Models.Responses;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        List<Role> _roles = new()
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
                RoleName = "Developer"
            }
        };

        public Task<Role> CreateRole(Role role)
        {
            role.RoleId = _roles.Count + 2;
            _roles.Add(role);
            return Task.FromResult(role);
        }

        public Task<Role> GetRoleId(int Id)
        {
            return Task.FromResult(_roles.FirstOrDefault(role => role.RoleId == Id));
        }

        public Task<Role> GetRoleName(string roleName)
        {
            return Task.FromResult(_roles.FirstOrDefault(role => role.RoleName == roleName));
        }

    }
}
