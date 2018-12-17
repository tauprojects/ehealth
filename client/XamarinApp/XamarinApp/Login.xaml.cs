using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace XamarinApp
{
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();

            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            textBoxSender.Text = "";
            textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if(textBoxSender.Text.Length == 0)
            {
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Gray);
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }
    }
}
