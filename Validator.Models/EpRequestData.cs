using Newtonsoft.Json;

namespace Validator.Models
{
    public class EpRequestData
    {
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;
        [JsonProperty("method")]
        public string Method { get; set; } = string.Empty;
        [JsonProperty("query_params")]
        public List<RequestParameterDefenition> QueryParams { get; set; } = [];
        [JsonProperty("headers")]
        public List<RequestParameterDefenition> Headers { get; set; } = [];
        [JsonProperty("body")]
        public List<RequestParameterDefenition> Body { get; set; } = [];
    }
}
