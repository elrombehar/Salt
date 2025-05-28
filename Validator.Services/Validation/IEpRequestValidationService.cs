using Validator.Models;

namespace Validator.Services.Validation
{
    public interface IEpRequestValidationService
    {
        ValidationResult ValidateRequest(EpRequestData request);
    }
}
