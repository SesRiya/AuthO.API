using ApiCore.Interfaces;
using ApiCore.Login;
using AuthServer.API.Controllers;
using Azure.Core;
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
        private Mock<CookieStorage> _mockCookieStorage;

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
            _mockCookieStorage = new Mock<CookieStorage>();

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

            var response = _mockAuthenticator.Setup(a => a.Authenticate(user)).ReturnsAsync(new AuthenticatedUserResponse()
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImUzODRmMDMwLTU0MzYtNGI4OS04NTFiLTlhNzA0ZWM2ZDI3MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6IkFkbWluMUBtYWlsLmNvbSIsIm5iZiI6MTY3NjM0NjE4NywiZXhwIjoxNjc2MzQ2MzA3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjY4IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCJ9.19pLbkt8frDPuHobsVGI5olPFEqSeGgUU2oRydeMUUY",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzYzNDYxODcsImV4cCI6MTY3NjM4MjE4NywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UnkNosTWYyYADVXjTR-FlMCkkk5H0fqdj3EhVRM0R-U"
            });

            var result = await authenticationController.Login(loginRequestMock);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(response, Is.Not.Null);
        }

    }
}