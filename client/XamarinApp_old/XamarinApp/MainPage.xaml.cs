using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace XamarinApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GoToLoginPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }
    }
}
