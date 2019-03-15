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
                AppDebug.Exception(x, "PostTreatment => OnPageLoaded");
            }
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private int[] GetRanks(Dictionary<string, string> questionDictionary)
        {
            int positiveSum = 0, medicalSum = 0;
            int positiveCnt = 0, medicalCnt = 0;
            int cnt = questionDictionary.Count;
            int is_blacklist = 0;
            int[] ans = new int[4];

            foreach (KeyValuePair<string, string> question in questionDictionary)
            {
                if (question.Key.Equals("Would you use this strain again?") || question.Key.Equals("Rate the quality of the treatment:"))
                {
                    if (question.Value.Equals("Yes")) positiveSum += 10;
                    else if (question.Value.Equals("No"))
                    {
                        positiveSum += 0;
                        is_blacklist = 1;
                    }
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
            ans[3] = is_blacklist;
            return ans;
        }



        private void ContinuePostFeedback(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostTreatment2), questionDictionary);
        }

        private void Answers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var check = sender as ComboBox;
            questionDictionary[check.Tag.ToString()] = check.SelectedValue.ToString();
            //AppDebug.Line("Question: " + check.Tag.ToString() + " Answer: " + check.SelectedValue.ToString());
        }
    }
}
