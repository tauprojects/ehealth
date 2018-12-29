using System;
using System.Collections.Generic;
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
            registerRequest.NegativePreferences = new List<int>();
            Frame.Navigate(typeof(RegisterPositiveEffectsPage), registerRequest);
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterNegativeEffectsGrid, 
                                                registerRequest.NegativePreferences);
           
            var req = registerRequest;

            HttpResponseMessage res = null;
            try
            {
                res = await HttpManager.Manager.Post(Constants.MakeUrl("register"), req);

                if(res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Status.Text = "Register Successful!";

                        //GlobalContext.User = new UserData("1234", //tryout
                        //                                    26, 
                        //                                    req.MedicalNeeds, 
                        //                                    req.PositivePreferences, 
                        //                                    req.NegativePreferences);
                        Frame.Navigate(typeof(DashboardPage), registerRequest);
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
