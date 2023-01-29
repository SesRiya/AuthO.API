using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Repository
{
    public class UserRoleRepository
    {
        List<UserRole> _usersWithRoles = new List<UserRole>
        {
            new UserRole
            {
               UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleId = 1
            },
            new UserRole
            {
                UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleId = 2
            },
            new UserRole
            {
                UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleId = 3
            },
            new UserRole
            {
                UserId = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                RoleId = 2
            },
            new UserRole
            {
                UserId = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                RoleId= 3
            }
        };

        public Task<UserRole> AddUserToRole(UserRole userRole)
        {
            _usersWithRoles.Add(userRole);
            return Task.FromResult(userRole);
        }

    }
}
