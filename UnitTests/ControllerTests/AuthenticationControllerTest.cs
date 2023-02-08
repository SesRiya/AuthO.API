using ApiCore.Interfaces;
using AuthServer.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Moq;
using Repository.Interfaces;
using Services.Authenticators;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

namespace UnitTests.ControllerTests
{
    [TestFixture]
    public class AuthenticationControllerTest
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

            authenticationController.ModelState.AddModelError("Username", "Name is required");

            authenticationController.Register(registerRequestMock);

            //if model invalid, user will never be created
            _mockRegisterUser.Verify(x => x.CreateUser(It.IsAny<RegisterRequest>()), Times.Never);
        }

        [Test]
        public async Task InvalidModelStateUsingAssert()
        {
            //invalid request missing role parameter
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit1@mymail.com",
                Password = "password1",
                ConfirmPassword = "password1",
            };

            authenticationController.ModelState.AddModelError("Roles", "Role is required");

            var errorResponse = authenticationController.Register(registerRequestMock);

            //assert that there is an error response therefore model not valid
            Assert.That(errorResponse, Is.Not.Null);
        }

        [Test]
        public async Task ValidModelState()
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

            _mockRegisterUser.Setup(u => u.CreateUser(It.IsAny<RegisterRequest>())).Callback<RegisterRequest>(x => user = x);

           authenticationController.Register(registerRequestMock);

            //if model is valid user will be created once
            _mockRegisterUser.Verify(x => x.CreateUser(It.IsAny<RegisterRequest>()), Times.Once);
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

            //create user in the repository
            //_mockUserRepository.Setup(u => u.Create(It.IsAny<User>())).ReturnsAsync(new User()
            //  {
            //      Email = "mockit1@mymail.com",
            //      Username = "mockit",
            //      PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
            //  });



            _mockRegisterUser.Setup(u => u.CreateUser(It.IsAny<RegisterRequest>())).Callback<RegisterRequest>(x => user = x);


            authenticationController.Register(registerRequestMock);

            //verify if the user was never created
            _mockRegisterUser.Verify(x => x.CreateUser(It.IsAny<RegisterRequest>()), Times.Once);

            //Assert.Equals(registerRequestMock.Username, user.Username);

        }




    }
}

