using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Requests;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public interface ILoginAuthentication
    {
        Task<User?> IsUserAuthenticated(LoginRequest loginRequest);

    }
}
