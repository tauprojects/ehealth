using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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
            //from: https://stackoverflow.com/questions/23585919/send-json-via-post-in-c-sharp-and-receive-the-json-returned

            // Serialize our concrete class into a JSON String
            var stringPayload =  JsonConvert.SerializeObject(req);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
