using System;
using System.Diagnostics;

namespace App2.Models
{
    class AppDebug
    {
        public static void line(object msg, bool OmitDate)
        {
            if (!OmitDate)
                msg = DateTime.Now.ToString("HH:mm:ss.ffffff") + " " + msg;
            Debug.WriteLine(msg);
        }

        public static void line(object msg) { line(msg, false); }
    }
}
