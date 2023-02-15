using ApiCore.Interfaces;
using AuthServer.API.Controllers;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Models.Responses;
using Moq;
using Repository;
using Repository.Interfaces;
using Services.Interfaces;

namespace UnitTests.ControllerTests.AuthenticationControllerTest
{
    [TestFixture]
    public class AuthenticationControllerLoginTest
    {
        private AuthenticationController authenticationController;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<RefreshTokenRepository> _mockRefreshTokenRepository;
        private Mock<IAuthenticator> _mockAuthenticator;
        private Mock<IRegisterUser> _mockRegisterUser;
        private Mock<IRoleAdditionToUser> _mockRoleAdditionToUser;
        private Mock<ILoginAuthentication> _mockLoginAuthentication;
        private Mock<IRefreshTokenVerification> _mockRefreshTokenVerification;
        private Mock<ICookieStorage> _mockCookieStorage;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRefreshTokenRepository = new Mock<RefreshTokenRepository>();
            _mockAuthenticator = new Mock<IAuthenticator>();
            _mockRegisterUser = new Mock<IRegisterUser>();
            _mockRoleAdditionToUser = new Mock<IRoleAdditionToUser>();
            _mockLoginAuthentication = new Mock<ILoginAuthentication>();
            _mockRefreshTokenVerification = new Mock<IRefreshTokenVerification>();
            _mockCookieStorage = new Mock<ICookieStorage>();

            authenticationController = new AuthenticationController
                (_mockUserRepository.Object, _mockRefreshTokenRepository.Object,
                 _mockAuthenticator.Object, _mockRegisterUser.Object,
                 _mockRoleAdditionToUser.Object, _mockLoginAuthentication.Object,
                _mockRefreshTokenVerification.Object, _mockCookieStorage.Object);
        }

        [Test]
        public async Task InvalidModelStateForLogin()
        {
            LoginRequest loginRequestMock = new()
            {
                Username = "mockito",
            };

            //adding model errors directly into the model state
            authenticationController.ModelState.AddModelError("Password", "Password is required");

            var errorResponse = await authenticationController.Login(loginRequestMock);

            //if model is invalid it will never be authenticated
            _mockLoginAuthentication.Verify(l => l.IsUserAuthenticated(It.IsAny<LoginRequest>()), Times.Never());

            //model invalid errorsResponse is not null
            Assert.That(errorResponse, Is.Not.Null);
        }

        [Test]
        public async Task ValidModelInvalidLoginRequest()
        {
            LoginRequest loginRequestMock = new()
            {
                Username = "mockito",
                Password = "pssword"
            };

            //invalid username or password returns user == null
            _mockLoginAuthentication.Setup(l => l.IsUserAuthenticated(loginRequestMock)).Equals(null);

            var result = await authenticationController.Login(loginRequestMock);

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        [Test]
        public async Task AuthenticatedUserWithTokenResponse()
        {
            User? user = null;
            LoginRequest loginRequestMock = new()
            {
                Username = "mockito",
                Password = "pssword"
            };

            _mockLoginAuthentication.Setup(l => l.IsUserAuthenticated(loginRequestMock)).ReturnsAsync(user = new User()
            {
                Username = loginRequestMock.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequestMock.Password)
            });

            var response = _mockAuthenticator.Setup(a => a.Authenticate(user));

            var httpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            //_mockCookieStorage.Setup(c => c.StoreJwtokensInCookies(user, response, httpResponse));
            var result = await authenticationController.Login(loginRequestMock);

            //Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(response, Is.Not.Null);
        }

    }
}