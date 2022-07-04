using CovacRegistration.Server.Interfaces;
using CovacRegistration.Shared.Models;

namespace CovacRegistration.Server.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly CovacRegistrationDbContext db;

        public VaccineService(CovacRegistrationDbContext db)
        {
            this.db = db;
        }

        public List<Vaccine> GetAllVaccines()
        {        
            return this.db.Vaccines.ToList();
        }

        public Vaccine GetVaccineById(int vaccineId)
        {
            return db.Vaccines
                .SingleOrDefault(vaccine => vaccine.VaccineId == vaccineId);
        }

        public Vaccine InsertVaccine(Vaccine vaccine)
        {
            db.Vaccines.Add(vaccine);
            db.SaveChanges();

            return vaccine;
        }

        public Vaccine UpdateVaccine(Vaccine vaccine)
        {
            db.Vaccines.Update(vaccine);
            db.SaveChanges();

            return db.Vaccines
                .SingleOrDefault(r => r.VaccineId == vaccine.VaccineId);
        }
    }
}
