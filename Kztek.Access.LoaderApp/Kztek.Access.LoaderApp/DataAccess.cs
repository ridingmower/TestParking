using Kztek.Access.LoaderApp.Model;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Kztek.Access.LoaderApp
{
    public class DataAccess
    {
        public string BaseURL = "";

        public DataAccess()
        {
            var host = FunctionHelper.ReadConfig("IPAddress");

            var port = FunctionHelper.ReadConfig("Port");

            BaseURL = $"http://{host}:{port}/api/loaderapp/";
        }

        public User CheckLogin(User user)
        {
            var response = FunctionHelper.Post_Json(BaseURL + "checklogin", user);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);

                return result;
            }
            else
            {
                return null;
            }
        }

        public List<AccessControllerAPI> ListController(string key = "", string computers = "")
        {
            return FunctionHelper.Get_Json_Object<List<AccessControllerAPI>>(BaseURL + $"listcontroller?key={key}&computers={computers}");
        }

        public List<SelfHostConfig> ListSelfHost(List<string> lstLineID)
        {
            var response = FunctionHelper.Post_Json(BaseURL + "listselfhost", lstLineID);

            var result = JsonConvert.DeserializeObject<List<SelfHostConfig>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public SelectListModelUpload GetSelectList()
        {
            var obj = FunctionHelper.Get_Json_Object<SelectListModelUpload>(BaseURL + "getselectlist");

            return obj;
        }

        public List<tblCardExtend> GetCardList(string key, string cardgroups, string customergroupid, string accesslevelids)
        {
            var obj = FunctionHelper.Get_Json_Object<List<tblCardExtend>>(BaseURL + $"getcardlist?key={key}&cardgroups={cardgroups}&customergroupid={customergroupid}&accesslevelids={accesslevelids}");

            return obj;
        }

        public List<tblCustomerExtend> GetCustomerList(string key, string customergroupid, string accesslevelids)
        {
            var obj = FunctionHelper.Get_Json_Object<List<tblCustomerExtend>>(BaseURL + $"getcustomerlist?key={key}&customergroupid={customergroupid}&accesslevelids={accesslevelids}");

            return obj;
        }

        public SelectListModelCardUploadReturn GetListCardWantToUse(CardUploadAPI listUpload)
        {
            var response = FunctionHelper.Post_Json(BaseURL + "getlistcardwanttouse", listUpload);

            var result = JsonConvert.DeserializeObject<SelectListModelCardUploadReturn>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public SelectListModelCardUploadReturn GetListCustomerWantToUse(CardUploadAPI listUpload)
        {
            var response = FunctionHelper.Post_Json(BaseURL + "getlistcustomerwanttouse", listUpload);

            var result = JsonConvert.DeserializeObject<SelectListModelCardUploadReturn>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public ReportResult SendUpload(Employee obj, string SelfHostAddress)
        {
            var response = FunctionHelper.Post_Json($"http://{SelfHostAddress}:8081/api/register/upload", obj);

            var result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

            return new ReportResult() { ControllerID = result[0].ControllerID, Message = result[0].Message, Success = result[0].Success };
        }

        public ReportResult SendDelete(Employee obj, string SelfHostAddress)
        {
            var response = FunctionHelper.Post_Json($"http://{SelfHostAddress}:8081/api/register/delete", obj);

            var result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

            return new ReportResult() { ControllerID = result[0].ControllerID, Message = result[0].Message, Success = result[0].Success };
        }
    }
}
