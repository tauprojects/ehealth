using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    static class GlobalContext
    {
        public static UserData User { get; set; } = null;
        public static RegisterRequest RegisterContext { get; set; } = null;
        public static BandContext Band { get; set; } = null;

        public static void AddUserToContext(NavigationEventArgs e)
        {
            try
            {
                User = new UserData(LoginResponse.CreateFromHttpResponse(e.Parameter));
            }
            catch (System.Exception exc)
            {
                AppDebug.Exception(exc, "AddUserToContext");
                User = null;
            }
        }


    }
}
