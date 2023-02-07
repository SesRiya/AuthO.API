using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Responses;
using WebModels;

namespace Services.Interfaces
{
    public interface IAuthenticator
    {
        Task<AuthenticatedUserResponse> Authenticate(User user);

    }
}
