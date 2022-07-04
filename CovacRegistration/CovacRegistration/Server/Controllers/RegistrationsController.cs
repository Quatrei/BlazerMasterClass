using CovacRegistration.Server.Interfaces;
using CovacRegistration.Shared.Enums;
using CovacRegistration.Shared.Extensions;
using CovacRegistration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CovacRegistration.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService, CovacRegistrationDbContext db)
        {
            _registrationService = registrationService;
        }

        #region Registration

        [HttpGet]
        public PagedResult<Registration> Get(int currentPage = 1, int? statusId = 0, int pageSize = 100, bool? isApproved = null)
        {
            var itemsToSkip = (currentPage - 1) * pageSize;

            var query = _registrationService.GetAllRegistrations();

            if (isApproved != null)
            {
                if (!isApproved.Value)
                    query = query.Where(r => r.RegistrationStatus == RegistrationStatus.Pending || r.RegistrationStatus == RegistrationStatus.Denied);
                else
                    query = query.Where(r => r.VaccinationProfile != null);
            }

            if (statusId > 0)
            {
                query = query.Where(r => (int)r.RegistrationStatus == statusId).AsQueryable();
            }

            return query.GetPageDetails(currentPage, pageSize);
        }

        [HttpGet("{registrationId}")]
        public ActionResult<Registration> GetById(int registrationId)
        {
            var registration = _registrationService.GetRegistrationById(registrationId);

            if (registration == null)
            {
                return NotFound();
            }

            return registration;
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            registration = _registrationService.InsertRegistration(registration);

            return Created($"Registrations/{registration.RegistrationId}", registration);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(registration.RegistrationStatus == RegistrationStatus.All)
                return BadRequest("Invalid Status");

            var existingRegistration = _registrationService.GetRegistrationById(registration.RegistrationId);

            if (existingRegistration == null)
                return NotFound();

            if (registration.RegistrationStatus != existingRegistration.RegistrationStatus &&
                existingRegistration.RegistrationStatus != RegistrationStatus.Pending)
            {
                return BadRequest("Status cannot be set for non-pending registration");
            }

            registration = _registrationService.UpdateRegistration(registration);

            return Ok(registration);
        }

        [HttpPatch("{registrationId}/RegistrationStatus")]
        public IActionResult UpdateStatus(int registrationId, [FromBody] RegistrationStatus status)
        {
            if (!Enum.IsDefined(status))
                return BadRequest("Invalid Status");

            if (status == RegistrationStatus.All)
                return BadRequest("Invalid Status");

            var registration = _registrationService.GetRegistrationById(registrationId);

            if (registration == null)
                return NotFound();

            if (registration.RegistrationStatus != RegistrationStatus.Pending)
                return BadRequest("Status cannot be set for non-pending registration");

            _registrationService.UpdateStatus(registrationId, status);

            return Ok();
        }

        #endregion

        #region VaccineProfile

        [HttpGet("{registrationId}/VaccinationProfile")]
        public ActionResult<VaccinationProfile> GetVaccineProfile(int registrationId)
        {
            var registration = _registrationService.GetRegistrationById(registrationId);

            if (registration?.VaccinationProfile == null)
                return NotFound();

            return registration.VaccinationProfile;
        }

        [HttpGet("VaccinationProfiles")]
        public ActionResult<IEnumerable<VaccinationProfile>> GetVaccineProfiles()
        {
            return _registrationService.GetAllVaccinationProfiles()
                    .ToList();
        }

        #endregion


    }
}
