namespace AuthenticationServer.API.Services.PasswordHasher
{
    public interface IPasswordHash
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);
    }
}
