using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CannaBe
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

        /////////////
        // Source:
        // https://stackoverflow.com/questions/41182664/how-to-not-focus-element-on-application-startup
        ////////////
        private static ScrollViewer getRootScrollViewer(object sender)
        {
            DependencyObject el = sender as DependencyObject;
            while (el != null && !(el is ScrollViewer))
            {
                el = VisualTreeHelper.GetParent(el);
            }

            return (ScrollViewer)el;
        }

        public static void DontFocusOnAnythingOnLoaded(object sender, RoutedEventArgs e)
        {
            getRootScrollViewer(sender).Focus(FocusState.Programmatic);
        }
    }

   
}
