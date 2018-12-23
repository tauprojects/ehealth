using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System;

namespace CannaBe.AppPages
{
    public sealed partial class StrainInformationPage : Page
    {
        private readonly static string STRAIN_EXAMPLE = "e.g. 'Alaska'";

        public StrainInformationPage()
        {
            this.InitializeComponent();

            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == STRAIN_EXAMPLE)
            {
                textBoxSender.Text = "";
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
            }
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text.Length == 0)
            {
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
                textBoxSender.Text = STRAIN_EXAMPLE;

            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToInformation(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InformationPage));
        }

        private async void SearchStrain(object sender, TappedRoutedEventArgs e)
        {
            var url = Constants.MakeUrl("ehealth/strain/" + StrainName.Text);
            try
            {
                var res = HttpManager.Manager.Get(url);

                var str = await res.Result.Content.ReadAsStringAsync();

                AppDebug.Line(str);
                await new MessageDialog(str, "Search Strain").ShowAsync();
            }
            catch (Exception ex)
            {
                AppDebug.Exception(ex, "SearchStrain");
                await new MessageDialog("Failed get: \n"+ url, "Exception in Search Strain").ShowAsync();
            }
        }
    }
}
