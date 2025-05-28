using Newtonsoft.Json;

namespace Validator.Models
{
    public  class ParameterDefenition
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("types")]
        public List<string> Types { get; set; } = new();
        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
