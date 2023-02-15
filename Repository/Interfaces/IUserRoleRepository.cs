using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddRoleToUser(UserRole userRole, User user);

        Task<List<string>> GetAllRolesByUserID(Guid userID);


    }
}
