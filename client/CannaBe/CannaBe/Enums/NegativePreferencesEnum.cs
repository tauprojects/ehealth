using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe.Enums
{
    enum NegativePreferencesEnum
    {
        DIZZY = 1,
        DRY_MOUTH = 2,
        PARANOID = 3,
        DRY_EYES = 4,
        ANXIOUS = 5
    }

    static class NegativePreferencesEnumMethods
    {
        public static List<string> FromIntToStringList(List<int> intList)
        {
            List<string> strList = new List<string>(intList.Count);

            foreach (int i in intList)
            {
                strList.Add(((NegativePreferencesEnum)i).ToString());
            }

            return strList;
        }


        public static List<NegativePreferencesEnum> FromIntList(List<int> intList)
        {
            List<NegativePreferencesEnum> enumList = new List<NegativePreferencesEnum>(intList.Count);

            foreach (int i in intList)
            {
                enumList.Add((NegativePreferencesEnum)i);
            }

            return enumList;
        }

        public static List<NegativePreferencesEnum> FromStringList(List<string> strList)
        {
            List<NegativePreferencesEnum> enumList = new List<NegativePreferencesEnum>(strList.Count);

            foreach (string s in strList)
            {
                try
                {
                    Enum.TryParse(s, out NegativePreferencesEnum val);
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
