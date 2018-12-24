using Newtonsoft.Json;
using System.Net.Http;

namespace CannaBe
{
    class BaseResponse
    {
        [JsonProperty("request_id")]
        string RequestId { get; set; }

        [JsonProperty("status")]
        string Status { get; set; }

        [JsonProperty("body")]
        string Body { get; set; }

        public BaseResponse(string requestId, string status, string body)
        {
            RequestId = requestId;
            Status = status;
            Body = body;
        }

        public BaseResponse(HttpResponseMessage msg)
        {
            var str = msg.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<BaseResponse>(str);

            RequestId = response.RequestId;
            Status = response.Status;
            Body = response.Body;
        }
    }
}
