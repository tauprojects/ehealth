using System.Net.Http;
using Windows.UI.Xaml.Controls;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage : Page
    {
        public StartUsage()
        {
            this.InitializeComponent();
        }

        private void LoadStrainList(object sender, object e)
        {
            var url = Constants.MakeUrl("ehealth/strain/all");
            //try
            //{
            //    var res = HttpManager.Manager.Get(url);

            //    var str = await res.Result.Content.ReadAsStringAsync();

            //    AppDebug.Line(str);
            //    await new MessageDialog(str, "Search Strain").ShowAsync();
            //}
            //catch (Exception ex)
            //{
            //    AppDebug.Exception(ex, "SearchStrain");
            //    await new MessageDialog("Failed get: \n" + url, "Exception in Search Strain").ShowAsync();
            //}
        }
    }
}
