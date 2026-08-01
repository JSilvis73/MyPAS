using MyPAS.Models.Auth;
using System.Security.Claims;

namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        Task<AuthIdentityResult> ChangePassword(ClaimsPrincipal User, ChangePasswordDTO changePasswordDTO);
        Task<AuthResult> Register(AuthRegisterRequest authRegisterRequest);
        Task<AuthResult> SignIn(SignInDTO signInDTO);
        Task<AuthIdentityResult> AddUserToRole(AssignRoleDTO assignRoleDTO);
        Task<AuthIdentityResult> RemoveUserFromRole(AssignRoleDTO assignRoleDTO);
        Task<AuthIdentityResult> UpdateUser(ClaimsPrincipal User, UpdateUserDTO updateUserDTO);
        Task<UserDTO?> GetCurrentUser(ClaimsPrincipal User);
        Task<bool> DeleteUserByEmailAdmin(string emailToDelete);
    }
}
