using Microsoft.AspNetCore.Mvc;
using PrescriptionAPI.Models;
using PrescriptionAPI.Services;

namespace PrescriptionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpDelete("{IdPatient}")]
        public IActionResult Delete(int IdPatient)
        {
            var result = _patientService.RemovePatient(IdPatient);
            if (result is OkResult)
            {
                return Ok("Patient removed successfully.");
            }
            else if (result is ObjectResult)
            {
                return StatusCode((int)((ObjectResult)result).StatusCode, ((ObjectResult)result).Value);
            }
            else
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}