using Models;
using Repository.Interfaces;
using Models.Responses;
using AuthenticationServerEntityFramework;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly AuthenticationServerDbContext _dbContext;

        public RoleRepository(AuthenticationServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role> CreateRole(Role role)
        {
            role.RoleId++;
            _dbContext.Add(role);
            await _dbContext.SaveChangesAsync();

            return role;
        }

        public async Task<Role> GetRoleId(int Id)
        {
            return _dbContext.Roles.FirstOrDefault(role => role.RoleId == Id);
        }

        public async Task<Role> GetRoleName(string roleName)
        {
            return _dbContext.Roles.FirstOrDefault(role => role.RoleName == roleName);
        }
    }
}
