using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> GetByToken(string token);

        Task<Task> DeleteAllRefreshToken(Guid id);
    }
}
