using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyPAS.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string? Address {  get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [Required]
        [Range(1,130)]
        public int Age { get; set; }
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [JsonIgnore]
        public List<Procedure> Procedures { get; set; } = new ();
        [JsonIgnore]
        public List<Payment> Payments { get; set; } = new ();

    }
}
