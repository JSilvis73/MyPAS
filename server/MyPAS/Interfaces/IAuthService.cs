using MyPAS.Models.Auth;

namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Register(AuthRegisterRequest authRegisterRequest);
        void LogIn(string username, string password);
        void LogOut(string username, string password);
    }
}
