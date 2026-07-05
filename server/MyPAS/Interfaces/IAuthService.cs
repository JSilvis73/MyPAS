using MyPAS.Models.Auth;

namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        Task<AuthIdentityResult> ChangePassword(ChangePasswordDTO changePasswordDTO);
        Task<AuthResult> Register(AuthRegisterRequest authRegisterRequest);
        Task<AuthResult> SignIn(SignInDTO signInDTO);
        Task<AuthIdentityResult> AddUserToRole(AssignRoleDTO assignRoleDTO);
        Task<AuthIdentityResult> RemoveUserFromRole(AssignRoleDTO assignRoleDTO);
    }
}
