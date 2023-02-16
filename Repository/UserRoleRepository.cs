using AuthenticationServerEntityFramework;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        #region fields
        private readonly AuthenticationServerDbContext _dbContext;
        #endregion

        #region constructor
        public UserRoleRepository(AuthenticationServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region methods
        public async Task<UserRole> AddRoleToUser(UserRole userRole, User user)
        {
            _dbContext.Add(userRole);
            await _dbContext.SaveChangesAsync();

            return userRole;
        }

        public async Task<List<string>> GetAllRolesByUserID(Guid userID)
        {
            List<string> roles = new();

            var query = from role in _dbContext.UserRoles
                        where role.UserId == userID
                        select role;

            foreach (UserRole userRole in query)
            {
                roles.Add(userRole.RoleName);
            }
            return await Task.FromResult(roles.ToList());
        }
        #endregion
    }
}