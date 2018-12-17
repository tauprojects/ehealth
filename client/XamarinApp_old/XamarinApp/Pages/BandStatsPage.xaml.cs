using App2.Models;
using Band;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band.Sensors;
using Microsoft.Band.Notifications;

namespace XamarinApp
{
    public sealed partial class BandStatsPage : Page
    {
        HeartRateModel  _hearRateModel;
        SkinTempModel   _skinTempModel;
        SkinResModel    _skinResModel;
        ContactModel    _contactModel;
        BarometerModel  _barometerModel;

        public BandStatsPage()
        {
            this.InitializeComponent();

            _hearRateModel = new HeartRateModel();
            _skinTempModel  = new SkinTempModel();
            _skinResModel   = new SkinResModel();
            _contactModel   = new ContactModel();
            _barometerModel = new BarometerModel();
        }

        private async void InitBandOperations(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 1);
            bandContact.Text = "Connecting to band...";
            bandContact.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray);

            do
            {
                await BandModel.InitAsync();

                if (!BandModel.IsConnected)
                {
                    AppDebug.line("Band NOT CONNECTED");
                    bandContact.Text = "Band not found!";
                    bandContact.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red);
                    break;
                }
                bandContact.Text = "Initializing HeartRate...";

                AppDebug.line("Band CONNECTED");

                InitHeartRate(sender, e);

                bandContact.Text = "Initializing SkinTemp...";

                InitSkinTemp(sender, e);
                bandContact.Text = "Initializing Gsr...";

                InitSkinResistance(sender, e);
                bandContact.Text = "Initializing AirTemp...";

                InitBarometer(sender, e);

                //stay at the end for loading to be replaced with band worn /not worn only when everything finished
                InitBandContact(sender, e);

                // send a vibration request of type alert alarm to the Band
                await BandModel.BandClient.NotificationManager.VibrateAsync(VibrationType.NotificationAlarm);

            } while (false);
           
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
        }

        private async void InitHeartRate(object sender, RoutedEventArgs e)
        {
            try
            {
                // Heart Rate
                await _hearRateModel.InitAsync();
                _hearRateModel.Changed += _heartRaterModel_Changed;
                _hearRateModel.Start();
            }
            catch (Exception x)
            {
                AppDebug.line("Exception caught in heartRate button");
                AppDebug.line(x.Message);
                AppDebug.line(x.StackTrace);
            }
        }

        void _heartRaterModel_Changed(int heart_rate, double accuracy)
        {
            if (_contactModel.State == BandContactState.Worn)
            {
                if (accuracy < 1)
                {
                    heartRate.Text = "";
                }
                else
                {
                    heartRate.Text = "Heart Rate:\t" + heart_rate;
                }
            }
            else
            {
                heartRate.Text = "";
            }
        }




        private async void InitSkinTemp(object sender, RoutedEventArgs e)
        {
            // Heart Rate
            await _skinTempModel.InitAsync();
            _skinTempModel.Changed += _skinTempModel_Changed;
            _skinTempModel.Start();
        }

        void _skinTempModel_Changed(double _skinTemp)
        {
            if (_contactModel.State == BandContactState.Worn)
            {
                skinTemp.Text = "Skin Temp:\t" + _skinTemp + "°C";
            }
            else
            {
                skinTemp.Text = "";
            }
}

        void _skinResModel_Changed(double _skinRes)
        {
            if (_contactModel.State == BandContactState.Worn)
            {
                skinRes.Text = "Gsr :\t" + _skinRes.ToString("0.000") + " μS";
            }
            else
            {
                skinRes.Text = "";
            }
        }

        private async void InitSkinResistance(object sender, RoutedEventArgs e)
        {
            await _skinResModel.InitAsync();
            _skinResModel.Changed += _skinResModel_Changed;
            _skinResModel.Start();
        }

        void _contactModel_Changed(BandContactState _state)
        {
            if (_state == BandContactState.NotWorn)
            {
                bandContact.Text = "Band Not Worn";
                bandContact.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                bandContact.Text = "Band Worn";
                bandContact.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Green);
            }
        }

        private async void InitBandContact(object sender, RoutedEventArgs e)
        {
            await _contactModel.InitAsync();
            _contactModel.Changed += _contactModel_Changed;
            _contactModel.Start();
        }
        


        void _barometer_Changed(double temperature, double airPressure)
        {
            airTemp.Text = "Air Temp :\t" + temperature.ToString("0.00") + "°C";
        }

        private async void InitBarometer(object sender, RoutedEventArgs e)
        {
            await _barometerModel.InitAsync();
            _barometerModel.Changed += _barometer_Changed;
            _barometerModel.Start();
        }
    }
}
