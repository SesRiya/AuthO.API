using AuthenticationServer.API.Models;

namespace AuthenticationServer.API.Services.UserRepository
{
    public interface ITempUserRepository
    {
        //return a user object
        Task<User> Create(User user);

        Task<User> GetByEmail(string email);

        Task<User> GetByUsername(string username);

        Task<User> GetByRole(string role);

        Task<User> GetById(Guid id);
      
    }
}
