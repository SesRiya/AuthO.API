
using ApiCore.Interfaces;
using ApiCore.Registration;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using System.Globalization;
using WebModels;
using WebModels.Requests;

namespace UnitTests.ApiCore
{
    public class RegisterUserUnitTest
    {
        private IRegisterUser _registerUser;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHash> _mockPasswordHash;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHash = new Mock<IPasswordHash>();
            _registerUser = new RegisterUser(_mockPasswordHash.Object, _mockUserRepository.Object);
        }

        [Test]
        public async Task UserVerificationTest()
        {
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit1@mymail.com",
                Username = "mockit1",
                Password = "password1",
                ConfirmPassword = "password1",
            };

            string email = "mockit2@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            string username = "mockit2";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            var userVerified = await _registerUser.UserVerification(registerRequestMock);

            Assert.That(userVerified, Is.Null);
        }

        [Test]
        public async Task UserHasRegisteredEmailAlready()
        {
            RegisterRequest registerRequestMock = new RegisterRequest()
            {
                Email = "mockit3@mymail.com",
                Username = "mockit3",
                Password = "password3",
                ConfirmPassword = "password3",
            };

            string email = "mockit3@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            string username = "mockit4";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            var userVerified = await _registerUser.UserVerification(registerRequestMock);

            Assert.That(userVerified, Is.Not.Null);
        }

        [Test]
        public async Task UserHasRegisteredUsernameAlready()
        {
            RegisterRequest registerRequestMock = new RegisterRequest()
            {
                Email = "mockit5@mymail.com",
                Username = "mockit5",
                Password = "password5",
                ConfirmPassword = "password5",
            };

            string email = "mockit6@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            string username = "mockit5";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            var userVerified = await _registerUser.UserVerification(registerRequestMock);

            Assert.That(userVerified, Is.Not.Null);

        }

        [Test]
        public async Task UserPasswordNotMatching()
        {
            RegisterRequest registerRequestMock = new RegisterRequest()
            {
                Email = "mockit7@mymail.com",
                Username = "mockit7",
                Password = "password7",
                ConfirmPassword = "password8",
            };

            var userVerified = await _registerUser.UserVerification(registerRequestMock);

            Assert.That(userVerified, Is.Not.Null);
        }
    }
}
