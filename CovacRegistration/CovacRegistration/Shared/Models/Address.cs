using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CovacRegistration.Shared.Models
{
    public class Address
	{
		[Key]
        [JsonIgnore]
        public int AddressId { get; set; }
        public string City { get; set; }
		public string Barangay { get; set; }
		public string Street { get; set; }
	}
}
