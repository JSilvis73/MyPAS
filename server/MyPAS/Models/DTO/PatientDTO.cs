using System.ComponentModel.DataAnnotations;

namespace MyPAS.Models.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [Range(1,130)]
        public int? Age { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
