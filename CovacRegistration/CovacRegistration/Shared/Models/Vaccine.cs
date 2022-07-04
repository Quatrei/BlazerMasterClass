using System.ComponentModel.DataAnnotations;

namespace CovacRegistration.Shared.Models
{
    public class Vaccine
    {
        [Key]
        public int VaccineId { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public int WeeksInterval { get; set; }

        public Vaccine()
        {
        }
    }
}
