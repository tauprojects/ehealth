using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace CannaBe.AppPages.Usage
{
    public sealed partial class UsageHistory : Page
    {
        private UsageData selectedUsage;

        public UsageHistory()
        {
            this.InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;

            do
            {
                if (GlobalContext.CurrentUser == null)
                {
                    AppDebug.Line("GlobalContext.CurrentUser == null");
                    break;
                }

                var usages = Task.Run(() => GetUsagesFromServer()).GetAwaiter().GetResult();
                if(usages != null)
                {
                    GlobalContext.CurrentUser.UsageSessions = usages;
                }
                
                ref var sessions = ref GlobalContext.CurrentUser.UsageSessions;

                if (sessions == null)
                {
                    AppDebug.Line("sessions == null");
                    break;
                }
                if (sessions.Count == 0)
                {
                    UsageListGui.Visibility = Visibility.Collapsed;
                    NoUsageButton.Visibility = Visibility.Visible;
                    AppDebug.Line("sessions.Count == 0");
                    break;
                }

                foreach (var usage in sessions)
                {
                    if(usage == null)
                    {
                        AppDebug.Line("usage == null");
                        continue;
                    }

                    UsageListGui.Items.Add(usage);
                    try
                    {
                        if (usage.usageFeedback == null)
                        {
                            AppDebug.Line("usage.usageFeedback == null");
                        }
                        else
                        {
                            /*foreach (var dic in usage.usageFeedback)
                            {
                                if (dic.Key != null && dic.Value != null)
                                    AppDebug.Line("Question: " + dic.Key + " Answer: " + dic.Value);
                            }*/
                        }
                    }
                    catch(Exception x)
                    {
                        AppDebug.Exception(x, "OnPageLoaded");
                    }
                }
            } while (false);
            progressRing.IsActive = false;

        }

        private List<UsageData> GetUsagesFromServer()
        {
            try
            {
                var res = HttpManager.Manager.Get(Constants.MakeUrl("usage/" + GlobalContext.CurrentUser.Data.UserID));
                var str = res.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<UsageUpdateRequest[]>(str).ToUsageList();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "GetUsagesFromServer");

                return null;
            }
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void UsageSelected(object sender, ItemClickEventArgs e)
        {
            ListView lst = sender as ListView;
            UsageData u = e.ClickedItem as UsageData;
            AppDebug.Line($"Selected usage on [{u.StartTimeString}]");
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            AppDebug.Line($"Remove usage on [{selectedUsage.StartTimeString}]");
            try
            {
                var yesCommand = new UICommand("Remove", cmd =>
                {
                    AppDebug.Line("removing...");
                    GlobalContext.CurrentUser.UsageSessions.Remove(selectedUsage);
                    Frame.Navigate(typeof(UsageHistory));
                });
                var noCommand = new UICommand("Cancel", cmd =>
                {
                    AppDebug.Line("Cancel remove");
                });
                var dialog = new MessageDialog("Are you sure you want to remove the usage from the history?", "Remove Usage")
                {
                    Options = MessageDialogOptions.None
                };
                dialog.Commands.Add(yesCommand);
                dialog.Commands.Add(noCommand);

                await dialog.ShowAsync();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "Remove_Click");
            }

        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                ListView lst = sender as ListView;
                selectedUsage = ((FrameworkElement)e.OriginalSource).DataContext as UsageData;
                if (selectedUsage != null)
                {
                    UsageMenu.ShowAt(lst, e.GetPosition(lst));
                    AppDebug.Line($"Right click usage on [{selectedUsage.StartTimeString}]");
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "ListView_RightTapped");
            }
        }

        private void AddNewUsage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }
    }
}
