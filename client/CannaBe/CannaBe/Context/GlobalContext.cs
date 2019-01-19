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


    }
}
