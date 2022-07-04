using CovacRegistration.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CovacRegistration.Shared.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender? Gender { get; set; }
        public string ContactNumber { get; set; }
        public string IDPresented { get; set; }
        public string IDNumber { get; set; }
        public Priority? Priority { get; set; }

        [EnumDataType(typeof(RegistrationStatus))]
        public RegistrationStatus RegistrationStatus { get; set; }

        [ForeignKey("Address")]
        [JsonIgnore]
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        [InverseProperty("Registration")]
        [JsonIgnore]
        public virtual VaccinationProfile VaccinationProfile { get; set; }

        [NotMapped]
        public string FullName => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FirstName.ToLower() + " " + (MiddleName != null ? MiddleName[0] + ". " : string.Empty) + LastName.ToLower());

        [NotMapped]
        public string RegistrationNumber => $"{Priority}{RegistrationId:00000}";

        public Registration() {
            Address = new Address();
        }
    }
}
