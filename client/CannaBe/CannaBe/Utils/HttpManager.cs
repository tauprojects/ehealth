using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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

        public static HttpContent CreateJson(object obj)
        {
            //from: https://stackoverflow.com/questions/23585919/send-json-via-post-in-c-sharp-and-receive-the-json-returned

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(obj);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            return httpContent;
        }

        public async Task<HttpResponseMessage> Post(string URL, HttpContent content)
        {
            AppDebug.Line("In Post");

            try
            {
                var response = await client.PostAsync(URL, content);
                
                var responseString = await response.Content.ReadAsStringAsync();

                AppDebug.Line("response from post: [" + responseString + "]");

                return response;
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "Post");
                return null;
            }

        }
    }
}
