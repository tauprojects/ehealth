using CannaBe.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace CannaBe
{
    class LoginResponse
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        public long CreatedAt { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("dob")]
        public string DOB { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("medical")]
        private List<string> StringMedicalNeeds { get; set; }

        public List<MedicalEnum> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        private List<string> StringPositivePreferences { get; set; }

        public List<PositivePreferencesEnum> PositivePreferences { get; set; }

        [JsonProperty("negative")]
        private List<string> StringNegativePreferences { get; set; }
        public List<NegativePreferencesEnum> NegativePreferences { get; set; }

        [JsonConstructor]
        public LoginResponse(string userID, string username, string dOB, 
            string gender, string country, string city, 
            List<string> medicalNeeds, 
            List<string> positivePreferences,
            List<string> negativePreferences,
            long createdAt)
        {
            UserID = userID;
            CreatedAt = createdAt;
            Username = username;
            DOB = dOB;
            Gender = gender;
            Country = country;
            City = city;

            StringMedicalNeeds = medicalNeeds;
            StringPositivePreferences = positivePreferences;
            StringNegativePreferences = negativePreferences;
        }

        public static LoginResponse CreateFromHttpResponse(HttpResponseMessage msg)
        {
            return HttpManager.ParseJson<LoginResponse>(msg);
        }

        public static LoginResponse CreateFromHttpResponse(object msg)
        {
            return HttpManager.ParseJson<LoginResponse>(msg as HttpResponseMessage);
        }
    }
}
