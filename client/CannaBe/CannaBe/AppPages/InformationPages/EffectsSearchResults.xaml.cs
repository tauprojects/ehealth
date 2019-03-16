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
    public sealed partial class EffectsSearchResults : Page
    {
        public EffectsSearchResults()
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
            searchByEffects(GlobalContext.searchResult);
        }

        private void searchByEffects(string req)
        {
            SuggestedStrains strains = JsonConvert.DeserializeObject<SuggestedStrains>(req);
            if ( (strains.suggestedStrains.Count == 0) || (strains.status != 0) )
            {
                Status.Text = "No strains found - Please narrow search parameters.";
            }
            if (strains.status == 0)
            {
                foreach (Strain s in strains.suggestedStrains)
                {
                    strainListGui.Items.Add(s);
                }
            }
        }
        private void strainSelected(object sender, ItemClickEventArgs e)
        {
            ListView lst = sender as ListView;
            Strain s = e.ClickedItem as Strain;
            Frame.Navigate(typeof(StrainSearchResults), s);
        }

        private void BackToSearchPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StrainInformationPage));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.searchResult = null;
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
