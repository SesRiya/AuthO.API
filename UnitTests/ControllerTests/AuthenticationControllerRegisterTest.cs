using ApiCore.Interfaces;
using AuthServer.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;
using WebModels;
using WebModels.Requests;

namespace UnitTests.ControllerTests
{
    [TestFixture]
    public class AuthenticationControllerRegisterTest
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
            User user = null;
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
            User x = null;
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

            _mockRegisterUser.Setup(x => x.CreateUser(registerRequestMock)).Returns(x = new User()
            {
                Username = registerRequestMock.Username,
                Email = registerRequestMock.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestMock.Password)
            });

            await authenticationController.Register(registerRequestMock);
            Assert.Multiple(() =>
            {
                Assert.That(registerRequestMock.Username, Is.EqualTo(x.Username));
                Assert.That(registerRequestMock.Email, Is.EqualTo(x.Email));
            });
        }
    }
}