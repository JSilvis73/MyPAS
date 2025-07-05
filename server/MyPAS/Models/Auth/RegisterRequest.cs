using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}