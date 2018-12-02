using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App2.Models;

namespace Band
{

    public class HeartRateModel : ViewModel
    {
        public delegate void ChangedHandler(int heartRate, double quality);
        public event ChangedHandler Changed;
        public static int peak = 0;
        public static double sum = 0;
        public static int count = 0;

        public async Task InitAsync()
        {
            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.HeartRate.ReadingChanged += HeartRate_ReadingChanged;
                AppDebug.line("HeartRateModel InitAsync");
            }
        }

        public void Start()
        {
            try
            {
                AppDebug.line("HeartRateModel Start");

                if (BandModel.IsConnected)
                {
                    AppDebug.line("HeartRateModel reading..");

                    BandModel.BandClient.SensorManager.HeartRate.StartReadingsAsync();

                    AppDebug.line("HeartRateModel read..");

                }

            }
            catch (Exception x)
            {
                AppDebug.line("Exception caught in Start");
                AppDebug.line(x.Message);
                AppDebug.line(x.StackTrace);
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.HeartRate.StopReadingsAsync();
            }
        }

        async void HeartRate_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     HeartRateSensorReading reading = new HeartRateSensorReading { HeartRate = e.SensorReading.HeartRate, Quality = e.SensorReading.Quality };
                     if (Changed != null)
                     {
                         AppDebug.line("HeartRate_ReadingChanged value<" + reading.Value + "> ,accuracy<"+ reading.Accuracy + ">");
                         Changed(reading.Value, reading.Accuracy);
                     }
                 });
        }
    }
}
