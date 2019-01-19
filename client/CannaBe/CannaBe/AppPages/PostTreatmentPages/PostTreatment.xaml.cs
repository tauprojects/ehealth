using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.PostTreatmentPages
{
    public sealed partial class PostTreatment : Page
    {
        public PostTreatment()
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

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {

            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
            AppDebug.Line(GlobalContext.CurrentUser.Data.MedicalNeeds.ToString());
            try
            {
                List<string> medicalList = new List<string>();
                foreach (MedicalEnum m in GlobalContext.CurrentUser.Data.MedicalNeeds)
                {
                    var info = m.GetAttribute<EnumDescriptions>();
                    medicalList.Add(info.q1);
                }
                question1.Text = medicalList[0];
                question2.Text = medicalList[1];
            }

            catch (Exception x)
            {
                AppDebug.Exception(x, "Failed receiving medical needs");
            }

        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void SubmitFeedback(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
