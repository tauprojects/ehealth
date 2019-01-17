using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using CannaBe;
using System.Threading.Tasks;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage2 : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();

        public StartUsage2()
        {
            this.InitializeComponent();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 1, 0); //so time would be updated
        }

        private void Timer_Tick(object sender, object e)
        {
            StartTime.Text = "Start Time: " + DateTime.Now.ToString("dd.MM.yy HH:mm");
        }

        private void OnPageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(UsageContext.ChosenStrain != null)
            {
                StrainChosenText.Text = UsageContext.ChosenStrain.Name;
                Timer.Start();
                Timer_Tick(sender, e); //current time, before tick
            }
            else
            {
                StrainChosenText.Text = "Error: No strain chosen!";
            }
        }

        private void GoBack(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }

        private void UseBandChanged(object sender, RoutedEventArgs e)
        {
            var b = sender as CheckBox;

            if(b.IsChecked.Value)
            {
                PairBandButton.IsEnabled = true;
            }
            else
            {
                PairBandButton.IsEnabled = false;
            }
        }


        private async void PairButtonClicked(object sender, RoutedEventArgs e)
        {
            StartAction();
            do
            {
                var isSupported = await BandContext.GetBluetoothIsEnabledAsync();

                if(!isSupported)
                {
                    EndAction();

                    await new MessageDialog("Please enable BlueTooth and pair phone with Band", "Error!").ShowAsync();
                    break;
                }

                GlobalContext.Band = new BandContext();
                try
                {
                    var isPaired = await GlobalContext.Band.PairBand().TimeoutAfter(new TimeSpan(0, 0, 15));
                    EndAction();

                    if (isPaired)
                    {
                        PairBandButton.Content = "Paired Successfully!";
                        PairBandButton.Width = double.NaN;
                        PairBandButton.IsEnabled = false;
                    }
                    else
                    {
                        await new MessageDialog("Did not find band\nPlease try again", "Pairing Failed!").ShowAsync();
                    }
                }
                catch(TimeoutException)
                {
                    EndAction();
                    await new MessageDialog("Pairing timed out after 15 seconds", "Pairing Failed!").ShowAsync();
                }
            } while (false);
        }

        private void StartAction()
        {
            progressRing.IsActive = true;
            BackButton.IsEnabled = false;
            ContinueButton.IsEnabled = false;
        }

        private void EndAction()
        {
            progressRing.IsActive = false;
            BackButton.IsEnabled = true;
            ContinueButton.IsEnabled = true;
        }

        private void StartSession(object sender, RoutedEventArgs e)
        {
            /*
            StartAction();
            UsageContext.Usage = new UsageData(GlobalContext.User, UsageContext.ChosenStrain, DateTime.Now);

            await Task.Run(() => GlobalContext.Band.StartHeartRate(UsageContext.Usage.HeartRateChanged));
            EndAction();
            */
        }
    }
}
;