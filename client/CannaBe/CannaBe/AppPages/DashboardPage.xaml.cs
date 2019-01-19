using CannaBe.AppPages;
using CannaBe.AppPages.PostTreatmentPages;
using CannaBe.AppPages.Usage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }
        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == ("Enter " + textBoxSender.Name))
            {
                textBoxSender.Text = "";
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
            }
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text.Length == 0)
            {
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            AppDebug.Line("In Dashboard page");

            if (e.Parameter == null)
                return;

            GlobalContext.AddUserToContext(e);

            Welcome.Text = $"Welcome, {GlobalContext.CurrentUser.Data.Username}!";

            AppDebug.Line($"Wrote welocme text: [{Welcome.Text}]");

            //Welcome.Text = "Welcome, User!";

            //AppDebug.Line("Dashboard: from " + req.GetType().Name);
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void GoToInformationPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InformationPage));
        }

        private void GoToPostTreatment(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostTreatment));
        }
   
        private void LogoutHandler(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void GoToStartUsage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }

        private void GoToUsageHistory(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UsageHistory));
        }
    }
}
