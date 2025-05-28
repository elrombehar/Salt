using Newtonsoft.Json;

namespace Validator.Models
{
    public class EpModelDefenition
    {
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;
        [JsonProperty("method")]
        public string Method { get; set; } = string.Empty;
        [JsonProperty("query_params")]
        public List<ParameterDefenition> QueryParams { get; set; } = [];
        [JsonProperty("headers")]
        public List<ParameterDefenition> Headers { get; set; } = [];
        [JsonProperty("body")]
        public List<ParameterDefenition> Body { get; set; } = [];
    }
}
