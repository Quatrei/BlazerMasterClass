using CovacRegistration.Server.Interfaces;
using CovacRegistration.Server.Services;
using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CovacRegistration.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public PagedResult<Schedule> Get(int currentPage, int? statusId = 0)
        {
            var pageSize = 10;

            return _scheduleService.GetAllSchedules(currentPage, pageSize, statusId);
        }

        [HttpGet("{scheduleId}")]
        public ActionResult<Schedule> GetById(int scheduleId)
        {
            var schedule = _scheduleService.GetScheduleById(scheduleId);

            if (schedule == null)
                return NotFound();

            return schedule;
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Schedule schedule)
        {
            try
            {
                schedule = _scheduleService.InsertSchedule(schedule);

                return Created($"Schedules/{schedule.ScheduleId}", schedule);
            }
            catch(Exception e) when (e.Message.Contains("not found"))
            {
                return new NotFoundObjectResult(e.Message);
            }
            catch (Exception e) when (e.Message.Contains("not allowed"))
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpPatch("{scheduleId}/ScheduleStatus")]
        public IActionResult UpdateStatus(int scheduleId, [FromBody] ScheduleStatus status)
        {
            if (!Enum.IsDefined(status))
                return BadRequest("Invalid Status");

            var existingSchedule = _scheduleService.GetScheduleById(scheduleId);

            if(existingSchedule == null)                
                return NotFound();

            if (existingSchedule.ScheduleStatus != ScheduleStatus.Pending)
                return BadRequest("Status cannot be set for non-pending schedule");

            _scheduleService.UpdateStatus(scheduleId, status);

            return Ok();
        }
    }
}
