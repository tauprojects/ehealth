using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using Windows.System;
using CannaBe.AppPages.InformationPages;

namespace CannaBe.AppPages
{
    public sealed partial class StrainInformationPage : Page
    {
        private readonly static string STRAIN_EXAMPLE = "e.g. 'Alaska'";

        public StrainInformationPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
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

        private async void SearchStrain(object sender, RoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(MedicalSearchGrid, out List<int> MedicalList);
            PagesUtilities.GetAllCheckBoxesTags(PositiveSearchGrid, out List<int> PositiveList);
            PagesUtilities.GetAllCheckBoxesTags(NegativeSearchGrid, out List<int> NegativeList);

            int MedicalBitMap = StrainToInt.FromIntListToBitmap(MedicalList);
            int PositiveBitMap = StrainToInt.FromIntListToBitmap(PositiveList);
            int NegativeBitMap = StrainToInt.FromIntListToBitmap(NegativeList);

            if ((MedicalList.Count == 0) && (PositiveList.Count == 0) && (NegativeList.Count == 0) && (StrainName.Text == "")) Status.Text = "Invaild Search! Please enter search parameter";
            else Status.Text = "";

            if (StrainName.Text != "")
            {
                var url = Constants.MakeUrl("strain/name/" + StrainName.Text);
                try
                {
                    var res = HttpManager.Manager.Get(url);

                    if (res == null)
                        return;

                    var str = await res.Result.Content.ReadAsStringAsync();
                    Frame.Navigate(typeof(StrainSearchResults), str);
                }
                catch (Exception ex)
                {
                    AppDebug.Exception(ex, "SearchStrain");
                    await new MessageDialog("Failed get: \n" + url, "Exception in Search Strain").ShowAsync();
                }
            }
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SearchStrain(sender, e);
            }
        }
    }
}
