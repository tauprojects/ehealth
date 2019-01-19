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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(RegisterMedicalGrid,
                                     req.IntListMedicalNeeds);
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

            GlobalContext.RegisterContext.IntListMedicalNeeds = intList;

            Frame.Navigate(typeof(RegisterPage));
        }

        private void ContinuePositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntListMedicalNeeds = intList;

            Frame.Navigate(typeof(RegisterPositiveEffectsPage), GlobalContext.RegisterContext);
        }
    }
}
