namespace Validator.Services.Validation
{
    public interface ITypeValidationService
    {
        public bool ValidateType(object? value, string type);

    }
}
