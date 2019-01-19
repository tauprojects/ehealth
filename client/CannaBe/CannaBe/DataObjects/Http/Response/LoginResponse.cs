using CannaBe.AppPages.Usage;
using CannaBe.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
        public List<string> StringMedicalNeeds { get; set; }

        public List<MedicalEnum> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        public List<string> StringPositivePreferences { get; set; }

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

            StringMedicalNeeds = new List<string> {"SEIZURES"};
            MedicalNeeds = MedicalEnumMethods.FromStringList(StringMedicalNeeds);
            StringPositivePreferences = positivePreferences;
            //PositivePreferences = PositivePreferencesEnumMethods.FromStringList(StringPositivePreferences);
            StringNegativePreferences = negativePreferences;
            //NegativePreferences = NegativePreferencesEnumMethods.FromStringList(StringNegativePreferences);

        }

        public static LoginResponse CreateFromHttpResponse(HttpResponseMessage msg)
        {
            return HttpManager.ParseJson<LoginResponse>(msg);
        }

        public static LoginResponse CreateFromHttpResponse(object msg)
        {
            var httpMsg = msg as HttpResponseMessage;
            var res = HttpManager.ParseJson<LoginResponse>(httpMsg);

            //var values = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(httpMsg.Content.ReadAsStringAsync().Result);

            //res.StringMedicalNeeds = values["medical"].ToList();
            //res.StringPositivePreferences = values["positive"].ToList();
            //res.StringNegativePreferences = values["negative"].ToList();

            return res;
        }
    }
}
