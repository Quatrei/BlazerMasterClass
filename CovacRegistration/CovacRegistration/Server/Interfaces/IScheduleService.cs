using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Models;

namespace CovacRegistration.Server.Interfaces
{
    public interface IScheduleService
    {
		PagedResult<Schedule> GetAllSchedules(int currentPage, int pageSize, int? statusId = 0);

		Schedule GetScheduleById(int scheduleId);

		Schedule InsertSchedule(Schedule schedule);

		void UpdateStatus(int scheduleId, ScheduleStatus status);
	}
}
