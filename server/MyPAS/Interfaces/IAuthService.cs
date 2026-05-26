using MyPAS.Models.Auth;

namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthRegisterRequest authRegisterRequest);
        Task<AuthResult> SignIn(SignInDTO signInDTO);
    }
}
