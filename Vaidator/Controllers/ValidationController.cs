using Microsoft.AspNetCore.Mvc;
using Validator.Models;
using Validator.Services.Validation;

namespace Validator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController(IEpRequestValidationService epRequestValidationService) : ControllerBase
    {
        private readonly IEpRequestValidationService _epRequestValidationService = epRequestValidationService;

        [HttpPost("validate-data")]
        public IActionResult ValidateRequestData([FromBody] EpRequestData requestData)
        {
            try
            {
                if (requestData == null)
                {
                    return BadRequest("Request data cannot be null.");
                }
                var validationResult = _epRequestValidationService.ValidateRequest(requestData);
                
                return Ok(validationResult);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
