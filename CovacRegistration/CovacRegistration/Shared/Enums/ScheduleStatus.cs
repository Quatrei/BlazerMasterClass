using System.ComponentModel;

namespace CovacRegistration.Shared.Enums
{
	public enum ScheduleStatus
	{
		[Description("Pending")]
		Pending = 1,

		[Description("Show")]
		Show,

		[Description("No Show")]
		NoShow
	}
}
