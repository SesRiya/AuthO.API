
using ApiCore.Interfaces;
using ApiCore.Registration;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using Services.PasswordHasher;
using UnitTests.MockRequests;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace UnitTests.ApiCore
{
    public class RegisterUserUnitTest
    {
        private RegisterUser _registerUser;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHash> _mockPasswordHash;
        private Mock<RegisterRequest> _mockRegisterRequest;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHash = new Mock<IPasswordHash>();
            _mockRegisterRequest = new Mock<RegisterRequest>();
            _registerUser = new RegisterUser(_mockPasswordHash.Object, _mockUserRepository.Object);
        }


        RegisterRequest registerRequestMock = new RegisterRequest()
        {
            Email = "mockit@mymail.com",
            Username = "mockit",
            Password = "password",
            ConfirmPassword = "password",
            Role = "admin"
        };


        [Test]
        public void RegisterWithPasswordsNotMatching()
        {
            var passwordMatching = _registerUser.IsPasswordMatching(registerRequestMock);
             
            Assert.IsTrue(passwordMatching);
        }


        [Test]
        public async Task RegisterwithExistingEmail()
        {
            string email = "mockit@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            bool emailExists = await _registerUser.IsEmailRegistered(registerRequestMock);

            Assert.True(emailExists);
        }

        [Test]
        public async Task RegisterwithExistingUsername()
        {
            string username = "mockit";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            bool usernameExists = await _registerUser.IsUserRegistered(registerRequestMock);

            Assert.True(usernameExists);
        }
    }
}
