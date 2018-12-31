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
        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);

            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                {
                    Username.Text = req.Username ?? "";
                    Password.Text = "";
                    Country.Text = req.Country ?? "";
                    City.Text = req.City ?? "";

                    if(req.Gender != null)
                    {
                        if (req.Gender == "Male")
                            Gender.SelectedIndex = 1;
                        else if (req.Gender == "Female")
                            Gender.SelectedIndex = 2;
                        else
                            Gender.SelectedIndex = 0;
                    }
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterPage.OnPageLoaded");
                }
            }
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
                if(GlobalContext.RegisterContext == null)
                {
                    GlobalContext.RegisterContext = new RegisterRequest();
                }

                GlobalContext.RegisterContext.Username  = Username.Text;
                GlobalContext.RegisterContext.Password  = Password.Text;
                GlobalContext.RegisterContext.DOB       = DOB.Date.Day + "/" + DOB.Date.Month + "/" + DOB.Date.Year;
                GlobalContext.RegisterContext.Gender    = Gender.SelectedValue.ToString();
                GlobalContext.RegisterContext.Country   = Country.Text;
                GlobalContext.RegisterContext.City      = City.Text;

                Frame.Navigate(typeof(RegisterMedicalPage), GlobalContext.RegisterContext);
            }
        }

        private void BackToHome(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.RegisterContext = null;
            Frame.Navigate(typeof(MainPage));
        }
    }
}
