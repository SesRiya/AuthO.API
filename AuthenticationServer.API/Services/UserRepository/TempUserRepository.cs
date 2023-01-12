using AuthenticationServer.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthenticationServer.API.Services.UserRepository
{
    public class TempUserRepository : ITempUserRepository
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

