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

        [JsonProperty("medical")]
        public List<string> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        public List<string> PositivePreferences{ get; set; }

        [JsonProperty("negative")]
        public List<string> NegativePreferences { get; set; }

        public List<int> IntMedicalNeeds  { get; set; }
        public List<int> IntPositivePreferences  { get; set; }
        public List<int> IntNegativePreferences  { get; set; }

        public RegisterRequest() { }

        public RegisterRequest(string username, string password, string dob, string gender, string country, string city) 
            : base(username, password)
        {
            DOB = dob;
            Gender = gender;
            Country = country;
            City = city;
            MedicalNeeds = new List<string>();
            PositivePreferences = new List<string>();
            NegativePreferences = new List<string>();
        }
    }
}
