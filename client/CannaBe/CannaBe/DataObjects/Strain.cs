using CannaBe.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        [JsonProperty("race")]
        public string Race;

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
                //AppDebug.Line($"Setting BitmapMedicalNeeds {value}");
                bitmapMedicalNeeds = value;
                MedicalNeeds = value.FromBitmapToEnumList<MedicalEnum>();
                //foreach(var i in MedicalNeeds)
                //{
                //    AppDebug.Line($"\tGot {i.ToString()}");
                //}
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
                //AppDebug.Line($"Setting BitmapPositivePreferences {value}");
                bitmapPositivePreferences = value;
                PositivePreferences = value.FromBitmapToEnumList<PositivePreferencesEnum>();
                //foreach (var i in PositivePreferences)
                //{
                //    AppDebug.Line($"\tGot {i.ToString()}");
                //}
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
                //AppDebug.Line($"Setting BitmapNegativePreferences {value}");
                bitmapNegativePreferences = value;
                NegativePreferences = value.FromBitmapToEnumList<NegativePreferencesEnum>();
                //foreach (var i in NegativePreferences)
                //{
                //    AppDebug.Line($"\tGot {i.ToString()}");
                //}
            }
        }

        public List<NegativePreferencesEnum> NegativePreferences { get; set; }

        [JsonConstructor]
        public Strain(string name, int iD, int medicalNeeds,
            int positivePreferences,
            int negativePreferences)
        {
            Name = name;
            ID = iD;
            BitmapMedicalNeeds = medicalNeeds;
            BitmapPositivePreferences = positivePreferences;
            BitmapNegativePreferences = negativePreferences;
        }

        public Strain(string name, int iD)
        {
            Name = name;
            ID = iD;
        }

        public string PropertiesString
        {
            get
            {
                return GetPropertiesString();
            }
        }

        public string GetPropertiesString()
        {
            StringBuilder b = new StringBuilder();
            int i = 1;

            if (MedicalNeeds.Count > 0)
            {
                b.AppendLine("- Medical Needs:");
                foreach (var mn in MedicalNeeds)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No medical needs listed.");
            }

            if (PositivePreferences.Count > 0)
            {
                b.AppendLine("- Positive Effects:");
                i = 1;
                foreach (var mn in PositivePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No positive effects listed.");
            }

            if (NegativePreferences.Count > 0)
            {
                b.AppendLine("- Negative Effects:");
                i = 1;
                foreach (var mn in NegativePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No negative effects listed.");
            }

            return b.ToString().Substring(0, b.Length - 2);
        }
    }
}
