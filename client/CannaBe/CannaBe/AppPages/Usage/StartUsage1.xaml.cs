using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage : Page
    {
        Dictionary<string,string> strains = null;

        List<string> strainNames = null;
        private static readonly string NoResult = "No Result";
        string StrainChosen = null;

        public StartUsage()
        {
            this.InitializeComponent();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);

            if(UsageContext.ChosenStrain != null) //went back to this page
            {
                StrainList.Text = UsageContext.ChosenStrain.Name;
            }
        }

        private async void LoadStrainList(object sender, object e)
        {
            if(strainNames == null)
            {
                progressRing.IsActive = true;
                await Task.Run(() =>
                {
                    AppDebug.Line("Loading strain list..");
                    try
                    {
                        strains = File.ReadAllLines("Assets/strains.txt")
                                                .Select(a => a.Split('='))
                                                .ToDictionary(x => x[0].Replace('_', ' '),
                                                                x => x[1]);

                        strainNames = strains.Keys.ToList();

                        AppDebug.Line($"loaded {strainNames.Count} strains");

                    }
                    catch (Exception exc)
                    {
                        AppDebug.Exception(exc, "LoadStrainList");
                    }
                });
                progressRing.IsActive = false;
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
                    lst.Add(NoResult);
                }

                sender.ItemsSource = lst;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var item = args.SelectedItem.ToString();

            if (item != NoResult)
            {
                AppDebug.Line($"AutoSuggestBox_SuggestionChosen [{item}]");

                sender.Text = item;
            }
            else
            {
                sender.Text = "";
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var item = args.ChosenSuggestion;

            if (item!=null)
            {
                var itemString = item.ToString();

                if (itemString != NoResult)
                {
                    ErrorNoStrainChosen.Visibility = Visibility.Collapsed;

                    AppDebug.Line($"AutoSuggestBox_QuerySubmitted [{item}]");

                    sender.Text = itemString;
                    StrainChosen = itemString;
                }
                else
                {
                    sender.Text = "";
                }
            }
        }

        private string GetPropertiesString()
        {
            StringBuilder b = new StringBuilder();
            int i = 1;

            if (UsageContext.ChosenStrain?.MedicalNeeds.Count > 0)
            {
                b.AppendLine("- Medical Needs:");
                foreach (string mn in UsageContext.ChosenStrain.MedicalNeeds)
                {
                    b.AppendLine($"\t{i++}. {mn}");
                }
            }
            else
            {
                b.AppendLine("- No medical needs listed.");
            }

            if (UsageContext.ChosenStrain?.PositivePreferences.Count > 0)
            {
                b.AppendLine("- Positive Effects:");
                i = 1;
                foreach (string mn in UsageContext.ChosenStrain.PositivePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn}");
                }
            }
            else
            {
                b.AppendLine("- No positive effects listed.");
            }

            if (UsageContext.ChosenStrain?.NegativePreferences.Count > 0)
            {
                b.AppendLine("- Negative Effects:");
                i = 1;
                foreach (string mn in UsageContext.ChosenStrain.NegativePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn}");
                }
            }
            else
            {
                b.AppendLine("- No negative effects listed.");
            }

            return b.ToString().Substring(0,b.Length - 2);
        }

        private async void SubmitString(object sender, RoutedEventArgs e)
        {
            if(StrainChosen != null)
            {
                progressRing.IsActive = true;
                StrainList.IsEnabled = false;
                SubmitButton.IsEnabled = false;

                await SubmitStringTask();

                StrainProperties.Text = GetPropertiesString();
                Title.Opacity = 1;
                Scroller.ChangeView(null, 0, null); //scroll to top
                Scroller.Visibility = Visibility.Visible;
                StrainProperties.Visibility = Visibility.Visible;
                StrainList.IsEnabled = true;
                SubmitButton.IsEnabled = true;
                progressRing.IsActive = false;

            }
        }

        private async Task SubmitStringTask()
        {
            await Task.Run(async () =>
            {
                AppDebug.Line("Submit string: " + StrainChosen);
                try
                {
                    if (!strains.ContainsKey(StrainChosen))
                    {
                        AppDebug.Line($"Strain '{StrainChosen}' not found in list");
                    }
                    else
                    {
                        var strainId = strains[StrainChosen];
                        AppDebug.Line($"Strain: [{StrainChosen}], ID: {strainId}");

                        var res = HttpManager.Manager.Get("http://strainapi.evanbusse.com/M076LdW/strains/data/effects/" + strainId);

                        if (res == null)
                            return;

                        var str = await res.Result.Content.ReadAsStringAsync();
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(str);

                        UsageContext.ChosenStrain = new Strain(StrainChosen, int.Parse(strainId))
                        {
                            MedicalNeeds = values["medical"].ToList(),
                            PositivePreferences = values["positive"].ToList(),
                            NegativePreferences = values["negative"].ToList()
                        };


                        AppDebug.Line("Finished submit");
                    }
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "SubmitString");
                    progressRing.IsActive = false;

                }
            });
        }

        private void ContinueHandler(object sender, RoutedEventArgs e)
        {
            if(UsageContext.ChosenStrain != null)
            {
                Frame.Navigate(typeof(StartUsage2));
            }
            else
            {
                ErrorNoStrainChosen.Visibility = Visibility.Visible;
            }
        }
    }
}
