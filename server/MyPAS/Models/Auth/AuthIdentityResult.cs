namespace MyPAS.Models.Auth
{
    public class AuthIdentityResult
    {
        public bool Result { get; set; }
        public List<string> Error { get; set; } = new ();
    }
}
