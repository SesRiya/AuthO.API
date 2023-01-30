using ApiCore.Interfaces;
using Repository.Interfaces;
using WebModels;
using WebModels.Requests;
using WebModels.Responses;

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

        //public async Task<ErrorResponse?> RoleVerification(RegisterRequest registerRequest, IRoleRepository _roleRepository)
        //{
        //    foreach (Role role in registerRequest.Roles)
        //    {
        //        if (role.RoleName.Equals(_roleRepository.GetRoleName(role.RoleName)))
        //        {
        //            return new ErrorResponse("Role exists already");
        //        }
        //    }
        //    return null;
        //}
        #endregion

    }



}

