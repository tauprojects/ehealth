using CannaBe.AppPages.RecomendationPages;
using CannaBe.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage : Page
    {
        Dictionary<string,string> StrainsDict = null;

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
            GetSuggestedRadio.IsChecked = true;

            StrainList.AddHandler(KeyDownEvent, new KeyEventHandler(AutoSuggestBox_KeyDown), true);

            if (UsageContext.ChosenStrain != null) //went back to this page
            {
                StrainChosen = StrainList.Text = UsageContext.ChosenStrain.Name;
            }
        }

        private async void LoadStrainList(object sender, object e)
        {
            if(strainNames == null)
            {
                progressRing.IsActive = true;
                await Task.Run(async () =>
                {
                    AppDebug.Line("Loading strain list..");
                    try
                    {

                        var res = await HttpManager.Manager.Get(Constants.MakeUrl("strains/all/"));

                        if (res == null)
                        {
                            throw new Exception("Get operation failed, response is null");
                        }

                        var strains = HttpManager.ParseJson<Dictionary<string, string>>(res);

                        strainNames = strains.Keys.ToList();
                        strainNames.Sort();

                        AppDebug.Line($"loaded {strainNames.Count} strains");

                    }
                    catch (Exception exc)
                    {
                        AppDebug.Exception(exc, "LoadStrainList");
                        await new MessageDialog($"Strain list loading operation failed. Please try again or use suggested strain option.", "Error").ShowAsync();

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

        private async void SubmitString(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StrainChosen != null)
                {
                    progressRing.IsActive = true;
                    StrainList.IsEnabled = false;
                    SubmitButton.IsEnabled = false;

                    await SubmitStringTask();

                    StrainProperties.Text = UsageContext.ChosenStrain?.GetPropertiesString();
                    Title.Opacity = 1;
                    Scroller.ChangeView(null, 0, null); //scroll to top
                    Scroller.Visibility = Visibility.Visible;
                    StrainProperties.Visibility = Visibility.Visible;
                    StrainList.IsEnabled = true;
                    SubmitButton.IsEnabled = true;
                    progressRing.IsActive = false;

                }
            }
            catch (Exception ex)
            {
                AppDebug.Exception(ex, "SubmitString");
            }
        }

        private async Task SubmitStringTask()
        {
            await Task.Run(async () =>
            {
                AppDebug.Line("Submit string: " + StrainChosen);
                try
                {
                    if (!StrainsDict.ContainsKey(StrainChosen))
                    {
                        AppDebug.Line($"Strain '{StrainChosen}' not found in list");
                    }
                    else
                    {
                        var strainId = StrainsDict[StrainChosen];
                        AppDebug.Line($"Strain: [{StrainChosen}], ID: {strainId}");

                        var res = await HttpManager.Manager.Get("http://strainapi.evanbusse.com/M076LdW/strains/data/effects/" + strainId);

                        if (res == null)
                            return;

                        var values = HttpManager.ParseJson<Dictionary<string, string[]>>(res);

                        UsageContext.ChosenStrain = new Strain(StrainChosen, int.Parse(strainId))
                        {
                            BitmapMedicalNeeds = MedicalEnumMethods.BitmapFromStringList(values["medical"].ToList()),
                            BitmapPositivePreferences = PositivePreferencesEnumMethods.BitmapFromStringList(values["positive"].ToList()),
                            BitmapNegativePreferences = NegativePreferencesEnumMethods.BitmapFromStringList(values["negative"].ToList())
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

        private void SuggestedChoose(object sender, RoutedEventArgs e)
        {
            StrainList.IsEnabled = false;
            SubmitButton.IsEnabled = false;
            ChooseButton.IsEnabled = true;
        }

        private void ListChoose(object sender, RoutedEventArgs e)
        {
            ChooseButton.IsEnabled = false;
            StrainList.IsEnabled = true;
            SubmitButton.IsEnabled = true;
        }

        private void GoTosSuggested(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyRecomendations));
        }

        private void AutoSuggestBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //AppDebug.Line("AutoSuggestBox_KeyDown");
            if (StrainChosen != null)
            {
                //AppDebug.Line($"StrainChosen = {StrainChosen}, StrainList.Text = {StrainList.Text}");
                if (StrainChosen.ToLower().Equals(StrainList.Text.ToLower()))
                {
                    if(e.Key == Windows.System.VirtualKey.Enter)
                    {
                        StrainList.IsSuggestionListOpen = false;
                        PagesUtilities.SleepSeconds(0.2);
                        //AppDebug.Line($"Enter");

                        SubmitString(null, null);
                    }
                }
            }
        }
    }
}
