using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> GetByToken(string token);

        Task DeleteRefreshToken(Guid id);
    }
}
