using ApiCore.Interfaces;
using AuthServer.API.Controllers;
using Moq;
using Repository.Interfaces;
using Services.Authenticators;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ControllerTests
{
    [TestFixture]
    public class AuthenticationControllerTest
    {
        private AuthenticationController authenticationController;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRoleRepository> _roleRepository;
        private Mock<IUserRoleRepository> _userRoleRepository;
        private Mock<IAuthenticator> _authenticator;
        private Mock<IRegisterUser> _registerUser;
        private Mock<IRoleAdditionToUser> _roleAdditionToUser;
        private Mock<ILoginAuthentication> _loginAuthentication;
        private Mock<IRefreshTokenVerification> _refreshTokenVerification;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _roleRepository = new Mock<IRoleRepository>();
            _userRoleRepository = new Mock<IUserRoleRepository>();
            _authenticator = new Mock<IAuthenticator>();
            _registerUser = new Mock<IRegisterUser>();
            _roleAdditionToUser = new Mock<IRoleAdditionToUser>();  
            _loginAuthentication = new Mock<ILoginAuthentication>();
            _refreshTokenVerification = new Mock<IRefreshTokenVerification>();
        }
    }
}
