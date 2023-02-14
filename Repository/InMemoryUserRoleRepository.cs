using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class InMemoryUserRoleRepository 
    {
        List<UserRole> _userWithRoles = new List<UserRole>
        {
            new UserRole
            {
               UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleName = "Administrator"
            },
            new UserRole
            {
               UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleName = "Developer"
            },
            new UserRole
            {
               UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleName = "Tester"
            },
            new UserRole
            {
                UserId = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                RoleName =  "Developer"
            },
            new UserRole
            {
                UserId = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                RoleName =  "Tester"
            },
            new UserRole
            {
                UserId = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                 RoleName = "Tester"
            }
        };

        public Task<UserRole> AddRoleToUser(UserRole userRole, User user)
        {
            if (userRole.UserId == user.Id)
            {
                userRole.Id++;
                _userWithRoles.Add(userRole);
                return Task.FromResult(userRole);
            }
            return null;
        }

        public Task<UserRole> GetRolesById(Guid userId)
        {
            return Task.FromResult(_userWithRoles.FirstOrDefault(user => user.UserId == userId));
        }

        public Task<List<string>> GetAllRolesByUserID(Guid userID)
        {
            List<string> roles = new List<string>();

            List<UserRole> userAndRoles = (_userWithRoles.FindAll(user => user.UserId == userID));
            foreach (UserRole userRole in userAndRoles)
            {
                roles.Add(userRole.RoleName);

            }
            return Task.FromResult(roles.ToList());
        }
    }
}
