using Models;
using Models.Requests;
using Models.Responses;

namespace ApiCore.Interfaces
{
    public interface IRegisterUser
    {
        public User CreateUser(RegisterRequest registerRequest);
        Task<ErrorResponse?> UserVerification(RegisterRequest registerRequest);

    }
}
