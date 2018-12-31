using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CannaBe
{
    class AppDebug
    {
        public static void Line(object msg, bool OmitDate)
        {
            if (!OmitDate)
                msg = DateTime.Now.ToString("HH:mm:ss.ffffff") + " " + msg;
            Debug.WriteLine(msg);
        }

        public static void Line(object msg) { Line(msg, false); }

        public static void Exception(Exception e, string caller)
        {
            Line("Exception caught in " + caller + ":");
            Line(e);
            Line("Type:\t" + e.Message);
            Line(e.StackTrace);
        }
    }
}
