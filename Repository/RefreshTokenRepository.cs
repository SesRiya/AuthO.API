using Models;
using Repository.Interfaces;

namespace Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        #region fields
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        #endregion

        #region contsructor
        public Task CreateRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();
            _refreshTokens.Add(refreshToken);
            return Task.CompletedTask;
        }
        #endregion

        #region methods
        public Task<RefreshToken> GetByToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(r => r.Token == token);
            return Task.FromResult(refreshToken);
        }

        public Task DeleteAllRefreshToken(Guid id)
        {
            _refreshTokens.RemoveAll(rt => rt.Id == id);

            return Task.CompletedTask;
        }
        #endregion
    }
}
