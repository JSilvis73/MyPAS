using MyPAS.Models.Auth;

namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthRegisterRequest authRegisterRequest);
        void SignIn(string username, string password);
        void SignOut(string username, string password);
    }
}
