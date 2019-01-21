using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CannaBe
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            //TextBox textBoxSender = sender as TextBox;

            //textBoxSender.SelectAll();
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            //TextBox textBoxSender = sender as TextBox;

            //textBoxSender.Select(0, 0);
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private async void PostLogin(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            progressRing.IsActive = true;

            try
            {
                var req = new LoginRequest(Username.Text, Password.Password);

                res = await HttpManager.Manager.Post(Constants.MakeUrl("login"), req);

                if (res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Status.Text = "Login success!";

                        PagesUtilities.SleepSeconds(1);
                        progressRing.IsActive = false;

                        Frame.Navigate(typeof(DashboardPage), res);
                    }
                    else
                    {
                        Status.Text = "Login failed! Status: " + res.StatusCode;
                    }
                }
                else
                {
                    Status.Text = "Login failed!\nPost operation failed";
                }

            }
            catch (Exception exc)
            {
                Status.Text = "Exception during login";

                AppDebug.Exception(exc, "PostLogin");
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (Username.Text.Length > 0 && Password.Password.Length > 0)
                {
                    PostLogin(sender, e);
                }
            }
        }
    }
}