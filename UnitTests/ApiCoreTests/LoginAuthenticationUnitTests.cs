using ApiCore.Interfaces;
using ApiCore.Login;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using WebModels;
using WebModels.Requests;

namespace UnitTests.ApiCoreTests
{
    public class LoginAuthenticationUnitTests
    {

        private ILoginAuthentication _loginAuthentication;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHash> _mockPasswordHash;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHash = new Mock<IPasswordHash>();
            _loginAuthentication = new LoginAuthentication(_mockUserRepository.Object, _mockPasswordHash.Object);
        }

        LoginRequest loginRequestMock = new LoginRequest()
        {
            Username = "mockit",
            Password = "password"
        };

        [Test]
        public async Task IsUserAuthenticatedTest()
        {
            string username = "mockit";
            var user =_mockUserRepository.Setup(u => u.GetByUsername(username)).ReturnsAsync(new User());
            Assert.IsNotNull(user);

            string passwordHash = "hashed";
            string password = "password";
            var passwordVerified = _mockPasswordHash.Setup(p => p.VerifyPassword(password, passwordHash)).Returns(true);
            
            Assert.IsNotNull(passwordVerified);
        }
    }
}
