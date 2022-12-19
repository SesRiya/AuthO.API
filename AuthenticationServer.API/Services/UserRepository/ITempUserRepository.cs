using AuthenticationServer.API.Models;

namespace AuthenticationServer.API.Services.UserRepository
{
    public interface ITempUserRepository
    {
        Task<User> GetByEmail(string email);

        Task<User> GetByUsername(string username);

        Task<User> GetByRole(string role);

        //return a user object
        Task<User> Create(User user);

        Task<User> GetById(Guid id);
      
    }
}
