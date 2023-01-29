﻿using Repository.Interfaces;
using WebModels;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        List<Role> _roles = new()
        {
            new Role
            {
                RoleId = 1,
                RoleName = "Admin"
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

        public Task<Role> CreateRole(Role role)
        {
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

        //public Task<List<string>> GetAllRoles(Role roleID)
        //{
        //    User user = _roles.FirstOrDefault(user => user.Id == userID);
        //    return Task.FromResult(user.Roles.ToList());
        //}
    }
}
