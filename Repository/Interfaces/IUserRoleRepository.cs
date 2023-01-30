using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Repository.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddUserToRole(UserRole userRole);

        Task<UserRole> GetById(Guid userId);


        Task<List<string>> GetAllRoles(Guid userID);


    }
}
