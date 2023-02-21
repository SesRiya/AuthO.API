using AuthenticationServerEntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using Repository;
using Repository.Interfaces;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;
using HttpContextMoq.Generic;

namespace UnitTests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private Mock<AuthenticationServerDbContext> dbContextMock;

        [SetUp]
        public void Setup()
        {
            dbContextMock = new Mock<AuthenticationServerDbContext>();
        }

        [Test]
        public async Task GetEmail()
        {
            var users = new List<User>();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(x => x.Add(It.IsAny<User>())).Callback<User>(users.Add);
           // dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);
            var repository = new UserRepository(dbContextMock.Object);

            var user = new User
            {
                Email = "Admin@mail.com",
                Username = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            };

            await repository.Create(user);

            mockSet.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
            Assert.That(users, Does.Contain(user));
        }
    }
}
