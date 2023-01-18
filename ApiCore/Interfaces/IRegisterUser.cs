using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace ApiCore.Interfaces
{
    public interface IRegisterUser
    {
        public User CreateUser(RegisterRequest registerRequest);
        Task<ErrorResponse?> UserVerification(RegisterRequest registerRequest);
    }
}
