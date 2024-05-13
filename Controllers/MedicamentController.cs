using Microsoft.AspNetCore.Mvc;
using PrescriptionAPI.Services;

namespace PrescriptionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IMedicamentService _medicamentService;

        public MedicamentController(IMedicamentService medicamentService)
        {
            _medicamentService = medicamentService ?? throw new ArgumentNullException(nameof(medicamentService));
        }

        [HttpGet("{IdMedicament}")]
        public async Task<IActionResult> GetMedicament(int IdMedicament)
        {
            try
            {
                var medicament = await _medicamentService.GetMedicament(IdMedicament);
                if (medicament == null)
                {
                    return NotFound();
                }
                return Ok(medicament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred, please provide good data according to your database schema.");
            }
        }
    }
}