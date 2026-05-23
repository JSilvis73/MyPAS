namespace MyPAS.Interfaces
{
    public interface IAuthService
    {
        void Login(string username, string password);
        void Register(string username, string password);
    }
}
