using System;
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
            AppDebug.Line("In UsageHistory page");
            foreach(var usage in GlobalContext.CurrentUser.UsageSessions)
            {
                UsageListGui.Items.Add(usage);
                foreach(var dic in usage.usageFeedback)
                {
                    AppDebug.Line("Question: " + dic.Key + " Answer: " + dic.Value);
                }
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

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            AppDebug.Line($"Remove usage on [{selectedUsage.StartTimeString}]");
            try
            {
                var yesCommand = new UICommand("Remove", cmd =>
                {
                    AppDebug.Line("removing...");
                    GlobalContext.CurrentUser.UsageSessions.Remove(selectedUsage);
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

                dialog.ShowAsync().GetAwaiter().GetResult();
            }catch(Exception x)
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
            catch(Exception x)
            {
                AppDebug.Exception(x, "ListView_RightTapped");
            }
        }
    }
}
