using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterMedicalPage : Page
    {
        public RegisterMedicalPage()
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
                    PagesUtilities.SetAllCheckBoxesTags(RegisterMedicalGrid,
                                     req.IntMedicalNeeds);
                }
                catch(Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterMedicalPage.OnNavigatedTo");
                }
            }
        }

        private void BackToRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                     out List<int> intList);

            GlobalContext.RegisterContext.IntMedicalNeeds = intList;

            Frame.Navigate(typeof(RegisterPage));
        }

        private void ContinuePositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntMedicalNeeds = intList;
            GlobalContext.RegisterContext.MedicalNeeds = MedicalEnumMethods.FromIntToStringList(intList);

            Frame.Navigate(typeof(RegisterPositiveEffectsPage), GlobalContext.RegisterContext);
        }
    }
}
