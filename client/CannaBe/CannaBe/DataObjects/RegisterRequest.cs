using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe.DataObjects
{
    class RegisterRequest
    {
        [JsonProperty("username")]
        string Username { get; set; }

        [JsonProperty("password")]
        string Password { get; set; }

        [JsonProperty("age")]
        string DOB { get; set; }

        [JsonProperty("gender")]
        string Gender { get; set; }

        [JsonProperty("country")]
        string Country { get; set; }

        [JsonProperty("city")]
        string City { get; set; }

        public RegisterRequest(string username, string password, string dob, string gender, string country, string city)
        {
            this.Username = username;
            this.Password = password;
            this.DOB = dob;
            this.Gender = gender;
            this.Country = country;
            this.City = city;
        }

        public static implicit operator HttpContent(RegisterRequest req)
        {
            //from: https://stackoverflow.com/questions/23585919/send-json-via-post-in-c-sharp-and-receive-the-json-returned

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(req);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
