using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Models;

namespace CovacRegistration.Server.Interfaces
{
    public interface IRegistrationService
	{
		IQueryable<Registration> GetAllRegistrations();		

		Registration GetRegistrationById(int registrationId);

		Registration InsertRegistration(Registration registration);

		Registration UpdateRegistration(Registration registration);

		void UpdateStatus(int registrationId, RegistrationStatus status);

		IQueryable<VaccinationProfile> GetAllVaccinationProfiles();
	}
}
