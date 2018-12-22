using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CannaBe
{
    public sealed class HttpManager
    {
        private static HttpManager instance = null;
        private static HttpClient client = null;

       HttpManager()
        {
        }

        public static HttpManager Manager
        {
            get
            {
                if (instance == null)
                {
                    instance = new HttpManager();
                    client = new HttpClient();
                }

                return instance;
            }
        }

        public async void Post(string URL, HttpContent content)
        {
            AppDebug.Line("In Post");

            try
            {
                var response = await client.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                AppDebug.Line("response from post: [" + responseString + "]");
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "Post");
            }

        }
    }
}
