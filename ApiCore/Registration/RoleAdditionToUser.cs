﻿using ApiCore.Interfaces;
using Models;
using Models.Requests;
using Repository.Interfaces;
using Models.Responses;

namespace ApiCore.Registration
{
    public class RoleAdditionToUser : IRoleAdditionToUser

    {
        #region fields
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region constructor
        public RoleAdditionToUser(
          IUserRepository userRepository,
          IRoleRepository roleRepository,
          IUserRoleRepository userRoleRepository
          )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        #region methods


        public UserRole AddRolesToUser(RegisterRequest registerRequest, User user)
        {
            List<string> roleNames = new();

            foreach (Role role in registerRequest.Roles)
            {
                roleNames.Add(role.RoleName);
            }

            UserRole userRole = new UserRole()
            {
                UserId = user.Id,
                RoleName = new(roleNames)
            };


            return userRole;
        }
        #endregion

    }



}

