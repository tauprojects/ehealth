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

        private int bitmapMedicalNeeds;

        [JsonProperty("medical")]
        public int BitmapMedicalNeeds
        {
            get
            {
                return bitmapMedicalNeeds;
            }
            set
            {
                AppDebug.Line($"Setting BitmapMedicalNeeds {bitmapMedicalNeeds}");
                bitmapMedicalNeeds = value;
                MedicalNeeds = value.FromBitmapToEnumList<MedicalEnum>();
                foreach(var i in MedicalNeeds)
                {
                    AppDebug.Line($"\tGot {i.ToString()}");
                }
            }
        }
        public List<MedicalEnum> MedicalNeeds { get; set; }

        private int bitmapPositivePreferences;

        [JsonProperty("positive")]
        public int BitmapPositivePreferences
        {
            get
            {
                return bitmapPositivePreferences;
            }
            set
            {
                AppDebug.Line($"Setting BitmapPositivePreferences {bitmapPositivePreferences}");
                bitmapPositivePreferences = value;
                PositivePreferences = value.FromBitmapToEnumList<PositivePreferencesEnum>();
                foreach (var i in PositivePreferences)
                {
                    AppDebug.Line($"\tGot {i.ToString()}");
                }
            }
        }
        public List<PositivePreferencesEnum> PositivePreferences { get; set; }

        private int bitmapNegativePreferences;

        [JsonProperty("negative")]
        public int BitmapNegativePreferences
        {
            get
            {
                return bitmapNegativePreferences;
            }
            set
            {
                AppDebug.Line($"Setting BitmapNegativePreferences {bitmapNegativePreferences}");
                bitmapNegativePreferences = value;
                NegativePreferences = value.FromBitmapToEnumList<NegativePreferencesEnum>();
                foreach (var i in NegativePreferences)
                {
                    AppDebug.Line($"\tGot {i.ToString()}");
                }
            }
        }

        public List<NegativePreferencesEnum> NegativePreferences { get; set; }

        [JsonConstructor]
        public LoginResponse(string userID, string username, string dOB,
            string gender, string country, string city,
            int medicalNeeds,
            int positivePreferences,
            int negativePreferences,
            long createdAt)
        {
            UserID = userID;
            CreatedAt = createdAt;
            Username = username;
            DOB = dOB;
            Gender = gender;
            Country = country;
            City = city;

            BitmapMedicalNeeds = medicalNeeds;
            BitmapPositivePreferences = positivePreferences;
            BitmapNegativePreferences = negativePreferences;

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
