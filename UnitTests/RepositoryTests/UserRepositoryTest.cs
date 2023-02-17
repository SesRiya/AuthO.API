using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using System.Collections.Generic;
using System;

namespace UnitTests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
            var users = new List<User>() {
               new User
            {
                Id = Guid.Parse("6b3e030b-665b-481e-b459-6b8ff679849c"),
                Email = "Admin@mail.com",
                Username = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            },
            new User
            {
                 Id = Guid.Parse("5cfe8c2d-5859-4ada-892c-e21c79d80805"),
                Email = "Dev@mail.com",
                Username = "Dev",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),
            },
             new User
            {
                Id = Guid.Parse("32d114de-5752-4dbe-8793-8b01a067cde2"),
                Email = "Tester@mail.com",
                Username = "Tester",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),
             }
            };
            var queryable = users.AsQueryable();

            Mocks = new Mock<DbSet<User>>();

            Mocks.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            Mocks.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            Mocks.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

            Mocks.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<Person>(queryable.Provider));
            Mocks.As<IDbAsyncEnumerable<Person>>().Setup(m => m.GetAsyncEnumerator()).Returns(new AsyncEnumerator<Person>(queryable.GetEnumerator()));
        }




    }
}
