using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public interface IRegisterUser
    {
        public User CreateUser(RegisterRequest registerRequest);
        Task<bool> UserExists(RegisterRequest registerRequest);
        public bool IsPasswordMatching(RegisterRequest registerRequest);



    }
}
