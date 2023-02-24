using AuthenticationServerEntityFramework;
using Microsoft.EntityFrameworkCore;
using Models;
using Repository.Interfaces;
using System.Data;

namespace Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        #region fields
        private readonly AuthenticationServerDbContext _dbContext;
        #endregion

        #region constructor
        public RefreshTokenRepository(AuthenticationServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region methods

        public async Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();
            _dbContext.Add(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> GetByToken(string token)
        {

            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            return refreshToken ?? throw new Exception($"Refreshtoken '{token}' not found.");
        }

        public async Task<Task> DeleteAllRefreshToken(Guid id)
        {
            var query = from token in _dbContext.RefreshTokens
                        where token.UserId == id
                        select token;
            foreach (var token in query)
            {
                _dbContext.RefreshTokens.Remove(token);
            }
            await _dbContext.SaveChangesAsync();
            return Task.CompletedTask;    
        }
        #endregion
    }
}





