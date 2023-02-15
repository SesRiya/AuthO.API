using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Create(User user);

        Task<User> GetByEmail(string email);

        Task<User> GetByUsername(string username);

        Task<User> GetById(Guid id);

    }
}
