using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterPositiveEffectsPage : Page
    {
        public RegisterPositiveEffectsPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == ("Enter " + textBoxSender.Name))
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
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(RegisterPositiveEffectsGrid,
                                     req.IntPositivePreferences);
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterPositiveEffectsPage.OnNavigatedTo");

                }
            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToMedicalRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterPositiveEffectsGrid,
                                       out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(RegisterMedicalPage));
        }

        private void ContinueNegativeEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterPositiveEffectsGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(RegisterNegativeEffectsPage));
        }
    }
}
