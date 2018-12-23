using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe
{
    static class Constants
    {
        public const string CannaBeUrl = "http://ehealth.westeurope.cloudapp.azure.com:8080/";
        public const string CannaBeUrlLocalHost = "http://localhost:8080/";

        public static bool IsLocalHost = false;

        public static string MakeUrl(string addition)
        {
            if (!IsLocalHost)
                return CannaBeUrl + addition;
            else
                return CannaBeUrlLocalHost + addition;
        }
    }
}
