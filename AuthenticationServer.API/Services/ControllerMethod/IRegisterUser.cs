using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Responses;

namespace AuthenticationServer.API.Services.ControllerMethod
{

    public interface IRegisterUser
    {
        public User CreateUser(RegisterRequest registerRequest);
        Task<ErrorResponse?> UserVerification(RegisterRequest registerRequest);


    }

}