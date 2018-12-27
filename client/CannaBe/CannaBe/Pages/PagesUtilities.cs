using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CannaBe
{
    static class PagesUtilities
    {
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
        private static ScrollViewer GetRootScrollViewer(object sender)
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
            GetRootScrollViewer(sender).Focus(FocusState.Programmatic);
        }

        public static void GetAllCheckBoxesTags(Grid gridWithCheckBoxes, List<int> listToAddTo)
        {
            var pageName = gridWithCheckBoxes.Parent.GetType().Name;

            foreach (var ctrl in gridWithCheckBoxes.Children)
            {
                if (ctrl is CheckBox)
                {
                    var chk = ctrl as CheckBox;

                    if (chk.IsChecked == true)
                    {
                        System.Int32.TryParse(chk.Tag.ToString(), out int tag);
                        if (!listToAddTo.Contains(tag))
                        {
                            listToAddTo.Add(tag);
                            AppDebug.Line(pageName + "." + tag);
                        }
                    }
                }
            }
        }
    }

   
}
