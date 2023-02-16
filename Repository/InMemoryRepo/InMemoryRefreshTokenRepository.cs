using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InMemoryRepo
{
    public class InMemoryRefreshTokenRepository
    {

        #region fields
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        #endregion

        #region constructor
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
