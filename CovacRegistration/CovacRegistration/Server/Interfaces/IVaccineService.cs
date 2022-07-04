using CovacRegistration.Shared.Models;

namespace CovacRegistration.Server.Interfaces
{
    public interface IVaccineService
    {
		List<Vaccine> GetAllVaccines();

		Vaccine GetVaccineById(int vaccineId);

		Vaccine InsertVaccine(Vaccine vaccine);

		Vaccine UpdateVaccine(Vaccine vaccine);
	}
}
