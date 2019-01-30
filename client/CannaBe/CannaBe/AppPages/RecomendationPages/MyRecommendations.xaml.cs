using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            if(GlobalContext.CurrentUser != null)
            {
                var user_id = GlobalContext.CurrentUser.Data.UserID;
                var url = Constants.MakeUrl($"/strains/recommended/{user_id}/");

                try
                {
                    var res = await HttpManager.Manager.Get(url);

                    if (res == null)
                        return;

                    var strains = JsonConvert.DeserializeObject<Strain[]>(res.Content.ReadAsStringAsync().Result);

                    int i = 0;
                    if(strains.Length == 0)
                    {
                        ErrorNoStrainFound.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        foreach (var strain in strains)
                        {
                            AppDebug.Line($"Got strain {i++}");
                            AppDebug.Line($"\tname = {strain.Name}");
                            StrainList.Children.Add(new RadioButton()
                            {
                                Foreground = new SolidColorBrush(Windows.UI.Colors.Black),
                                FontSize = 20,
                                VerticalContentAlignment = VerticalAlignment.Top,
                                FontWeight = FontWeights.Bold,
                                Content = strain.Name,
                                DataContext = strain
                            });
                        }
                    }
                }
                catch(Exception x)
                {
                    AppDebug.Exception(x, "OnPageLoaded");
                }

                progressRing.IsActive = false;
            }

        }

        private void GoToDashboard(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void ContinueHandler(object sender, RoutedEventArgs e)
        {

        }
    }
}
