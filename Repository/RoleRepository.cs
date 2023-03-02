using Models;
using Repository.Interfaces;
using AuthenticationServerEntityFramework;
using Microsoft.EntityFrameworkCore;

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
            _dbContext.Add(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<Role> GetRoleId(int Id)
        {
            var role = await _dbContext.Roles.FindAsync(Id);
            if (role == null)
            {
                throw new NotSupportedException($"Role with ID {Id} not found.");
            }
            return role;
        }

        public async Task<Role> GetRoleName(string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(role => role.RoleName == roleName);
            if(role == null)
            {
                return null;
            }
            return role;
        }
    }
}
