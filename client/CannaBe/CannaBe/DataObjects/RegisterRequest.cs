using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class RegisterRequest : LoginRequest
    {
        [JsonProperty("dob")]
        public string DOB { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("medicalneeds")]
        public List<int> MedicalNeeds { get; set; }

        public RegisterRequest(string username, string password, string dob, string gender, string country, string city) 
            : base(username, password)
        {
            DOB = dob;
            Gender = gender;
            Country = country;
            City = city;
            MedicalNeeds = new List<int>();
        }
    }
}
