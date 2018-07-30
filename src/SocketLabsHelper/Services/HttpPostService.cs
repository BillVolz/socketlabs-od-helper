using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SocketLabsHelper.Services
{
    public class HttpPostService : IHttpPostService
    {
        public T Get<T>(string url, string header)
        {
            return Deserialize<T>(GetResponseAsString(url, header));
        }

        public dynamic Get(string url, string header)
        {
            return JObject.Parse(GetResponseAsString(url, header));
        }

        public string GetResponseAsString(string url, string header)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "GET";

            if (!string.IsNullOrEmpty(header))
            {
                httpWebRequest.Headers.Add(header);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public string Post(object postdata, string url)
        {
            var json = Serialize(postdata);
            return Post(json, url);
        }

        public T PostAndGetResponse<T>(object payload, string url, string header)
        {
            var json = Serialize(payload);
            var response = Post(json, url, header);
            return Deserialize<T>(response);
        }


        #region Helpers

        internal virtual string Serialize<TPayLoad>(TPayLoad payLoad)
        {
            return JsonConvert.SerializeObject(payLoad);
        }

        internal virtual T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        internal virtual string Post(string json, string url)
        {
            return Post(json, url, string.Empty);
        }

        internal virtual string Post(string json, string url, string header)
        {
            var httpWebRequest = WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";

            if (!string.IsNullOrEmpty(header))
            {
                httpWebRequest.Headers.Add(header);
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        #endregion
    }
}
