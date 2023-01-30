using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;
using WebModels;
using Repository.Interfaces;
using WebModels.Responses;

namespace ApiCore.Interfaces
{
    public interface IRoleAdditionToUser
    {
        UserRole AddRolesToUser(RegisterRequest registerRequest, User user);

        //    Task<ErrorResponse?> RoleVerification(RegisterRequest registerRequest, IRoleRepository _roleRepository);
        //}
    }
}
