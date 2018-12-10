using System;
using System.Collections.Generic;
using App2.Models;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band;

namespace Band
{
    public class BandModel : ViewModel
    {
        static IBandInfo _selectedBand;

        public static IBandInfo SelectedBand
        {
            get { return BandModel._selectedBand; }
            set { BandModel._selectedBand = value; }
        }

        private static IBandClient _bandClient;
        public static IBandClient BandClient
        {
            get { return _bandClient; }
            set
            {
                _bandClient = value;
            }
        }


        public static bool IsConnected
        {
            get
            {
                return BandClient != null;
            }

        }

        public static async Task FindDevicesAsync()
        {
            AppDebug.line("In FindDevicesAsync");
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if (bands != null && bands.Length > 0)
            {
                AppDebug.line("Found Band");
                SelectedBand = bands[0]; // take the first band

            }
            else
            {
                AppDebug.line("Did not find Band");

            }
        }

        public static async Task InitAsync()
        {
            AppDebug.line("In InitAsync");
            try
            {
                if (IsConnected)
                    return;

                await FindDevicesAsync();
                if (SelectedBand != null)
                {
                    AppDebug.line("connecting to band");

                    BandClient = await BandClientManager.Instance.ConnectAsync(SelectedBand);

                    AppDebug.line("connected to band");

                }
            }
            catch (Exception x)
            {
                AppDebug.line("Exception in InitAsync: \n" + x.Message + "\nAt:\n" + x.StackTrace);
            }
        }
    }
}
