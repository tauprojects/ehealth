using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace CannaBe
{
    class Strain : ViewModel
    {
        private string name;

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

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

        public Strain(string name, int iD)
        {
            Name = name;
            ID = iD;
        }
    }
}
