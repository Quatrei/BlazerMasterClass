using CovacRegistration.Server.Interfaces;
using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Extensions;
using CovacRegistration.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CovacRegistration.Server.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly CovacRegistrationDbContext db;

        public ScheduleService(CovacRegistrationDbContext db)
        {
            this.db = db;
        }

        public PagedResult<Schedule> GetAllSchedules(int currentPage, int pageSize, int? statusId = 0)
        {
            var itemsToSkip = (currentPage - 1) * pageSize;

            var query = db.Schedules
                .Include(r => r.VaccinationProfile)
                    .ThenInclude(r => r.Registration)
                    .ThenInclude(r => r.Address)
                .Include(r => r.Vaccine)
                .AsQueryable();

            if (statusId > 0)
            {
                query = query.Where(r => (int)r.ScheduleStatus == statusId).AsQueryable();
            }

            return query.GetPageDetails(currentPage, pageSize);
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            return db.Schedules
                .AsNoTracking()
                .Include(r => r.Vaccine)
                .Include(r => r.VaccinationProfile)
                    .ThenInclude(v => v.Registration)
                        .ThenInclude(r => r.Address)
                .SingleOrDefault(schedule => schedule.ScheduleId == scheduleId);
        }

        public Schedule InsertSchedule(Schedule schedule)
        {
            var vaccinationProfile = db.VaccinationProfiles
                                        .Where(v => v.VaccinationProfileId == schedule.VaccinationProfileId)
                                        .SingleOrDefault();
            if (vaccinationProfile == null)
                throw new Exception("VaccinationProfile not found");

            switch (vaccinationProfile.VaccinationStatus)
            {
                case VaccinationStatus.FirstDoseBooked:
                case VaccinationStatus.SecondDoseBooked:
                case VaccinationStatus.FirstBoosterBooked:
                case VaccinationStatus.SecondBoosterBooked:
                    throw new Exception("Pending session found. New schedule not allowed");
                case VaccinationStatus.SecondBoosterAdministered:
                    throw new Exception("Third Booster schedule not allowed");
                default:
                    break;
            }

            var vaccine = db.Vaccines.SingleOrDefault(v => v.VaccineId == schedule.VaccineId);

            if (vaccine == null)
                throw new Exception("Vaccine not found");

            schedule.Vaccine = vaccine;

            db.Schedules.Add(schedule);
            db.SaveChanges();

            UpdateVaccinationProfileStatus(schedule.VaccinationProfileId);

            return schedule;
        }

        public void UpdateStatus(int scheduleId, ScheduleStatus status)
        {
            var schedule = db.Schedules.SingleOrDefault(schedule => schedule.ScheduleId == scheduleId);
            schedule.ScheduleStatus = status;
            db.Schedules.Update(schedule);
            db.SaveChanges();

            UpdateVaccinationProfileStatus(schedule.VaccinationProfileId);
        }

        public void UpdateVaccinationProfileStatus(int vaccinationProfileId)
        {
            var vaccinationProfile = db.VaccinationProfiles
                                        .Include(v => v.Schedules)
                                        .SingleOrDefault(s=>s.VaccinationProfileId == vaccinationProfileId);

            if (vaccinationProfile == null)
                throw new Exception("Vaccination profile not found");

            var status = VaccinationStatus.WaitingForSchedule;

            if (vaccinationProfile.Schedules == null || !vaccinationProfile.Schedules.Any())
                status = VaccinationStatus.WaitingForSchedule;
            else
            {
                var lastStatus = vaccinationProfile.Schedules.OrderByDescending(s => s.ScheduleId).First().ScheduleStatus;

                var showedCount = vaccinationProfile.Schedules.Where(s => s.ScheduleStatus == ScheduleStatus.Show).Count();

                switch (lastStatus)
                {
                    case ScheduleStatus.Pending:
                        status =  BookedStatusMap[showedCount];
                        break;
                    case ScheduleStatus.Show:
                        status = AdministeredStatusMap[showedCount];
                        break;
                    case ScheduleStatus.NoShow:
                        status = ReschedulingStatusMap[showedCount];
                        break;
                    default:
                        break;
                }
            }

            vaccinationProfile.VaccinationStatus = status;
            db.SaveChanges();
        }

        private Dictionary<int, VaccinationStatus> BookedStatusMap = new Dictionary<int, VaccinationStatus>()
        {
            { 0, VaccinationStatus.FirstDoseBooked },
            { 1, VaccinationStatus.SecondDoseBooked },
            { 2, VaccinationStatus.FirstBoosterBooked },
            { 3, VaccinationStatus.SecondBoosterBooked }
        };

        private Dictionary<int, VaccinationStatus> AdministeredStatusMap = new Dictionary<int, VaccinationStatus>()
        {
            { 1, VaccinationStatus.FirstDoseAdministered },
            { 2, VaccinationStatus.FullyVaccinated },
            { 3, VaccinationStatus.FirstBoosterAdministered },
            { 4, VaccinationStatus.SecondBoosterAdministered }
        };

        private Dictionary<int, VaccinationStatus> ReschedulingStatusMap = new Dictionary<int, VaccinationStatus>()
        {
            { 0, VaccinationStatus.FirstDoseForRescheduling },
            { 1, VaccinationStatus.SecondDoseForRescheduling },
            { 2, VaccinationStatus.FullyVaccinated },
            { 3, VaccinationStatus.SecondBoosterForRescheduling }
        };
    }
}
