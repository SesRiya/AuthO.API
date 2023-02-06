using ApiCore.Interfaces;
using ApiCore.Login;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using System.Security.Principal;
using WebModels;
using WebModels.Requests;

namespace UnitTests.ApiCoreTests
{
    [TestFixture]
    public class LoginAuthenticationUnitTests
    {

        private ILoginAuthentication _loginAuthentication;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHash> _mockPasswordHash;

        [SetUp]
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
        public async Task LoginWithAuthenticatedUserAndPassword()
        {
            string expectedUsername = "mockit";
            _mockUserRepository.Setup(u => u.GetByUsername(expectedUsername)).ReturnsAsync(new User() { Username = expectedUsername});

            string passwordHash = "hashed";
            string password = "password";
            _mockPasswordHash.Setup(p => p.VerifyPassword(password, passwordHash)).Returns(true);

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequestMock);

            string actualUsername = user.Username;
            Assert.AreEqual(expectedUsername, actualUsername);



        }

        [Test]
        public async Task LoginRegisteredUserAndInvalidPassword()
        {

            LoginRequest loginRequestMock = new LoginRequest()
            {
                Username = "mockit",
                Password = "password"
            };

            _mockUserRepository.Setup(u => u.GetByUsername(loginRequestMock.Username)).ReturnsAsync(new User());

            string passwordHash = "hashed";
            string password = "wrongPassword";

            _mockPasswordHash.Setup(p => p.VerifyPassword(loginRequestMock.Password, passwordHash)).Returns(false);

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequestMock);

        }

        [Test]
        public async Task LoginUnregisteredUser()
        {
            LoginRequest loginRequestMock = new LoginRequest()
            {
                Username = "mockit",
                Password = "password"
            };
        }


    }

}
