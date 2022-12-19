using AuthenticationServer.API.Models;

namespace AuthenticationServer.API.Services.RefreshTokenRepository
{
    public interface ITempRefreshTokenRepository
    {
        Task CreateRefreshToken(RefreshToken refreshToken);

        Task <RefreshToken> GetByRefreshToken (string token); 

        Task DeleteRefreshToken (Guid id);
    }

}
