﻿using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.PostTreatmentPages
{
    public sealed partial class PostTreatment : Page
    {
        Dictionary<string, string> questionDictionary = new Dictionary<string, string>();
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
            try
            {
                foreach (var medicalNeed in GlobalContext.CurrentUser.Data.MedicalNeeds)
                {
                    var info = medicalNeed.GetAttribute<EnumDescriptions>();
                    PostQuestions.Items.Add(info);
                    questionDictionary[info.q1] = "Don't know";
                }
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
            try
            {
                GlobalContext.CurrentUser.UsageSessions.LastOrDefault().usageFeedback = questionDictionary;
            }
            catch(Exception x)
            {
                AppDebug.Exception(x, "SubmitFeedback");
            }
        }

        private void Answers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var check = sender as ComboBox;
            questionDictionary[check.Tag.ToString()] = check.SelectedValue.ToString();
            //AppDebug.Line("Question: " + check.Tag.ToString() + " Answer: " + check.SelectedValue.ToString());
        }
    }
}
