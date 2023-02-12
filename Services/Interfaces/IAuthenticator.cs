using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;
using Models;

namespace Services.Interfaces
{
    public interface IAuthenticator
    {
        Task<AuthenticatedUserResponse> Authenticate(User user);

    }
}
