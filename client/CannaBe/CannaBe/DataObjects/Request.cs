using System.Net.Http;

namespace CannaBe
{
    abstract class Request
    {
        public static implicit operator HttpContent(Request req)
        {
            return HttpManager.CreateJson(req);
        }
    }
}
