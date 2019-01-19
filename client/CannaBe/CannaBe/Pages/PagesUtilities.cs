using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CannaBe
{
    static class PagesUtilities
    {

        public static void FixPageSize(this Page p)
        {
            p.Height = 569;
            p.Width = 341;
            Size s = new Size(341, 569);
            ApplicationView.GetForCurrentView().SetPreferredMinSize(s);
            ApplicationView.GetForCurrentView().TryResizeView(s);

        }

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

        public static void GetAllCheckBoxesTags(Grid gridWithCheckBoxes, out List<int> listToAddTo)
        {
            var pageName = gridWithCheckBoxes.Parent.GetType().Name;
            listToAddTo = new List<int>();

            foreach (var ctrl in gridWithCheckBoxes.Children)
            {
                if (ctrl is CheckBox)
                {
                    var chk = ctrl as CheckBox;

                    if (chk.IsChecked == true)
                    {
                        System.Int32.TryParse(chk.Tag.ToString(), out int tag);
                        //if (!listToAddTo.Contains(tag))
                        //{
                            listToAddTo.Add(tag);
                            //MedicalEnum m = (MedicalEnum)tag;
                            //var info = MedicalEnum.SEIZURES.GetAttribute<EnumDescriptions>();
                            //AppDebug.Line(pageName + "." + info.q1);
                            AppDebug.Line(pageName + "." + tag);
                        //}
                    }
                }
            }
        }

        public static void SetAllCheckBoxesTags(Grid gridWithCheckBoxes, List<int> listOfTags)
        {
            if (listOfTags == null)
                return;

            if (listOfTags.Count == 0)
                return;

            try
            {
                foreach (var ctrl in gridWithCheckBoxes.Children)
                {
                    if (ctrl is CheckBox)
                    {
                        var chk = ctrl as CheckBox;

                        if (listOfTags.Contains(System.Int32.Parse(chk.Tag as string)))
                        {
                            chk.IsChecked = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "SetAllCheckBoxesTags");
            }
        }


        public static void SleepSeconds(double seconds)
        {
            System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(seconds)).GetAwaiter().GetResult();
        }


    }

   
}
