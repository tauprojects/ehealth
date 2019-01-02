using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading;
using System;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage : Page
    {
        List<string> strainNames = null;

        public StartUsage()
        {
            this.InitializeComponent();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadStrainList(object sender, object e)
        {
            if(strainNames == null)
            {
                PagesUtilities.StartProgressRing(sender);
                try
                {
                    List<string[]> strains = File.ReadAllLines("Assets/strains.txt")
                                            .Select(a => a.Split('='))
                                            .ToList<string[]>();

                    strainNames = new List<string>(strains.Count);

                    foreach (string[] strain in strains)
                    {
                        strainNames.Add(strain[0].Replace('_',' '));
                    }

                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "LoadStrainList");
                }
                finally
                {
                    PagesUtilities.StopProgressRing();
                }
            }
        }

        private void GoToDashboard(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void StrainList_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;

                var lst = strainNames.Where(item => item.ToLower().Contains(sender.Text.ToLower())).ToList();
                if(lst.Count() == 0)
                {
                    lst.Add("No Result");
                }

                sender.ItemsSource = lst;
            }
        }
    }
}
