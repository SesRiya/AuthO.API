using ApiCore.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;
using WebModels.Requests;

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

            //get the roles and validate if roles are already on the Role if not add first to the role
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
