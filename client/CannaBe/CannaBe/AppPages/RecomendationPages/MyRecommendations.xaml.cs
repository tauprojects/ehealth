using CannaBe.AppPages.Usage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CannaBe.AppPages.RecomendationPages
{
    public sealed partial class MyRecomendations : Page
    {
        public MyRecomendations()
        {
            InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            if (GlobalContext.CurrentUser != null)
            {
                Message.Text = "Searching for matching strains based on your profile...";

                var user_id = GlobalContext.CurrentUser.Data.UserID;
                var url = Constants.MakeUrl($"/strains/recommended/{user_id}/");

                try
                {
                    var res = await HttpManager.Manager.Get(url);

                    if (res == null)
                    {
                        progressRing.IsActive = false;
                        return;
                    }

                    PagesUtilities.SleepSeconds(1);

                    var strains = await Task.Run(async () => JsonConvert.DeserializeObject<SuggestedStrains>(await res.Content.ReadAsStringAsync()));


                    int i = 1;
                    if (strains.suggestedStrains.Count == 0)
                    {
                        ErrorNoStrainFound.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        switch (strains.status)
                        {
                            case 0:
                                Message.Text = $"Showing {strains.suggestedStrains.Count} matching strains:";
                                break;

                            case 1:
                                Message.Text = $"No exact matches found!\nTry updating your positive preferences.\nShowing {strains.suggestedStrains.Count} partially matching strains:";
                                break;

                            case 2:
                                Message.Text = $"No exact matches found!\nTry updating your positive and medical preferences.\nShowing {strains.suggestedStrains.Count} partially matching strains:";
                                break;
                        }
                        Scroller.Height = Stack.ActualHeight - Message.ActualHeight;

                        if (strains.status != 0)
                        {
                            foreach (var strain in strains.suggestedStrains)
                            {
                                strain.MatchingPercent = strain / GlobalContext.CurrentUser;
                            }

                            strains.suggestedStrains.Sort((s1, s2) =>
                            {
                                var r = -1 * s1.MatchingPercent.CompareTo(s2.MatchingPercent);
                                if (r == 0)
                                {
                                    r = s1.Name.CompareTo(s2.Name);
                                }
                                return r;
                            });
                        }

                        foreach (var strain in strains.suggestedStrains)
                        {
                            string percent = strains.status != 0 ? string.Format(" ({0:0}% match)", strain.MatchingPercent) : "";
                            var r = new RadioButton()
                            {
                                Foreground = new SolidColorBrush(Windows.UI.Colors.Black),
                                FontSize = 15,
                                VerticalContentAlignment = VerticalAlignment.Top,
                                FontWeight = FontWeights.Bold,
                                Content = $"{i++}. {strain.Name}{percent}",
                                DataContext = strain
                            };
                            /*
                            var t = new ToolTip()
                            {
                                Content = "check"
                            };
                            ToolTipService.SetToolTip(r, t);
                            */
                            r.Checked += OnChecked;
                            StrainList.Children.Add(r);
                        }
                    }
                }
                catch (Exception x)
                {
                    AppDebug.Exception(x, "OnPageLoaded");
                }

                progressRing.IsActive = false;
            }

        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            ContinueButton.IsEnabled = true;
            UsageContext.ChosenStrain = (sender as RadioButton).DataContext as Strain;
        }

        private void GoToDashboard(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void ContinueHandler(object sender, RoutedEventArgs e)
        {
            if (UsageContext.ChosenStrain != null)
            {
                Frame.Navigate(typeof(StartUsage2));
            }
        }
    }
}
