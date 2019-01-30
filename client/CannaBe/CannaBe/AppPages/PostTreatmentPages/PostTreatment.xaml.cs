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

        private int[] getRanks(Dictionary<string, string> questionDictionary)
        {
            int positiveSum = 0, medicalSum = 0;
            int positiveCnt = 0, medicalCnt = 0;
            int cnt = questionDictionary.Count;
            int[] ans = new int[3];

            foreach (KeyValuePair<string, string> question in questionDictionary)
            {
                if (question.Key.Equals("Do think the session was positive?") || question.Key.Equals("Rate the quality of the treatment:"))
                {
                    if (question.Value.Equals("Yes")) positiveSum += 10;
                    else if (question.Value.Equals("No")) positiveSum += 0;
                    else if (question.Value.Equals("Don't know")) positiveSum += 5;
                    else positiveSum += System.Convert.ToInt32(question.Value);
                    positiveCnt += 1;
                }
                else { 
                    if (question.Value.Equals("Yes")) medicalSum += 10;
                    else if (question.Value.Equals("No")) medicalSum += 0;
                    else if (question.Value.Equals("Don't know")) medicalSum += 5;
                    else medicalSum += (10 - System.Convert.ToInt32(question.Value));
                    medicalCnt += 1;

                }
            }
            ans[0] = medicalSum / medicalCnt;
            ans[1] = positiveSum / positiveCnt;
            ans[2] = ((positiveSum + medicalSum) / cnt);
            return ans;
        }



        private async void SubmitFeedback(object sender, TappedRoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            UsageUpdateRequest req;

            int[] ranks = new int[3];

            ranks = getRanks(questionDictionary);

            try
            {
                progressRing.IsActive = true;
                GlobalContext.CurrentUser.UsageSessions.LastOrDefault().usageFeedback = questionDictionary;

                UsageData use = GlobalContext.CurrentUser.UsageSessions.LastOrDefault();
                string id = GlobalContext.CurrentUser.Data.UserID;

                req = new UsageUpdateRequest(use.UsageStrain.Name, id, ranks[0], ranks[1], ranks[2], use.HeartRateMax, use.HeartRateMin, (int)use.HeartRateAverage);

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
