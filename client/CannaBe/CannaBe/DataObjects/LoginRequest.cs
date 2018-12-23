using Newtonsoft.Json;
using System.Net.Http;

namespace CannaBe
{
    class LoginRequest
    {
        [JsonProperty("username")]
        string Username { get; set; }

        [JsonProperty("password")]
        string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public static implicit operator HttpContent(LoginRequest req)
        {
            return HttpManager.CreateJson(req);
        }
    }
}
