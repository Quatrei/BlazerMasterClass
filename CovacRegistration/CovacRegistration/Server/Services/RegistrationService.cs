using CovacRegistration.Server.Interfaces;
using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Extensions;
using CovacRegistration.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CovacRegistration.Server.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly CovacRegistrationDbContext db;

        public RegistrationService(CovacRegistrationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Registration> GetAllRegistrations()
        {
            var query = db.Registrations
                .Include(r => r.Address)
                .Include(r => r.VaccinationProfile)
                .AsQueryable();

            return query;
        }

        public Registration GetRegistrationById(int registrationId)
        {
            return db.Registrations
                .AsNoTracking()
                .Include(r => r.Address)
                .Include(r => r.VaccinationProfile)
                    .ThenInclude(v => v.Schedules)
                        .ThenInclude(s => s.Vaccine)
                .SingleOrDefault(registration => registration.RegistrationId == registrationId);
        }

        public Registration InsertRegistration(Registration registration)
        {
            // make sure no vaccination profile included in new registration
            registration.VaccinationProfile = null;

            // set initial status
            registration.RegistrationStatus = RegistrationStatus.Pending;

            db.Registrations.Add(registration);
            db.SaveChanges();

            return registration;
        }

        public Registration UpdateRegistration(Registration registration)
        {
            db.Registrations.Update(registration);

            db.SaveChanges();

            ProcessRegistrationApproval(registration.RegistrationId);

            return db.Registrations
                .SingleOrDefault(r => r.RegistrationId == registration.RegistrationId);
        }

        public void UpdateStatus(int registrationId, RegistrationStatus status)
        {
            var registration = GetRegistrationById(registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            registration.RegistrationStatus = status;
            
            db.Registrations.Update(registration);
            db.SaveChanges();

            ProcessRegistrationApproval(registrationId);
        }

        public IQueryable<VaccinationProfile> GetAllVaccinationProfiles()
        {
            return db.VaccinationProfiles
                .Include(v => v.Registration)
                .AsQueryable();
        }

        private void ProcessRegistrationApproval(int registrationId)
        {
            var registration = db.Registrations
                                .Include(r => r.VaccinationProfile)
                                .SingleOrDefault(r => r.RegistrationId == registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            if (registration.RegistrationStatus == RegistrationStatus.Approved && registration.VaccinationProfile == null)
            {
                registration.VaccinationProfile = new VaccinationProfile() { VaccinationStatus = VaccinationStatus.WaitingForSchedule };
                db.SaveChanges();
            }            
        }
    }
}
