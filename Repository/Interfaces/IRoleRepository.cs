using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> Create(Role role);

    }
}
