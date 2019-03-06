
using System;

namespace CannaBe
{
    static class Constants
    {
        public const string CannaBeUrl = "http://medicanna.westeurope.cloudapp.azure.com:8080/";
        public const string CannaBeUrlLocalHost = "http://localhost:8080/";

        public static bool IsLocalHost = false;

        public static string MakeUrl(string addition)
        {
            if (!IsLocalHost)
                return CannaBeUrl + addition;
            else
                return CannaBeUrlLocalHost + addition;
        }

        public static int ToAge(this Da♠teTime dob)
        {
            try
            {
                //taken from here: stackoverflow.com/a/11942
                int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int dobInt = int.Parse(dob.ToString("yyyyMMdd"));
                return (now - dobInt) / 10000;
            }
            catch(Exception x)
            {
                AppDebug.Exception(x, "GetAgeFromDob");
                return 0;
            }

        }
    }
}
