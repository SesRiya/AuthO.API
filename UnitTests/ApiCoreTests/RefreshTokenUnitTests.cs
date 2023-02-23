using ApiCore.Interfaces;
using ApiCore.Refresh;
using Models;
using Models.Requests;
using Models.Responses;
using Moq;
using Repository.Interfaces;
using Services.Interfaces;

namespace UnitTests.ApiCoreTests
{
    [TestFixture]
    public class RefreshTokenUnitTests
    {
        private IRefreshTokenVerification _refreshTokenVerification;
        private Mock<IRefreshTokenValidator> _mockRefreshTokenValidator;
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockRefreshTokenValidator = new Mock<IRefreshTokenValidator>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _refreshTokenVerification = new RefreshTokenVerification(_mockRefreshTokenValidator.Object, _mockRefreshTokenRepository.Object, _mockUserRepository.Object);
        }

        [Test]
        public async Task UserExistsTest()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU3MTU1ODIsImV4cCI6MTY3NTc1MTU4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UznJLgkTK93Lv4_nvn2l4AQu7BJ_NTuHQQ3HX-VhB2k"
            };

            RefreshToken refreshDTO = new RefreshToken()
            {
                Id = Guid.Parse("3eb78052-88eb-4c75-8566-9b88c1fdb96b"),
                Token = refreshRequestMock.RefreshToken,
                UserId = Guid.Parse("6ec2e8e7-8122-4f8f-8869-0536af04a198")
            };

            _mockRefreshTokenRepository.Setup(t => t.GetByToken(refreshRequestMock.RefreshToken)).ReturnsAsync(refreshDTO);

            _mockUserRepository.Setup(u => u.GetById(refreshDTO.UserId)).ReturnsAsync(new User
            {
                Email = "test@mail.com",
                Id = Guid.Parse("6ec2e8e7-8122-4f8f-8869-0536af04a198")
            });

            User user = await _refreshTokenVerification.UserExists(refreshRequestMock);

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public async Task UserDoesNotExistsTest()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU3MTU1ODIsImV4cCI6MTY3NTc1MTU4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UznJLgkTK93Lv4_nvn2l4AQu7BJ_NTuHQQ3HX-VhB2k"
            };

            RefreshToken refreshDTO = new()
            {
                Id = Guid.Parse("3eb78052-88eb-4c75-8566-9b88c1fdb96b"),
                Token = refreshRequestMock.RefreshToken,
                UserId = Guid.Parse("6ec2e8e7-8122-4f8f-8869-0536af04a198")
            };

            _mockRefreshTokenRepository.Setup(t => t.GetByToken(refreshRequestMock.RefreshToken)).ReturnsAsync(refreshDTO);

            _mockUserRepository.Setup(u => u.GetById(refreshDTO.UserId)).ReturnsAsync(new User
            {
                Email = "test@mail.com",
                Id = Guid.Parse("6ec2e8e7-8144-4f8f-8869-0536af04a198")
            });

            User user = await _refreshTokenVerification.UserExists(refreshRequestMock);
            Guid userId = user.Id;

            Assert.That(userId, Is.Not.EqualTo(refreshDTO.UserId));
        }

        [Test]
        public async Task ValidRefreshTokenTest()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU3MTU1ODIsImV4cCI6MTY3NTc1MTU4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UznJLgkTK93Lv4_nvn2l4AQu7BJ_NTuHQQ3HX-VhB2k"
            };

            _mockRefreshTokenValidator.Setup(t => t.Validate(refreshRequestMock.RefreshToken)).Returns(true);
            _mockRefreshTokenRepository.Setup(t => t.GetByToken(refreshRequestMock.RefreshToken)).ReturnsAsync(new RefreshToken());

            ErrorResponse errorResponse = await _refreshTokenVerification.VerifyRefreshToken(refreshRequestMock);

            Assert.That(errorResponse, Is.Null);
        }

        [Test]
        public async Task InvalidRefreshTokenTest()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU3MTU1ODIsImV4cCI6MTY3NTc1MTU4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UznJLgkTK93Lv4_nvn2l4AQu7BJ_NTuHQQ3HX-VhB2k"
            };

            _mockRefreshTokenValidator.Setup(t => t.Validate(refreshRequestMock.RefreshToken)).Returns(false);

            _mockRefreshTokenRepository.Setup(t => t.GetByToken(refreshRequestMock.RefreshToken)).ReturnsAsync(new RefreshToken());

            ErrorResponse errorResponse = await _refreshTokenVerification.VerifyRefreshToken(refreshRequestMock);

            Assert.That(errorResponse, Is.Not.Null);
        }

        [Test]
        public async Task RefreshTokenNotOnRepository()
        {
            RefreshRequest refreshRequestMock = new()
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzU3MTU1ODIsImV4cCI6MTY3NTc1MTU4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.UznJLgkTK93Lv4_nvn2l4AQu7BJ_NTuHQQ3HX-VhB2k"
            };

            _mockRefreshTokenValidator.Setup(t => t.Validate(refreshRequestMock.RefreshToken)).Returns(true);

            _mockRefreshTokenRepository.Setup(t => t.GetByToken(refreshRequestMock.RefreshToken));

            ErrorResponse errorResponse = await _refreshTokenVerification.VerifyRefreshToken(refreshRequestMock);

            Assert.That(errorResponse, Is.Not.Null);
        }
    }
}
