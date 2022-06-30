using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using FastMember;
using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.Models;
using Newtonsoft.Json;

namespace Kztek.Access.LoaderApp
{
    public static class FunctionHelper
    {
        public static User CurrentUser;

        public static string ReadConfig(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                return null;
            }
        }

        public static HttpResponseMessage Post_Json<T>(string url, T data)
        {
            using (HttpClient hClient = new HttpClient())
            {
                try
                {
                    var _data = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                    return hClient.PostAsync(url, _data).Result;
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable) { Content = new StringContent("Không thể kết nối đến máy chủ") };
                }

            }
        }

        public static HttpResponseMessage Get_Json(string url)
        {
            using (HttpClient hClient = new HttpClient())
            {
                try
                {
                    return hClient.GetAsync(url).Result;
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable) { Content = new StringContent("Không thể kết nối đến máy chủ") };
                }

            }
        }

        public static T Get_Json_Object<T>(string url)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Get_Json(url).Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return default(T);
            }

        }

        public static DataTable ToDataTable<T>(this List<T> input)
        {
            DataTable result = new DataTable();

            using (var reader = ObjectReader.Create(input))
            {
                result.Load(reader);
            }

            return result;
        }

        public static List<string> GetListSelfHostId(this List<SelfHostConfig> lstSelfhost)
        {
            List<string> result = new List<string>();

            foreach (var host in lstSelfhost)
            {
                result.Add(host.Id);
            }

            return result;
        }

        public static List<tblAccessController> ToListController(this List<AccessControllerAPI> lstController)
        {
            List<tblAccessController> result = new List<tblAccessController>();

            foreach (var controller in lstController)
            {
                result.Add(new tblAccessController() { ControllerID = new Guid(controller.ControllerID) });
            }

            return result;
        }

        public static void SetSelectList(this ComboBox cb, DataTable source, string displayMember, string valueMember)
        {
            List<object> datasource = new List<object>();

            datasource.Add(new { Display = "Tất cả", Value = string.Empty });

            foreach (DataRow row in source.Rows)
            {
                datasource.Add(new { Display = row[displayMember].ToString(), Value = row[valueMember].ToString() });
            }

            cb.DataSource = datasource;
            cb.DisplayMember = "Display";
            cb.ValueMember = "Value";
        }
    }
}
