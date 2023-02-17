using Models;
using Models.Requests;
using Models.Responses;

namespace ApiCore.Interfaces
{
    public interface IRegisterUser
    {
        User CreateUser(RegisterRequest registerRequest);
        Task<ErrorResponse> UserVerification(RegisterRequest registerRequest);

    }
}
