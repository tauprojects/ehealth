using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace CannaBe
{
    static class AppDebug
    {
        private static StorageFile LogFile = null;

        public static void Line(object msg, bool OmitDate, string caller)
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
            catch (Exception exc)
            {
                Line($"!!! *** Exception caught in [{caller} => AppDebug.Line] *** !!!");
                Debug.WriteLine(exc);
            }
        }

        public static void Init()
        {
            Application.Current.UnhandledException += UnhandledExceptionEventHandler;

            if (LogFile == null)
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

        public static void Line(object msg, [CallerMemberName] string functionCaller = "")
        {
            Line(msg, false, functionCaller);
        }

        public static void Exception(Exception e, string caller,
            [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            Line($"!!! *** Exception caught in [{caller}] *** !!!");
            try { Line($"File: [{Path.GetFileName(filename)}:{lineNumber}]"); } catch { }
            Line(e);
        }

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Line($"!!! *** UNHANDLED Exception *** !!!");
            Line(e.Exception);
        }
    }
}
