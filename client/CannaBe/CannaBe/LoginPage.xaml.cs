﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CannaBe
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if(textBoxSender.Text == ("Enter " + textBoxSender.Name))
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
        }

        private async void PostLogin(object sender, RoutedEventArgs e)
        {
            var req = new LoginRequest(Username.Text, Password.Text);


            HttpResponseMessage res = null;
            try
            {
                res = await HttpManager.Manager.Post("http://ehealth.westeurope.cloudapp.azure.com:8080/login", req);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Status.Text = "Login success!";
                }
                else
                {
                    Status.Text = "Login failed! Status = " + res.StatusCode;
                }
            }
            catch(Exception exc)
            {
                Status.Text = "Exception during login:\n" + exc.Message;
            }

        }
    }
}