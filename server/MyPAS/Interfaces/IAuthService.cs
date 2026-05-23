namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        void Register(string username, string password);
        void LogIn(string username, string password);
        void LogOut(string username, string password);
    }
}
