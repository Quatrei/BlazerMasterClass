using CovacRegistration.Server.Interfaces;
using CovacRegistration.Server.Services;
using CovacRegistration.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovacRegistration.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class VaccinesController : Controller
    {
        private readonly IVaccineService _vaccineService;

        public VaccinesController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        [HttpGet]
        public List<Vaccine> Get()
        {
            return _vaccineService.GetAllVaccines();
        }

		[HttpGet("{vaccineId}")]
		public ActionResult<Vaccine> GetById(int vaccineId)
		{
			var vaccine = _vaccineService.GetVaccineById(vaccineId);

			if (vaccine == null)
			{
				return NotFound();
			}

			return vaccine;
		}

		[HttpPost]
		public IActionResult Insert([FromBody] Vaccine vaccine)
		{
			vaccine = _vaccineService.InsertVaccine(vaccine);

			return Created($"Vaccines/{vaccine.VaccineId}", vaccine);
		}

		[HttpPut]
		public IActionResult Update([FromBody] Vaccine vaccine)
		{
			vaccine = _vaccineService.UpdateVaccine(vaccine);

			return Ok(vaccine);
		}
	}
}
