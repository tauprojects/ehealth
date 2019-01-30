using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                EnumDescriptions positive = new EnumDescriptions("Do think the session was positive?", "Rate the quality of the treatment:");
                PostQuestions.Items.Add(positive);
                questionDictionary[positive.q1] = "Don't know";
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

        private int getMedicalRank(Dictionary<string, string> questionDictionary)
        {
            int sum = 0;
            int cnt = questionDictionary.Count;
            foreach (KeyValuePair<string, string> question in questionDictionary)
            {
                if (question.Value.Equals("Yes"))
                {
                    sum += 10;
                }
                else if (question.Value.Equals("No"))
                {
                    sum += 0;
                }
                else if (question.Value.Equals("Don't know"))
                {
                    sum += 5;
                }
                else
                {
                    sum += (10 - System.Convert.ToInt32(question.Value));
                }
            }
            AppDebug.Line(sum/cnt);
            return (sum/cnt);
        }

        private async void SubmitFeedback(object sender, TappedRoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            UsageUpdateRequest req;

            int medicalRank = getMedicalRank(questionDictionary);

            try
            {
                progressRing.IsActive = true;
                GlobalContext.CurrentUser.UsageSessions.LastOrDefault().usageFeedback = questionDictionary;

                UsageData use = GlobalContext.CurrentUser.UsageSessions.LastOrDefault();
                string id = GlobalContext.CurrentUser.Data.UserID;

                req = new UsageUpdateRequest(use.UsageStrain.Name, id, medicalRank, 3, 4, 80, 80, 80);

                res = await HttpManager.Manager.Post(Constants.MakeUrl("usage/"), req);

                if (res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Status.Text = "Usage update Successful!";
                        PagesUtilities.SleepSeconds(1);
                        Frame.Navigate(typeof(DashboardPage));
                    }
                    else
                    {
                        Status.Text = "Usage update failed! Status = " + res.StatusCode;
                    }
                }
                else
                {
                    Status.Text = "Usage update failed!\nPost operation failed";
                }

                Frame.Navigate(typeof(DashboardPage));
            }
            catch(Exception x)
            {
                AppDebug.Exception(x, "UsageUpdate");
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
