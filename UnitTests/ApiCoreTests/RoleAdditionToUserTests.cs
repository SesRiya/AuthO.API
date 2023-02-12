﻿using ApiCore.Interfaces;
using ApiCore.Registration;
using Models;
using Models.Requests;
using Moq;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ApiCoreTests
{
    [TestFixture]   
    public class RoleAdditionToUserTests
    {
        private IRoleAdditionToUser _roleAdditionToUser;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRoleRepository> _mockRoleRepository;
        private Mock<IUserRoleRepository> _mockUserRoleRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockUserRoleRepository = new Mock<IUserRoleRepository>();
            _roleAdditionToUser = new RoleAdditionToUser(
                _mockUserRepository.Object, _mockRoleRepository.Object,
                _mockUserRoleRepository.Object);
        }

        [Test]
        public void AddRolesToUser()
        {
            RegisterRequest registerRequestMock = new()
            {
                Email = "mockit3@mymail.com",
                Username = "mockit3",
                Password = "password3",
                ConfirmPassword = "password3",
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


        }

    }
}
