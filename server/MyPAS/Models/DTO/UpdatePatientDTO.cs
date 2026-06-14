using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class UpdatePatientDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [Range(1,130)]
        public int? Age { get; set; } 
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
