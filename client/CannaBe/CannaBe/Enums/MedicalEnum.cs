using System;
using System.Collections.Generic;

namespace CannaBe.Enums
{
    enum MedicalEnum
    {
        SEIZURES = 1,
        MUSCLE_SPASMS = 2,
        SPASTICITY = 3,
        INFLAMMATION = 4,
        EYE_PRESSURE = 5,
        HEADACHES = 6,
        FATIGUE = 7,
        NAUSEA = 8,
        LACK_OF_APPETITE = 9,
        CRAMPS = 10,
        STRESS = 11,
        PAIN = 12,
        DEPRESSION = 13        
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

            foreach(int i in intList)
            {
                enumList.Add((MedicalEnum)i);
            }

            return enumList;
        }

        public static List<MedicalEnum> FromStringList(List<string> strList)
        {
            AppDebug.Line("MedicalEnum isStrNull = "+ (strList == null).ToString());

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
