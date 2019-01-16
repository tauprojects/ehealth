using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
    }
}
;