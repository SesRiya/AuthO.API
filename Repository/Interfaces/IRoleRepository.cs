using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateRole(Role role);

        Task<Role> GetRoleName(string roleName);

        Task<Role> GetRoleId(int Id);

    }
}
