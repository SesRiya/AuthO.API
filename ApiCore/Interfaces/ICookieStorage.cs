using Microsoft.AspNetCore.Http;
using Models.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCore.Interfaces
{
    public interface ICookieStorage
    {
        void StoreJwtokensInCookies(User user, AuthenticatedUserResponse response, HttpResponse httpResponse);

    }
}
