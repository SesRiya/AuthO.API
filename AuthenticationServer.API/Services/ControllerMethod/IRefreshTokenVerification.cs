using AuthenticationServer.API.Models;
using AuthenticationServer.API.Models.Requests;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public interface IRefreshTokenVerification
    {
        Task<bool> IsValidRefreshToken(RefreshRequest refreshRequest);
        Task<User> UserExists(RefreshRequest refreshRequest);

    }
}
