using AuthenticationServer.API.Controllers;
using Moq;
using Repository.Interfaces;

namespace UnitTests.ControllerTests.AuthOControllerTest
{
    [TestFixture]
    public class AuthOControllerTest
    {
        private AuthOController authOController;
        private Mock<IUserRoleRepository> _mockUserRoleRepository;

        [SetUp]
        public void Setup()
        {
            _mockUserRoleRepository = new Mock<IUserRoleRepository>();
            authOController = new AuthOController(_mockUserRoleRepository.Object);
        }

        [Test]
        public void GetRolesForValidUsers()
        {   
            List<string>? roles = null;
            var idClaim = Guid.Parse("d22d9a72-7ce3-48c2-8ccb-8d8ff33dda7b");

             _mockUserRoleRepository.Setup(roles => roles.GetAllRolesByUserID(idClaim)).ReturnsAsync(roles = new List<string> { "Administrator", "Developer", "Tester" });

            Assert.That(roles, Is.Not.Null);
        }

        [Test]
        public void InvalidUsers()
        {
            List<string>? roles = null;
            var idClaim = Guid.Parse("d22d9a72-7ce3-48c2-8ccb-8d8ff33dda7b");

            _mockUserRoleRepository.Setup(roles => roles.GetAllRolesByUserID(idClaim)).ReturnsAsync(roles = null);

            Assert.That(roles, Is.Null);
        }
    }
}
