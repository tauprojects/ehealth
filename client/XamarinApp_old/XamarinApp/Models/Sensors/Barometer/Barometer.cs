using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App2.Models;

namespace Band
{

    public class BarometerModel : ViewModel
    {
        public delegate void ChangedHandler(double temperature, double airPressure);
        public event ChangedHandler Changed;


        public async Task InitAsync()
        {
            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.Barometer.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.Barometer.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.Barometer.ReadingChanged += Barometer_ReadingChanged;
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Barometer.StartReadingsAsync();
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Barometer.StopReadingsAsync();
            }
        }

        async void Barometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandBarometerReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     BarometerSensorReading reading = new BarometerSensorReading { Temperature = e.SensorReading.Temperature };
                     if (Changed != null)
                     {
                         AppDebug.line("Barometer_ReadingChanged value<" + reading.Value + ">");
                         Changed(reading.Value, e.SensorReading.AirPressure);
                     }
                 });
        }
    }
}
