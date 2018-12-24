using Newtonsoft.Json;
using System.Net.Http;

namespace CannaBe
{
    class RegisterRequest : LoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("dob")]
        public string DOB { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        public RegisterRequest(string username, string password, string dob, string gender, string country, string city) 
            : base(username, password)
        {
            Username = username;
            Password = password;
            DOB = dob;
            Gender = gender;
            Country = country;
            City = city;
        }

        public static implicit operator HttpContent(RegisterRequest req)
        {
            return HttpManager.CreateJson(req);
        }
    }
}
