using AuthenticationServerEntityFramework;
using Microsoft.EntityFrameworkCore;
using Models;
using Repository.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationServerDbContext _dbContext;

        public UserRepository(AuthenticationServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user ?? throw new Exception($"User with email '{email}' not found.");
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            return user ?? throw new Exception(($"User with id '{id}' not found."));
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
            return user ?? throw new Exception(($"User with username '{username}' not found."));
        }
    }
}