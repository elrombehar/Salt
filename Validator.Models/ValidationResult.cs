namespace Validator.Models
{
    public class ValidationResult
    {
        public bool Status { get; set; } = true;
        public List<ValidationMismatch> AbnormalFields { get; set; } = new();
    }
}
