using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interfaces;
using Models.Responses;
using Models.Requests;
using Models;

namespace ApiCore.Interfaces
{
    public interface IUserRoleManager
    {
        Task AddRolesToUser(RegisterRequest registerRequest, User user);

        Task AddRoleToDbIfNotStored(RegisterRequest registerRequest);
    }
}
