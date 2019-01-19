using CannaBe.AppPages.PostTreatmentPages;
using System;
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

            if (UsageContext.Usage.UseBandData)
            {
                UsageContext.Usage.Handler += HeartRateUpdateScreen;
            }
            else
            {
                Acquiring.Opacity = 0.85;
                BandIsNA.Visibility = Visibility.Visible;
                progressRing.IsActive = false;
            }
        }

        private void HeartRateUpdateScreen(double avg, int min, int max)
        {
            progressRing.IsActive = false;
            Acquiring.Visibility = Visibility.Collapsed;
            Min.Text = min.ToString();
            Avg.Text = Convert.ToInt32(avg).ToString();
            Max.Text = max.ToString();
        }

        private void EndSession(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            try
            {
                if(UsageContext.Usage.UseBandData)
                {
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
