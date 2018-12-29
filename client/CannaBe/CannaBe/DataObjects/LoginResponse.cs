using CannaBe.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class LoginResponse : IJsonResponse
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
        public List<MedicalEnum> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        public List<PositivePreferencesEnum> PositivePreferences { get; set; }

        [JsonProperty("negative")]
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

            MedicalNeeds = MedicalEnumMethods.FromStringList(medicalNeeds);
            PositivePreferences = PositivePreferencesEnumMethods.FromStringList(positivePreferences);
            NegativePreferences = NegativePreferencesEnumMethods.FromStringList(negativePreferences);
        }
    }
}
