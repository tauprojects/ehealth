using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
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
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ContinueMedicalRegister(object sender, TappedRoutedEventArgs e)
        {
            int flag = 0;
            try
            {
                if (!Gender.SelectedValue.ToString().Equals(null))
                {

                }
            }
            catch
            {
                flag = 1;
            }

            if (Username.Text == "")
            {
                Status.Text = "Please enter a valid username";
            }
            else if (Password.Text == "")
            {
                Status.Text = "Please enter a valid password";
            }
            else if (flag == 1)
            {
                Status.Text = "Please enter a valid gender";
            }
            else if (Country.Text == "")
            {
                Status.Text = "Please enter a valid country";
            }
            else if (City.Text == "")
            {
                Status.Text = "Please enter a valid city";
            }
            else
            {
                RegisterRequest registerRequest = new RegisterRequest(Username.Text, Password.Text, DOB.Date.Day + "/" + DOB.Date.Month + "/" + DOB.Date.Year, Gender.SelectedValue.ToString(), Country.Text, City.Text);
                Frame.Navigate(typeof(RegisterMedicalPage), registerRequest);
            }
        }

        private void BackToHome(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
