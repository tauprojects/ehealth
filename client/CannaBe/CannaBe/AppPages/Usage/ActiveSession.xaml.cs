using CannaBe.AppPages.PostTreatmentPages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CannaBe.AppPages.Usage
{

    public sealed partial class ActiveSession : Page
    {
        public ActiveSession()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            StrainChosenText.Text = UsageContext.Usage.UsageStrain.Name;
            StartTime.Text = "Start Time: " + UsageContext.Usage.StartTime.ToString("dd.MM.yy HH:mm");
            UsageContext.Usage.AddTimerFunction(Timer_Tick);

            if (UsageContext.Usage.UseBandData)
            { // Using band
                UsageContext.Usage.Handler += HeartRateUpdateScreen;
            }
            else
            {
                Acquiring.Opacity = 0.85;
                BandIsNA.Visibility = Visibility.Visible;
                progressRing.IsActive = false;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            try
            { // Get time for usage
                TimeSpan timePassed = DateTime.Now.Subtract(UsageContext.Usage.StartTime);
                Duration.Text = $"Duration: {new DateTime(timePassed.Ticks).ToString("HH:mm:ss")}";
            }
            catch (Exception x2)
            {
                AppDebug.Exception(x2, "Timer_Tick");
            }
        }

        private void HeartRateUpdateScreen(double avg, int min, int max)
        { // Update heart rate from band
            progressRing.IsActive = false;
            Acquiring.Visibility = Visibility.Collapsed;
            Min.Text = min.ToString();
            Avg.Text = Convert.ToInt32(avg).ToString();
            Max.Text = max.ToString();
        }

        private void EndSessionAsync(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            try
            {
                if (UsageContext.Usage.UseBandData)
                { // Stop band usage
                    GlobalContext.Band.StopHeartRate();
                    PagesUtilities.SleepSeconds(0.5);
                }
                UsageContext.Usage.EndUsage();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "EndSession");
            }
            progressRing.IsActive = false;
            Frame.Navigate(typeof(PostTreatment));
        }
    }
}
