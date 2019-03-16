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
            if (GlobalContext.searchType == 1)
            {
                var req = (string)e.Parameter;
                searchByStrain(req);
            }
            else if (GlobalContext.searchType == 2)
            {
                var req = (Strain)e.Parameter;
                searchFromEffects(req);
            }
            else
            {
                AppDebug.Line("Invalid navigation!");
            }
        }

        private void searchFromEffects(Strain req)
        {
            strain.Text = req.Name + ":";
            desc.Text = req.Description;
            score.Text = "Overall rank by users: " + req.Rank;
        }

        private void searchByStrain(string req)
        {
            string name = "", description = "", rank = "", status = "";
            res = JsonConvert.DeserializeObject<Dictionary<string, string>>(req);
            try
            {
                res.TryGetValue("name", out name);
                res.TryGetValue("description", out description);
                res.TryGetValue("rank", out rank);
                strain.Text = name + ":";
                desc.Text = description;
                score.Text = "Overall rank by users: " + rank;
            }
            catch
            {
                res.TryGetValue("status", out status);
                if (int.Parse(status) == 400) Status.Text = "Not a valid strain name - Please try again.";
            }
        }

        private void BackToSearchPage(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalContext.searchType == 1) Frame.Navigate(typeof(StrainInformationPage));
            else if (GlobalContext.searchType == 2) Frame.Navigate(typeof(EffectsSearchResults));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.searchResult = null;
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
