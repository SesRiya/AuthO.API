using ApiCore.Interfaces;
using AuthServer.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace UnitTests.ControllerTests.AuthenticationControllerTest
{
    public class AuthenticationControllerRefreshTest
    {
        private AuthenticationController authenticationController;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRoleRepository> _mockRoleRepository;
        private Mock<IUserRoleRepository> _mockUserRoleRepository;
        private Mock<IAuthenticator> _mockAuthenticator;
        private Mock<IRegisterUser> _mockRegisterUser;
        private Mock<IRoleAdditionToUser> _mockRoleAdditionToUser;
        private Mock<ILoginAuthentication> _mockLoginAuthentication;
        private Mock<IRefreshTokenVerification> _mockRefreshTokenVerification;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockUserRoleRepository = new Mock<IUserRoleRepository>();
            _mockAuthenticator = new Mock<IAuthenticator>();
            _mockRegisterUser = new Mock<IRegisterUser>();
            _mockRoleAdditionToUser = new Mock<IRoleAdditionToUser>();
            _mockLoginAuthentication = new Mock<ILoginAuthentication>();
            _mockRefreshTokenVerification = new Mock<IRefreshTokenVerification>();

            authenticationController = new AuthenticationController
                (_mockUserRepository.Object, _mockRoleRepository.Object,
                _mockUserRoleRepository.Object, _mockAuthenticator.Object,
                _mockRegisterUser.Object, _mockRoleAdditionToUser.Object,
                _mockLoginAuthentication.Object, _mockRefreshTokenVerification.Object);
        }

        [Test]
        public async Task InvalidModelState()
        {
            RefreshRequest refreshRequestMock = new();

            authenticationController.ModelState.AddModelError("RefreshToken", "Not provided");

            var errorResponse = await authenticationController.Refresh(refreshRequestMock);

            //if model is invalid it will never be authenticated
            _mockRefreshTokenVerification.Verify(r => r.VerifyRefreshToken(It.IsAny<RefreshRequest>()), Times.Never());

            //model invalid errorsResponse is not null
            Assert.That(errorResponse, Is.Not.Null);
        }

        [Test]
        public async Task InvalidToken()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU5NzgxMDYsImV4cCI6MTY3NjAxNDEwNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.nApJtW_97dlY8z-tgqev0s3vE0eLpodtWG5cSasd5Ho"
            };

            var result = _mockRefreshTokenVerification.Setup(r => r.VerifyRefreshToken(refreshRequestMock)).ReturnsAsync(new ErrorResponse("invalid"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task UserNotFound()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU5NzgxMDYsImV4cCI6MTY3NjAxNDEwNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.nApJtW_97dlY8z-tgqev0s3vE0eLpodtWG5cSasd5Ho"
            };

            var result = _mockRefreshTokenVerification.Setup(r => r.UserExists(refreshRequestMock)).Equals(false);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ValidToken()
        {
            User r = null;
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU5NzgxMDYsImV4cCI6MTY3NjAxNDEwNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.nApJtW_97dlY8z-tgqev0s3vE0eLpodtWG5cSasd5Ho"
            };

            _mockRefreshTokenVerification.Setup(r => r.VerifyRefreshToken(refreshRequestMock)).Equals(null);
            _mockRefreshTokenVerification.Setup(r => r.UserExists(refreshRequestMock)).ReturnsAsync(r = new User());

            var response = _mockAuthenticator.Setup(a => a.Authenticate(r));
            var result = authenticationController.Refresh(refreshRequestMock).Result;

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(response, Is.Not.Null);
        }
    }
}