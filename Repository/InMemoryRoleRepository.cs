using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class InMemoryRoleRepository : IRoleRepository
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
            role.RoleId++;
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
