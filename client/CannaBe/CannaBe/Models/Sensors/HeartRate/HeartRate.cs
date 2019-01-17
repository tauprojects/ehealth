using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
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
            AppDebug.Line("HeartRateModel Starting...");

            if (BandModel.IsConnected)
            {
                AppDebug.Line("HeartRateModel Is connected...");

                if (BandModel.BandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    AppDebug.Line("HeartRateModel requesting user access");

                    await BandModel.BandClient.SensorManager.HeartRate.RequestUserConsentAsync();

                    AppDebug.Line("HeartRateModel user access success");

                }

                BandModel.BandClient.SensorManager.HeartRate.ReadingChanged += HeartRate_ReadingChanged;
                AppDebug.Line("HeartRateModel InitAsync");
            }
        }

        public void Start()
        {
            try
            {
                AppDebug.Line("HeartRateModel Start");

                if (BandModel.IsConnected)
                {
                    AppDebug.Line("HeartRateModel reading..");

                    BandModel.BandClient.SensorManager.HeartRate.StartReadingsAsync();

                    AppDebug.Line("HeartRateModel read..");

                }

            }
            catch (Exception x)
            {
                AppDebug.Line("Exception caught in Start");
                AppDebug.Line(x.Message);
                AppDebug.Line(x.StackTrace);
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.HeartRate.StopReadingsAsync();
            }
        }

        private void HeartRate_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            HeartRateSensorReading reading = new HeartRateSensorReading
            { HeartRate = e.SensorReading.HeartRate,
                Quality = e.SensorReading.Quality
            };
            Changed?.Invoke(reading.Value, reading.Accuracy);
        }
    }
}
