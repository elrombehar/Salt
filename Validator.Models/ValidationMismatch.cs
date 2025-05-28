namespace Validator.Models
{
    public class ValidationMismatch
    {
        public string Field { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty; // "query_params", "headers", "body"
        public string Reason { get; set; } = string.Empty;
        public object? ProvidedValue { get; set; }
        public List<string> ExpectedTypes { get; set; } = new();
    }
}
