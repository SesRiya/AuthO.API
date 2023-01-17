using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels.Requests;
using WebModels.Responses;
using WebModels;

namespace ApiCore.Interfaces
{
    public interface IRefreshTokenVerification
    {
        Task<User> UserExists(RefreshRequest refreshRequest);
        Task<ErrorResponse?> VerifyRefreshToken(RefreshRequest refreshRequest);
    }
}
