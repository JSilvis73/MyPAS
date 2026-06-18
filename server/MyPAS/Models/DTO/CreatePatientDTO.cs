using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class CreatePatientDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]  
        public string LastName { get; set; } = string.Empty ;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [Required]
        [Range(1,130)]
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

    }
}
