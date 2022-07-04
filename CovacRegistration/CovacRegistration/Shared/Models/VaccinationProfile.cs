using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CovacRegistration.Shared.Models
{
    public class VaccinationProfile
    {
        public int VaccinationProfileId { get; set; }

        public VaccinationStatus VaccinationStatus { get; set; } = VaccinationStatus.WaitingForSchedule;

        public string BookAction => GetBookAction();

        [JsonIgnore]
        public int? RegistrationId { get; set; }

        [ForeignKey(nameof(RegistrationId))]
        public virtual Registration Registration { get; set; }

        [InverseProperty(nameof(Schedule.VaccinationProfile))]
        public virtual List<Schedule> Schedules { get; set; } = null;

        #region Private Methods

        private string GetBookAction()
        {
            switch (VaccinationStatus)
            {
                case VaccinationStatus.WaitingForSchedule:
                case VaccinationStatus.FirstDoseForRescheduling:
                    return "Book First Dose";
                case VaccinationStatus.FirstDoseAdministered:
                case VaccinationStatus.SecondDoseForRescheduling:
                    return "Book Second Dose";
                case VaccinationStatus.FullyVaccinated:
                    return "Book First Booster";
                case VaccinationStatus.FirstBoosterAdministered:
                case VaccinationStatus.SecondBoosterForRescheduling:
                    return "Book Second Booster";
                default:
                    return "";
            };
        }        
    }

    #endregion
}
