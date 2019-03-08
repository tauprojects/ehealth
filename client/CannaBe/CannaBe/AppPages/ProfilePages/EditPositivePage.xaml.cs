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

namespace CannaBe.AppPages.ProfilePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPositivePage : Page
    {
        public EditPositivePage()
        {
            this.InitializeComponent();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

       
        private void BackToEditMedical(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditPositiveEffectsGrid,
                           out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(EditMedicalPage));
        }

        private void GoToEditNegative(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditPositiveEffectsGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(EditNegativePage));
        }
    }
}
