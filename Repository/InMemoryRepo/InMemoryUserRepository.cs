using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InMemoryRepo
{
    public class InMemoryUserRepository
    {

        List<User> _users = new()
        {
            new User
            {
                Id = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
                Email = "Admin@mail.com",
                Username = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            },
            new User
            {
                 Id = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                Email = "Dev@mail.com",
                Username = "Dev",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),
            },
             new User
            {
                Id = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                Email = "Tester@mail.com",
                Username = "Tester",
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
