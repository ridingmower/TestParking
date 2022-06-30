using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Helpers
{
    public class ApiService<T> where T : class
    {
        public static List<T> GetList(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<List<T>>(response.Result);
            }
        }

        public static T GetObj(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<T>(response.Result);
            }
        }

        //public static T PutObj(string uri)
        //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        Task<String> response = httpClient.PutAsJsonAsync(uri);
        //        return JsonConvert.DeserializeObjectAsync<T>(response.Result).Result;
        //    }
        //}

        public static Kztek.Model.CustomModel.MessageReport PostObjReturnObj(string uri, T obj)
        {
            var client = new HttpClient();

            var content = JsonConvert.SerializeObject(obj);

            var data = new StringContent(content, Encoding.UTF8, "application/json");

            var response = client.PostAsync(uri, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Kztek.Model.CustomModel.MessageReport>(response.Content.ReadAsStringAsync().Result);
            }

            return null;
        }
    }
}
