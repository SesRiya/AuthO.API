using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.ClaimsUserService
{
    public interface IUserService
    {
        Task<List<string>> GetRolesAsync(Guid userId);
    }
}
