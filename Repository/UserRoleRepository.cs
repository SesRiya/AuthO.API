using AuthenticationServerEntityFramework;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AuthenticationServerDbContext _dbContext;

        public UserRoleRepository(AuthenticationServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRole> AddRoleToUser(UserRole userRole, User user)
        {
      
                _dbContext.Add(userRole);
                await _dbContext.SaveChangesAsync();

                return userRole;
           
        }

        public async Task<List<string>> GetAllRolesByUserID(Guid userID)
        {
            List<string> roles = new List<string>();

            var query = from role in _dbContext.UserRoles
                        where role.UserId == userID
                        select role;

            foreach (UserRole userRole in query)
            {
                roles.Add(userRole.RoleName);

            }
            return await Task.FromResult(roles.ToList());


        }

   
    }
}