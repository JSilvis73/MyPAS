using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models
{
    public class MyPASUser : IdentityUser
    {
        // Optional: Real name fields
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        // Optional: Role or custom claims (can also use IdentityRole system)
        public string Role { get; set; }

        // Optional: Timestamp of registration
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: Status flag
        public bool IsActive { get; set; } = true;



        // Navigation properties could be added here later.
    }
}
