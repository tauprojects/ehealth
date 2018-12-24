using System;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterNegativeEffectsPage : Page
    {
        RegisterRequest registerRequest;
        public RegisterNegativeEffectsPage()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            registerRequest = (RegisterRequest)e.Parameter;

        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToPositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPositiveEffectsPage));
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            int rescheck;
            string selectedNegativeEffects = string.Empty;
            CheckBox[] checkboxes = new CheckBox[] { AnxiousCheckbox, DryEyesCheckbox , ParanoidCheckbox , DryMouthCheckbox , DizzyCheckbox };
            foreach (CheckBox c in checkboxes)
            {
                if (c.IsChecked == true)
                {
                    System.Int32.TryParse(c.Tag.ToString(), out rescheck);
                    registerRequest.NegativePreferences.Add(rescheck);

                }
            }

            var req = registerRequest;


            HttpResponseMessage res = null;
            try
            {
                res = await HttpManager.Manager.Post(Constants.MakeUrl("register"), req);

                if(res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        //Frame.Navigate(typeof(DashboardPage), registerRequest);
                        //foreach (int x in registerRequest.PositivePreferences)
                        //{
                        //    AppDebug.Line(x);
                        //}
                        //AppDebug.Line("After Positive Effects");
                        //foreach (int x in registerRequest.NegativePreferences)
                        //{
                        //    AppDebug.Line(x);
                        //}
                        //AppDebug.Line("After Negative Effects");
                        Status.Text = "Register Successful!";
                    }
                    else
                    {
                        Status.Text = "Register failed! Status = " + res.StatusCode;
                    }
                }
                else
                {
                    Status.Text = "Register failed!\nPost operation failed";
                }
            }
            catch (Exception exc)
            {
                Status.Text = "Exception during regsiter:\n" + exc.Message;
            }

        }
    }
}
