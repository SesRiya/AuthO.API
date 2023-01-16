using AuthenticationServer.API.Models.Requests;
using WebModels;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public interface ILoginAuthentication
    {
        Task<User?> IsUserAuthenticated(LoginRequest loginRequest);

    }
}
