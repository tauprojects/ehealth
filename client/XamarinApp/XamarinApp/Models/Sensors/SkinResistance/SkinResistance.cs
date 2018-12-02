using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App2.Models;

namespace Band
{

    public class SkinResModel : ViewModel
    {
        public delegate void ChangedHandler(double SkinRes);
        public event ChangedHandler Changed;
        public static int peak = 0;
        public static double sum = 0;
        public static int count = 0;

        public async Task InitAsync()
        {
            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.Gsr.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.Gsr.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.Gsr.ReadingChanged += SkinRes_ReadingChanged;
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Gsr.StartReadingsAsync();
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Gsr.StopReadingsAsync();
            }
        }

        async void SkinRes_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandGsrReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     SkinResSensorReading reading = new SkinResSensorReading { SkinRes = e.SensorReading.Resistance };
                     if (Changed != null)
                     {
                         AppDebug.line("SkinRes_ReadingChanged value<" + reading.Value + ">");
                         Changed(reading.Value);
                     }
                 });
        }
    }
}
