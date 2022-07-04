using System.ComponentModel;

namespace CovacRegistration.Shared.Enums
{
    public enum RegistrationStatus
	{
		[Description("All Status")]
		All = 0,

		[Description("Pending")]
		Pending = 1,

		[Description("Denied")]
		Denied = 2,

		[Description("Approved")]
		Approved = 3
	}
}
