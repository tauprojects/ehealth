using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


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

                    var dicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(res.Content.ReadAsStringAsync().Result);

                    int i = 0;
                    foreach (var dict in dicts)
                    {
                        AppDebug.Line($"Got strain {i++}");
                        foreach(var pair in dict)
                        {
                            AppDebug.Line($"\t{pair.Key} = {pair.Value}");
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
    }
}
