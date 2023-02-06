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


        [Test]
        public async Task LoginWithAuthenticatedUserAndPassword()
        {
            LoginRequest loginRequestMock = new LoginRequest()
            {
                Username = "mockit",
                Password = "password"
            };

            _mockUserRepository.Setup(u => u.GetByUsername(loginRequestMock.Username)).ReturnsAsync(new User() { Username = loginRequestMock.Username, PasswordHash = "hashed" });


            _mockPasswordHash.Setup(p => p.VerifyPassword(loginRequestMock.Password, It.IsAny<string>())).Returns(true);

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequestMock);

            string actualUsername = user.Username;
            Assert.AreEqual(loginRequestMock.Username, actualUsername);
        }

        [Test]
        public async Task LoginUnregisteredUser()
        {
            LoginRequest loginRequestMock = new LoginRequest()
            {
                Username = "mockit",
                Password = "password"
            };

            _mockUserRepository.Setup(u => u.GetByUsername(loginRequestMock.Username)).ReturnsAsync(new User() { Username = "notRegistered", PasswordHash = "hashed" });


            _mockPasswordHash.Setup(p => p.VerifyPassword(loginRequestMock.Password, It.IsAny<string>())).Returns(true);

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequestMock);

            string actualUsername = user.Username;
            Assert.AreNotEqual(loginRequestMock.Username, actualUsername);
        }

        [Test]
        public async Task LoginRegisteredUserAndInvalidPassword()
        {
            LoginRequest loginRequestMock = new LoginRequest()
            {
                Username = "mockit",
                Password = "password"
            };

            _mockUserRepository.Setup(u => u.GetByUsername(loginRequestMock.Username)).ReturnsAsync(new User() { Username = loginRequestMock.Username, PasswordHash = "hashed" });


            _mockPasswordHash.Setup(p => p.VerifyPassword(loginRequestMock.Password, It.IsAny<string>())).Returns(false);

            User user = await _loginAuthentication.IsUserAuthenticated(loginRequestMock);

            Assert.IsNull(user);
        }
    }
}
