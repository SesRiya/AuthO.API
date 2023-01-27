using Repository.Interfaces;
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

        public Task<List<string>> GetAllRoles(Guid userID)
        {
            User user = _users.FirstOrDefault(user => user.Id == userID);
            return Task.FromResult(user.Roles.ToList());
        }

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Id == userId));
        }
    }
}
