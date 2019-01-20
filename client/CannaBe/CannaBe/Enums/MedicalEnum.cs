using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CannaBe.Enums
{
    enum MedicalEnum
    {
        [EnumDescriptions("Are your seizures less frequent?", "0")]
        SEIZURES = 1,
        [EnumDescriptions("Do your muscles fell less stiff?", "0")]
        MUSCLE_SPASMS = 2,
        [EnumDescriptions("Do your muscles fell less stiff?", "0")]
        SPASTICITY = 3,
        [EnumDescriptions("Is there a relief in you inflammation effect?", "0")]
        INFLAMMATION = 4,
        [EnumDescriptions("Is there an improvement in your eye pressure?", "0")]
        EYE_PRESSURE = 5,
        [EnumDescriptions("Do you have fewer headaches?", "Are your headaches calmer?")]
        HEADACHES = 6,
        [EnumDescriptions("Do you feel more energetic?", "0")]
        FATIGUE = 7,
        [EnumDescriptions("Are you feeling less nauseous?", "0")]
        NAUSEA = 8,
        [EnumDescriptions("Is your appetite larger?", "0")]
        LACK_OF_APPETITE = 9,
        [EnumDescriptions("Do you have less cramps?", "0")]
        CRAMPS = 10,
        [EnumDescriptions("Do you feel more relaxed?", "0")]
        STRESS = 11,
        [EnumDescriptions("Is your pain improving?", "Rate your pain:")]
        PAIN = 12,
        [EnumDescriptions("Do you feel less depressed?", "0")]
        DEPRESSION = 13,
        [EnumDescriptions("", "0")]
        INSOMNIA = 14,
        [EnumDescriptions("", "0")]
        HEADACHE = 15
    }

    static class MedicalEnumMethods
    {
        public static List<string> FromIntToStringList(List<int> intList)
        {
            List<string> strList = new List<string>(intList.Count);

            foreach (int i in intList)
            {
                strList.Add(((MedicalEnum)i).ToString());
            }

            return strList;
        }

        public static List<MedicalEnum> FromIntList(List<int> intList)
        {
            List<MedicalEnum> enumList = new List<MedicalEnum>(intList.Count);

            foreach (int i in intList)
            {
                MedicalEnum m = ((MedicalEnum)i);
                enumList.Add(m);
                AppDebug.Line(m.GetType().GetMember(m.ToString()));//.First().GetCustomAttributes<TAttribute>();
            }

            return enumList;
        }

        public static List<MedicalEnum> FromStringList(List<string> strList)
        {
            AppDebug.Line("MedicalEnum isStrNull = " + (strList == null).ToString());

            List<MedicalEnum> enumList = new List<MedicalEnum>(strList.Count);
            AppDebug.Line("Created List<MedicalEnum>");

            foreach (string s in strList)
            {
                try
                {
                    AppDebug.Line($"Trying to parse '{s}'");
                    Enum.TryParse(s, out MedicalEnum val);
                    enumList.Add(val);
                }
                catch
                {
                    AppDebug.Line($"Failed FromStringList:\nValue '{s}' unknown");
                }

            }

            return enumList;
        }
    }
}
