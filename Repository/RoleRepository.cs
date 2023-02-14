using Models;
using Repository.Interfaces;
using Models.Responses;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        public Task<Role> CreateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleId(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleName(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
