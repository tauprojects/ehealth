using System.Net.Http;

namespace CannaBe
{
    abstract class IJsonResponse
    {
        public static IJsonResponse CreateFromHttpResponse(HttpResponseMessage msg)
        {
            return HttpManager.ParseJson<IJsonResponse>(msg);
        }
    }
}
