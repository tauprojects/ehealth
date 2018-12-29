using Newtonsoft.Json;
using System.Net.Http;

namespace CannaBe
{
    class LoginResponse : IJsonResponse
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonConstructor]
        public LoginResponse(string requestId, string status, string body)
        {
            RequestId = requestId;
            Status = status;
            Body = body;
        }
    }
}
