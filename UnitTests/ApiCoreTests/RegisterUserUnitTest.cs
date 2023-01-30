
using ApiCore.Interfaces;
using ApiCore.Registration;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
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


        RegisterRequest registerRequestMock = new RegisterRequest()
        {
            Email = "mockit@mymail.com",
            Username = "mockit",
            Password = "password",
            ConfirmPassword = "password",
            //Roles = new List<>();
        };


        [Test]
        public void RegisterWithPasswordsNotMatchingTest()
        {
            var passwordMatching = _registerUser.IsPasswordMatching(registerRequestMock);
             
            Assert.IsTrue(passwordMatching);
        }


        [Test]
        public async Task RegisterwithExistingEmailTest()
        {
            string email = "mockit@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            bool emailExists = await _registerUser.IsEmailRegistered(registerRequestMock);

            Assert.True(emailExists);
        }

        [Test]
        public async Task RegisterwithExistingUsernameTest()
        {
            string username = "mockit";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            bool usernameExists = await _registerUser.IsUserRegistered(registerRequestMock);

            Assert.True(usernameExists);
        }

        [Test]
        public async Task UserVerificationTest()
        {
            string email = "mockit3@mymail.com";

            _mockUserRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(new User());

            string username = "mockit3";

            _mockUserRepository.Setup(user => user.GetByUsername(username)).ReturnsAsync(new User());

            var userVerified = _registerUser.UserVerification(registerRequestMock);

            Assert.Null(userVerified.Result);
        }
    }
}
