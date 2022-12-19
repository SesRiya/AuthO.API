using AuthenticationServer.API.Models;

namespace AuthenticationServer.API.Services.RefreshTokenRepository
{
    public class TempRefreshTokenRepository : ITempRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public Task CreateRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();
            _refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByRefreshToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(r => r.RefToken == token);

            return Task.FromResult(refreshToken);
        }

        public Task DeleteRefreshToken(Guid id)
        {
            _refreshTokens.RemoveAll(rt => rt.Id == id);

            return Task.CompletedTask;
        }



    }
}
