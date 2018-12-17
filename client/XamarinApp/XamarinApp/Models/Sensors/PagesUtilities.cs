using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XamarinApp
{
    class PagesUtilities
    {
        private PagesUtilities() { }

        public static void AddBackButtonHandler()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                ((object sender, BackRequestedEventArgs e) =>
                {
                    Frame rootFrame = Window.Current.Content as Frame;

                    if (rootFrame.CanGoBack)
                    {
                        e.Handled = true;
                        rootFrame.GoBack();
                    }
                });
        }
    }

   
}
