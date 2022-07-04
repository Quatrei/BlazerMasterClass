using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovacRegistration.Shared.Enums
{
    public enum VaccinationStatus
    {
		[Description("All Status")]
		AllStatus = 0,

		[Description("Waiting for Schedule")]
		WaitingForSchedule = 1,

		[Description("1st Dose Booked")]
		FirstDoseBooked,

		[Description("1st Dose Administered")]
		FirstDoseAdministered,

		[Description("1st Dose For Rescheduling")]
		FirstDoseForRescheduling,

		[Description("2nd Dose Booked")]
		SecondDoseBooked,

		[Description("Fully Vaccinated")]
		FullyVaccinated,

		[Description("2nd Dose For Rescheduling")]
		SecondDoseForRescheduling,

		[Description("1st Booster Booked")]
		FirstBoosterBooked,

		[Description("1st Booster Administered")]
		FirstBoosterAdministered,

		[Description("2nd Booster Booked")]
		SecondBoosterBooked,

		[Description("2nd Booster Administered")]
		SecondBoosterAdministered,

		[Description("2nd Booster For Rescheduling")]
		SecondBoosterForRescheduling
	}
}
