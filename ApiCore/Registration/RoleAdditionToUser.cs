using ApiCore.Interfaces;
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

        public async Task AddRolesToUser(RegisterRequest registerRequest, User user)
        {

            foreach (Role role in registerRequest.Roles)
            {

                UserRole userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleName = role.RoleName,
                };
                await _userRoleRepository.AddRoleToUser(userRole, user);
            }
        }
        public async Task AddRoleToDbIfNotStored(RegisterRequest registerRequest)
        {
            foreach (Role roleUser in registerRequest.Roles)
            {
                Role role = await _roleRepository.GetRoleName(roleUser.RoleName);
                if(role == null)
                {
                    await _roleRepository.CreateRole(roleUser);
                }
            }
        }

        #endregion
    }
}

