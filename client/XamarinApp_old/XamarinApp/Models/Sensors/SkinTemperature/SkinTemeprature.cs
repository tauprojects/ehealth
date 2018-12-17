using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App2.Models;

namespace Band
{

    public class SkinTempModel : ViewModel
    {
        public delegate void ChangedHandler(double skinTemp);
        public event ChangedHandler Changed;
        public static int peak = 0;
        public static double sum = 0;
        public static int count = 0;

        public async Task InitAsync()
        {
            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.SkinTemperature.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.SkinTemperature.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.SkinTemperature.ReadingChanged += SkinTemp_ReadingChanged;
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.SkinTemperature.StartReadingsAsync();
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.SkinTemperature.StopReadingsAsync();
            }
        }

        async void SkinTemp_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandSkinTemperatureReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     SkinTempSensorReading reading = new SkinTempSensorReading { SkinTemp = e.SensorReading.Temperature };
                     if (Changed != null)
                     {
                         AppDebug.line("SkinTemp_ReadingChanged value<" + reading.Value + ">");
                         Changed(reading.Value);
                     }
                 });
        }
    }
}
