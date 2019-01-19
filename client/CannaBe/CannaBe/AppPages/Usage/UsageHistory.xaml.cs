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
            /*
            var yesCommand = new UICommand("Yes", cmd => { });
            var noCommand = new UICommand("No", cmd => { });
            var cancelCommand = new UICommand("Cancel", cmd => { });
            var dialog = new MessageDialog(content, title);
            dialog.Options = MessageDialogOptions.None;
            dialog.Commands.Add(yesCommand);
            */
            AppDebug.Line($"Remove usage on [{selectedUsage.StartTimeString}]");
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
