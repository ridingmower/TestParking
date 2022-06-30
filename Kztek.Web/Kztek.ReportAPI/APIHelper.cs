using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.ReportAPI
{
    public class API : IDisposable
    {
        private readonly HttpClient hClient;

        public API()
        {
            hClient = new HttpClient();
        }

        public string BuildParameters(IDictionary<string, string> pars)
        {
            string result = "";

            if (pars.Count > 0) result += "?";

            var listKey = pars.Keys.ToList();

            for (int i = 0; i < listKey.Count; i++)
            {
                result += $"{HttpUtility.UrlEncode(listKey[i])}={HttpUtility.UrlEncode(pars[listKey[i]])}";

                if (i < listKey.Count - 1) result += "&";
            }

            return result;
        }

        #region Main Method
        public HttpResponseMessage Post(string url, IDictionary<string, string> data)
        {
            var content = new FormUrlEncodedContent(data);

            return hClient.PostAsync(url, content).Result;
        }

        public HttpResponseMessage Get(string url)
        {
            return hClient.GetAsync(url).Result;
        }
        #endregion

        #region Json Method
        public HttpResponseMessage Post_Json<T>(string url, T data)
        {
            var _data = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            return hClient.PostAsync(url, _data).Result;
        }

        public HttpResponseMessage Get_Json(string url)
        {
            return hClient.GetAsync(url).Result;
        }

        public T ResponseToObj<T>(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (hClient != null)
                    {
                        hClient.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~API() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
