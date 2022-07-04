using CovacRegistration.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CovacRegistration.Shared.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [EnumDataType(typeof(ScheduleStatus))]
        public ScheduleStatus ScheduleStatus { get; set; } = ScheduleStatus.Pending;

        public string Venue { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public int VaccinationProfileId { get; set; }

        [ForeignKey(nameof(VaccinationProfileId))]
        public virtual VaccinationProfile VaccinationProfile { get; set; }

        public int? VaccineId { get; set; }

        [ForeignKey(nameof(VaccineId))]
        public virtual Vaccine Vaccine { get; set; }
    }
}
