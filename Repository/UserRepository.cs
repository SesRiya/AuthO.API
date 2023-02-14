using AuthenticationServerEntityFramework;
using Microsoft.EntityFrameworkCore;
using Models;
using Repository.Interfaces;

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
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

    }
}