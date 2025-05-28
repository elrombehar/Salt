using Microsoft.AspNetCore.Mvc;
using Validator.Data.Services;
using Validator.Models;

namespace Validator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpModelsController(IEpModelsResolver epModelsResolver) : ControllerBase
    {
        private readonly IEpModelsResolver _epModelsResolver = epModelsResolver;


        [HttpPost("load")]
        public IActionResult LoadEpModels([FromBody] List<EpModelDefenition> models)
        {
            try
            {
                _epModelsResolver.ClearAllEpModels();
                _epModelsResolver.AddEpModels(models);
                return _epModelsResolver.GetEpModelsCount() == models.Count
                                                            ? Ok() 
                                                            : BadRequest("Failed importing all models");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
