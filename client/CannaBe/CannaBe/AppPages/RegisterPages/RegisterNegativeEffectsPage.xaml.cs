using CannaBe.Enums;
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
        public RegisterNegativeEffectsPage()
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
            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                                     req.IntNegativePreferences);
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterNegativeEffectsPage.OnNavigatedTo");

                }
            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void BackToPositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                    out List<int> intList);
            GlobalContext.RegisterContext.IntNegativePreferences = intList;

            Frame.Navigate(typeof(RegisterPositiveEffectsPage));
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;

            try
            {
                PagesUtilities.StartProgressRing(sender);

                PagesUtilities.GetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                                    out List<int> intList);
                
                GlobalContext.RegisterContext.IntNegativePreferences = intList;
                GlobalContext.RegisterContext.NegativePreferences = NegativePreferencesEnumMethods.FromIntToStringList(intList);

                res = await HttpManager.Manager.Post(Constants.MakeUrl("register"), GlobalContext.RegisterContext);

                if (res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Status.Text = "Register Successful!";
                        PagesUtilities.SleepSeconds(1);
                        Frame.Navigate(typeof(DashboardPage), res);
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
            finally
            {
                PagesUtilities.StopProgressRing();
            }
        }
    }
}
