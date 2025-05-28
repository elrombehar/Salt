using Newtonsoft.Json;

namespace Validator.Models
{
    public class RequestParameterDefenition
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("value")]
        public object? Value { get; set; }
    }
}
