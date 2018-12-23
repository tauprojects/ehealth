﻿using CannaBe.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CannaBe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterMedicalPage : Page
    {
        RegisterRequest registerRequest;
        public RegisterMedicalPage()
        {
            this.InitializeComponent();
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
            //PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            registerRequest = (RegisterRequest)e.Parameter;
            AppDebug.Line(registerRequest.Username);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackToRegister(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void ContinuePositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPositiveEffectsPage), registerRequest);
        }

    }
}
