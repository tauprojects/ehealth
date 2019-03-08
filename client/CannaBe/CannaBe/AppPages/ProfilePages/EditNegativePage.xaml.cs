using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CannaBe.AppPages.ProfilePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditNegativePage : Page
    {
        public EditNegativePage()
        {
            this.InitializeComponent();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

      

        private void BackToPositive(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditNegativeEffectsGrid, out List<int> intList);
            GlobalContext.RegisterContext.IntNegativePreferences = intList;

            Frame.Navigate(typeof(EditPositivePage));
        }

        private async void EditRegister(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            var user_id = GlobalContext.CurrentUser.Data.UserID;


            try
            {
                progressRing.IsActive = true;

                PagesUtilities.GetAllCheckBoxesTags(EditNegativeEffectsGrid,
                out List<int> intList);

                GlobalContext.RegisterContext.IntNegativePreferences = intList;
                res = await HttpManager.Manager.Post(Constants.MakeUrl($"edit/{user_id}"), GlobalContext.RegisterContext);


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
                AppDebug.Exception(exc, "Register");
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }
    }
}
