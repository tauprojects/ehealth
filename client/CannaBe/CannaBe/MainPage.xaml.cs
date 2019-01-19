using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
        }

        private void GoToLoginPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void GoToRegisterPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void GoToDashboardPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void LocalHostDebugChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (LocalHostDebug.IsChecked == true)
                Constants.IsLocalHost = true;
            else
                Constants.IsLocalHost = false;
        }

        private void Page_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LocalHostDebug.IsChecked = Constants.IsLocalHost;
        }
    }
}
