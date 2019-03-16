using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace CannaBe.AppPages.InformationPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StrainSearchResults : Page
    {
        Dictionary<string, string> res;
        public StrainSearchResults()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = (string)e.Parameter;
            string name = "", description = "", rank = "";
            res = JsonConvert.DeserializeObject<Dictionary<string, string>>(req);
            res.TryGetValue("name", out name);
            res.TryGetValue("description", out description);
            res.TryGetValue("rank", out rank);
            strain.Text = name + ":";
            desc.Text = description;
            score.Text = "Overall rank by users: " + rank;

        }

        private void BackToSearchPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StrainInformationPage));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
