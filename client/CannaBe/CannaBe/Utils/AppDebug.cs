using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;

namespace CannaBe
{
    class AppDebug
    {
        private static StorageFile LogFile = null;

        public static void Line(object msg, bool OmitDate)
        {
            if (!OmitDate)
                msg = DateTime.Now.ToString("HH:mm:ss.ffffff") + " " + msg;

            Debug.WriteLine(msg);

            try
            {
                if (LogFile == null)
                {
                    Init();
                }

                FileIO.AppendTextAsync(LogFile, msg.ToString() + Environment.NewLine).GetAwaiter().GetResult();
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc);
            }
        }

        public static void Init()
        {
            if(LogFile == null)
            {
                Task.Run(() =>
                {
                    StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
                    LogFile = storageFolder.CreateFileAsync("CannaBeLogFile.txt", CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
                    String log = $"*** Start New Log Session at {DateTime.Now.ToString("dd/MM/yy HH:mm:ss.ffffff")} ***";
                    FileIO.AppendTextAsync(LogFile, log + Environment.NewLine).GetAwaiter().GetResult();
                    Debug.WriteLine(log);
                });
            }
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
