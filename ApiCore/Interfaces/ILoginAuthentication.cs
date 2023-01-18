using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;
using WebModels;

namespace ApiCore.Interfaces
{
    public interface ILoginAuthentication
    {
        Task<User?> IsUserAuthenticated(LoginRequest loginRequest);
    }
}
