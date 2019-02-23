using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    static class GlobalContext
    {
        public static UserData CurrentUser { get; set; } = null;
        public static RegisterRequest RegisterContext { get; set; } = null;
        public static BandContext Band { get; set; } = null;

        public static void AddUserToContext(NavigationEventArgs e)
        {
            try
            {
                CurrentUser = new UserData(LoginResponse.CreateFromHttpResponse(e.Parameter));
            }
            catch (System.Exception exc)
            {
                AppDebug.Exception(exc, "AddUserToContext");
                CurrentUser = null;
            }
        }

        public static void UpdateUsagesContextIfEmptyAsync()
        {
            if (CurrentUser.UsageSessions.Count == 0)
            {
                var usages = Task.Run(() => GetUsagesFromServer()).GetAwaiter().GetResult();
                if (usages != null)
                {
                    CurrentUser.UsageSessions = usages;
                }
            }
        }

        private static List<UsageData> GetUsagesFromServer()
        {
            try
            {
                var res = HttpManager.Manager.Get(Constants.MakeUrl("usage/" + GlobalContext.CurrentUser.Data.UserID));
                var str = res.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<UsageUpdateRequest[]>(str).ToUsageList();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "GetUsagesFromServer");

                return null;
            }
        }
    }
}
