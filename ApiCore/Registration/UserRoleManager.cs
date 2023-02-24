using ApiCore.Interfaces;
using Models;
using Models.Requests;
using Repository.Interfaces;
using Models.Responses;

namespace ApiCore.Registration
{
    public class UserRoleManager : IUserRoleManager
    {
        #region fields
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region constructor
        public UserRoleManager(
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
            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

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
            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest));
            }

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

