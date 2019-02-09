using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class UsageDisplay : Page
    {
        public UsageDisplay()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var u = UsageContext.DisplayUsage;

            if (u != null)
            {
                StrainChosenText.Text = u.UsageStrain.Name;
                StartTime.Text = "Start Time: " + u.StartTime.ToString("dd.MM.yy HH:mm:ss");
                EndTime.Text = "End Time: " + u.EndTime.ToString("dd.MM.yy HH:mm:ss");
                Duration.Text = "Duration: " + u.DurationString;

                if (u.usageFeedback != null)
                {
                    foreach (var q in u.usageFeedback)
                    {
                        var t = new TextBlock()
                        {
                            Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black),
                            FontSize = 22,
                            TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        t.Inlines.Add(new Run()
                        {
                            FontWeight = FontWeights.Bold,
                            Text = q.Key + " "
                        });
                        t.Inlines.Add(new Run()
                        {
                            Text = q.Value
                        });
                        //t.Inlines.Add(new LineBreak());

                        Questions.Children.Add(t);
                    }
                }
                else
                {
                    Scroller.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GoBack(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            UsageContext.DisplayUsage = null;
            Frame.Navigate(typeof(UsageHistory));
        }
    }
}
