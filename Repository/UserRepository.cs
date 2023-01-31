using Repository.Interfaces;
using WebModels;

namespace Repository
{
    public class UserRepository : IUserRepository
    {        

        List<User> _users = new()
        {
            new User
            {
                Id = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
                Email = "Sam@mail.com",
                Username = "Sam",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            },
            new User
            {
                 Id = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                Email = "Prateek@mail.com",
                Username = "Prateek",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),                
            },
             new User
            {
                Id = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                Email = "Cheyenne@mail.com",
                Username = "Cheyenne",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),
             }
        };

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

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_users.FirstOrDefault(user => user.Id == userId));
        }
    }
}
