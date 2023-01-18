using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Email == email));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Username == username));
        }

        public Task<User?> GetByRole(string role)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Role == role));
        }

        public Task<User> GetById(Guid UserId)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Id == UserId));
        }
    }
}
