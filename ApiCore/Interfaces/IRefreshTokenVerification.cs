using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Requests;
using Models.Responses;
using Models;

namespace ApiCore.Interfaces
{
    public interface IRefreshTokenVerification
    {
        Task<User> UserExists(RefreshRequest refreshRequest);
        Task<ErrorResponse> VerifyRefreshToken(RefreshRequest refreshRequest);
    }
}
