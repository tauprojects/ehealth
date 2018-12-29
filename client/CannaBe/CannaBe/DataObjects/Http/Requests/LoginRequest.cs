using Newtonsoft.Json;

namespace CannaBe
{
    class LoginRequest : Request
    {
        [JsonProperty("username")]
        string Username { get; set; }

        [JsonProperty("password")]
        string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
