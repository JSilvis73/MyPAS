using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyPAS.Models
{
    public class Patient
    {
        //Fields
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation property to Services
        [JsonIgnore]
        public List<Procedure> Procedures { get; set; } = new List<Procedure>();
        [JsonIgnore]
        public List<Payment> Payments { get; set; } = new List<Payment>();

    }
}
