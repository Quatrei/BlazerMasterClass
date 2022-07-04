using CovacRegistration.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;

namespace CovacRegistration.Server
{
    public class CovacRegistrationDbContext : DbContext
    {
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<VaccinationProfile> VaccinationProfiles { get; set; }

        public CovacRegistrationDbContext(DbContextOptions<CovacRegistrationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>(registration => { registration.Property(p => p.RegistrationId).ValueGeneratedOnAdd(); });

            modelBuilder.Entity<Schedule>(schedule => { schedule.Property(p => p.ScheduleId).ValueGeneratedOnAdd(); });

            modelBuilder.Entity<Vaccine>(vaccine => { vaccine.Property(p => p.VaccineId).ValueGeneratedOnAdd(); });

            modelBuilder.Entity<VaccinationProfile>(vaccineProfile => { vaccineProfile.Property(p => p.VaccinationProfileId).ValueGeneratedOnAdd(); });

            base.OnModelCreating(modelBuilder);
        }
    }
}
