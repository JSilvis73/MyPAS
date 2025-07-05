using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        [Email]
    }
}