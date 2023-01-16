using AuthenticationServer.API.Models.Requests;
using AuthenticationServer.API.Models.Responses;
using WebModels;

namespace AuthenticationServer.API.Services.ControllerMethod
{
    public interface IRefreshTokenVerification
    {
        Task<User> UserExists(RefreshRequest refreshRequest);
        Task<ErrorResponse?> VerifyRefreshToken(RefreshRequest refreshRequest);

    }
}
