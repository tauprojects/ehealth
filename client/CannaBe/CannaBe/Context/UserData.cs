using CannaBe.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class UserData
    {
        [JsonProperty("user_id")]
        public string UID { get; }

        [JsonProperty("age")]
        public int Age { get; }

        [JsonProperty("medical_needs")]
        public List<MedicalEnum> MedicalNeeds { get; }

        [JsonProperty("positive_effects")]
        public List<PositivePreferencesEnum> ChosenPositiveEffects { get; }

        [JsonProperty("negative_effects")]
        public List<NegativePreferencesEnum> ChosenNegativeEffects { get; }

        [JsonConstructor]
        public UserData(string uID, int age, List<int> medicalNeeds, List<int> chosenPositiveEffects, List<int> chosenNegativeEffects)
        {
            UID = uID;
            Age = age;

            MedicalNeeds = MedicalEnumMethods.FromIntList(medicalNeeds);
            ChosenPositiveEffects =PositivePreferencesEnumMethods.FromIntList(chosenPositiveEffects);
            ChosenNegativeEffects = NegativePreferencesEnumMethods.FromIntList(chosenNegativeEffects);
        }
    }
}
