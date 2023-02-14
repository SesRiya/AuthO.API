using Models.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ApiCore.Interfaces;

namespace ApiCore.Login
{
    public class CookieStorage : ICookieStorage
    {
        public void StoreJwtokensInCookies(User user, AuthenticatedUserResponse response, HttpResponse httpResponse)
        {
            //save jwt in a cookie if user authenticated
            if (user != null)
            {
                var token = response.AccessToken;
                httpResponse.Cookies.Append("AccessToken", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            }
        }
    }
}
