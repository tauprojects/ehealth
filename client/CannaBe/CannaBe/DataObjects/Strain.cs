using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class Strain
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("id")]
        public int ID;

        [JsonProperty("medical")]
        public List<string> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        public List<string> PositivePreferences { get; set; }

        [JsonProperty("negative")]
        public List<string> NegativePreferences { get; set; }

        [JsonConstructor]
        public Strain(string name, int iD, List<string> medicalNeeds, List<string> positivePreferences, List<string> negativePreferences)
        {
            Name = name;
            ID = iD;
            MedicalNeeds = medicalNeeds;
            PositivePreferences = positivePreferences;
            NegativePreferences = negativePreferences;
        }
    }
}
