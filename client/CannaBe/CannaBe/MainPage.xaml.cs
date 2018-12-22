using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
    }
}
