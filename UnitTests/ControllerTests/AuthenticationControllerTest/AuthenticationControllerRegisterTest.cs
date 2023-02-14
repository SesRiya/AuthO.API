using ApiCore.Interfaces;
using ApiCore.Login;
using AuthServer.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Moq;
using Repository;
using Repository.Interfaces;
using Services.Interfaces;

namespace UnitTests.ControllerTests.AuthenticationControllerTest
{
    [TestFixture]
    public class AuthenticationControllerRegisterTest
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
        public async Task InvalidModelState()
        {
            //invalid request missing role parameter
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit1@mymail.com",
                Password = "password1",
                ConfirmPassword = "password1",
                Roles = new List<Role>
                {
                    new Role
                    {
                         RoleId = 1,
                         RoleName = "Admin"
                    },
                    new Role
                    {
                        RoleId = 2,
                        RoleName = "Tester"
                    }
                }
            };

            //adding model errors directly into the model state
            authenticationController.ModelState.AddModelError("Username", "Name is required");

            //if model invalid, user will never be created
            _mockRegisterUser.Verify(x => x.CreateUser(It.IsAny<RegisterRequest>()), Times.Never);

            var errorResponse = await authenticationController.Register(registerRequestMock);

            //assert that there is an error response therefore model not valid
            Assert.That(errorResponse, Is.Not.Null);
        }

        [Test]
        public async Task ValidModelState()
        {
            RegisterRequest registerRequestMock = new();
            await authenticationController.Register(registerRequestMock);

            //if model is valid user will be created once
            _mockRegisterUser.Verify(x => x.CreateUser(It.IsAny<RegisterRequest>()), Times.Once);
        }

        [Test]
        public async Task ValidModelStateWithResult()
        {
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit1@mymail.com",
                Username = "mockit",
                Password = "password",
                ConfirmPassword = "password",
                Roles = new List<Role>
                {
                    new Role
                    {
                         RoleName = "Admin"
                    },
                    new Role
                    {
                        RoleName = "Tester"
                    }
                }
            };

            var result = await authenticationController.Register(registerRequestMock);

            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task ValidModelStateWithAssertion()
        {
            User? user = null;
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit1@mymail.com",
                Username = "mockit",
                Password = "password",
                ConfirmPassword = "password",
                Roles = new List<Role>
                {
                    new Role
                    {
                         RoleName = "Admin"
                    },
                    new Role
                    {
                        RoleName = "Tester"
                    }
                }
            };

            _mockRegisterUser.Setup(x => x.CreateUser(registerRequestMock)).Returns(user = new User()
            {
                Username = registerRequestMock.Username,
                Email = registerRequestMock.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestMock.Password)
            });

            await authenticationController.Register(registerRequestMock);
            Assert.Multiple(() =>
            {
                Assert.That(registerRequestMock.Username, Is.EqualTo(user.Username));
                Assert.That(registerRequestMock.Email, Is.EqualTo(user.Email));
            });
        }
    }
}