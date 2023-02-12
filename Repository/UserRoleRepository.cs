using Models;
using Repository.Interfaces;

namespace Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        List<UserRole> _usersWithRoles = new List<UserRole>
        {
            new UserRole
            {
               UserId = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
               RoleName = new List<string>{"Administrator", "Tester", "Developer"}
            },
            new UserRole
            {
                UserId = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                RoleName = new List<string>{ "Developer" , "Tester"}
            },
            new UserRole
            {
                UserId = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                 RoleName = new List<string>{"Tester" }
            }
        };

        public Task<UserRole> AddUserToRole(UserRole userRole)
        {
            _usersWithRoles.Add(userRole);
            return Task.FromResult(userRole);
        }

        public Task<UserRole> GetById(Guid userId)
        {
            return Task.FromResult(_usersWithRoles.FirstOrDefault(user => user.UserId == userId));
        }

        public Task<List<string>> GetAllRoles(Guid userID)
        {
            List<string> roles = new List<string>();

            UserRole userRole = (_usersWithRoles.FirstOrDefault(user => user.UserId == userID));
            return Task.FromResult(userRole.RoleName.ToList());
        }
    }
}
