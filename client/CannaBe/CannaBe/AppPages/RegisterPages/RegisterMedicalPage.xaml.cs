using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterMedicalPage : Page
    {
        RegisterRequest registerRequest;
        public RegisterMedicalPage()
        {
            this.InitializeComponent();
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
        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            //PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            registerRequest = (RegisterRequest)e.Parameter;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToRegister(object sender, TappedRoutedEventArgs e)
        {
            registerRequest.MedicalNeeds = new List<int>();
            Frame.Navigate(typeof(RegisterPage), registerRequest);
        }

        private void ContinuePositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                                registerRequest.MedicalNeeds);
           
            Frame.Navigate(typeof(RegisterPositiveEffectsPage), registerRequest);
        }
    }
}
