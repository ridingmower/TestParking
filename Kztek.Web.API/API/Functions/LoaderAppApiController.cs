using Kztek.Service.Admin;
using Kztek.Web.API.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Kztek.Web.Core.Functions;
using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.CustomModel;
using Kztek.Web.Core.Extensions;
using Kztek.Model.Models;
using Kztek.Security;
using System.Configuration;
using Kztek.Web.Core.Helpers;

namespace Kztek.Web.API.API.Functions
{
    [RoutePrefix("api/loaderapp")]
    public class LoaderAppApiController : ApiBaseController
    {
        ItblAccessControllerService _tblAccessControllerService;
        ItblAccessLineService _tblAccessLineService;
        ISelfHostConfigService _SelfHostConfigService;
        ItblAccessPCService _tblAccessPCService;
        ItblCardGroupService _tblCardGroupService;
        ItblAccessLevelService _tblAccessLevelService;
        ItblCustomerGroupService _tblCustomerGroupService;
        ItblCardService _tblCardService;
        IUserService _UserService;
        ItblCustomerService _tblCustomerService;

        public LoaderAppApiController(ItblAccessControllerService _tblAccessControllerService, ItblAccessLineService _tblAccessLineService, ISelfHostConfigService _SelfHostConfigService, ItblAccessPCService _tblAccessPCService, ItblCardGroupService _tblCardGroupService, ItblAccessLevelService _tblAccessLevelService, ItblCustomerGroupService _tblCustomerGroupService, ItblCardService _tblCardService, IUserService _UserService, ItblCustomerService _tblCustomerService)
        {
            this._tblAccessControllerService = _tblAccessControllerService;
            this._tblAccessLineService = _tblAccessLineService;
            this._SelfHostConfigService = _SelfHostConfigService;
            this._tblAccessPCService = _tblAccessPCService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblAccessLevelService = _tblAccessLevelService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCardService = _tblCardService;
            this._UserService = _UserService;
            this._tblCustomerService = _tblCustomerService;
        }

        [Route("checklogin")]
        [HttpPost]
        public HttpResponseMessage CheckLogin(User user)
        {
            //if (SecureDongleProvider.CheckHardKey() == false)
            //{
            //    //return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Dongle Not Found"), RequestMessage = Request };
            //}


            //Kiểm tra có phải Superadmin
            if (user.Username == SecurityModel.Username)
            {
                var password = ConfigurationManager.AppSettings["FUTECHSUPPORTPASS"];
                password = CryptoProvider.SimpleDecryptWithPassword(password, SecurityModel.Keypass);

                if (user.Password == password)
                {
                    //Đăng nhập superadmin thành công
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, RequestMessage = Request };
                }
                else
                {
                    //Sai thông tin superadmin
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, RequestMessage = Request };
                }
            }

            //Lấy khách hàng qua email
            var userInfo = _UserService.GetByUserNameOREmail(user.Username);

            if (userInfo == null)
            {
                //User không tồn tại
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, RequestMessage = Request };
            }
            else
            {
                if (userInfo.Active)
                {
                    var password = user.Password.PasswordHashed(userInfo.PasswordSalat);

                    if (!password.Equals(userInfo.Password))
                    {
                        //Sai mật khẩu
                        return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, RequestMessage = Request };
                    }
                    else
                    {
                        //Đăng nhập thành công
                        var content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");

                        return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
                    }
                }
                else
                {
                    //Tài khoản chưa kích hoạt
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, RequestMessage = Request };
                }
            }
        }

        [Route("listcontroller")]
        [HttpGet]
        public HttpResponseMessage ListController(string key, string computers)
        {
            List<AccessControllerAPI> lstResult = new List<AccessControllerAPI>();

            var list = _tblAccessControllerService.GetAllByFirst(key, computers, "").ToList();

            foreach (var ac in list)
            {
                var url = "";

                var t = _tblAccessLineService.GetById(Guid.Parse(ac.LineID));
                if (t != null)
                {
                    var k = _SelfHostConfigService.GetByPCID(t.PCID);
                    if (k != null)
                    {
                        url = k.Address;
                    }
                }

                var status = FunctionHelper.CheckConnectControllerDesktop(url, ac.ControllerID.ToString());

                lstResult.Add(new AccessControllerAPI { ControllerID = ac.ControllerID.ToString(), ControllerName = ac.ControllerName, Status = status, LineID = ac.LineID });
            }

            var content = new StringContent(JsonConvert.SerializeObject(lstResult), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("listselfhost")]
        [HttpPost]
        public HttpResponseMessage GetListSelfHost(List<string> listID)
        {
            var lstHost = _SelfHostConfigService.GetAllActiveByListLineId(listID).Distinct().ToList();

            var content = new StringContent(JsonConvert.SerializeObject(lstHost), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("getselectlist")]
        [HttpGet]
        public HttpResponseMessage GetSelectList()
        {
            var model = new SelectListModelUpload()
            {
                dtComputer = _tblAccessPCService.GetAllActive().ToDataTableNullable(),
                dtCardGroup = _tblCardGroupService.GetAllActive().ToDataTableNullable(),
                dtCustomerGroup = GetMenuList().ToDataTableNullable(),
                dtAccessLevel = _tblAccessLevelService.GetAllActive().ToDataTableNullable()
            };

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("getcardlist")]
        [HttpGet]
        public HttpResponseMessage GetCardList(string key, string cardgroups, string customergroupid, string accesslevelids)
        {
            var customergroups = GetListChild("", customergroupid);

            var list = _tblCardService.GetAllByFirstForUpload(key, "", cardgroups, customergroups, "", "", accesslevelids);

            var content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("getcustomerlist")]
        [HttpGet]
        public HttpResponseMessage GetCustomerList(string key, string customergroupid, string accesslevelids)
        {
            var customergroups = GetListChild("", customergroupid);

            var list = _tblCustomerService.GetAllByFirstForUpload(key, "", customergroups, "", "", accesslevelids);

            var content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("getlistcardwanttouse")]
        [HttpPost]
        public HttpResponseMessage GetListCardWantToUse(CardUploadAPI listUpload)
        {
            var user = listUpload.CurrentUser;

            var model = new SelectListModelCardUploadReturn();

            model.ListSelfHost = listUpload.ListSelfHost;

            model.ListController = listUpload.ListController;

            model.ListEmployee = new List<Employee>();

            if (listUpload.ListFilter.isall)
            {
                var customergroups = GetListChild("", (listUpload.ListFilter.customergroupid));

                model.ListCard = _tblCardService.GetAllByFirst(listUpload.ListFilter.key, "", listUpload.ListFilter.cardgroupids, customergroups, "", "", listUpload.ListFilter.accesslevelids);
            }
            else
            {
                model.ListCard = _tblCardService.GetAllActiveByListIdForUpload(listUpload.ListCardId).ToList();
            }

            if (model.ListCard.Any())
            {
                foreach (var itemCard in model.ListCard)
                {
                    foreach (var itemController in model.ListController)
                    {
                        var map = new Employee();

                        map.CardNumber = itemCard.CardNumber.Trim();

                        map.AccessLevelID = itemCard.AccessLevelID.Trim();

                        map.ControllerIDs = itemController.ControllerID.ToString();

                        map.UserID = user.Id;

                        map.UserIDofFinger = 0;

                        if (listUpload.ListFilter.isusenewdate)
                        {
                            map.ExpireDate = Convert.ToDateTime(listUpload.ListFilter.newdateexpire).ToString("yyyyMMdd").Trim();
                        }
                        else
                        {
                            map.ExpireDate = Convert.ToDateTime(itemCard.AccessExpireDate).ToString("yyyyMMdd").Trim();
                        }

                        model.ListEmployee.Add(map);
                    }
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        [Route("getlistcustomerwanttouse")]
        [HttpPost]
        public HttpResponseMessage GetListCustomerWantToUse(CardUploadAPI listUpload)
        {
            //Lấy người dùng hiện tại
            var user = listUpload.CurrentUser;

            var model = new SelectListModelCardUploadReturn();
            model.ListSelfHost = listUpload.ListSelfHost;

            model.ListController = listUpload.ListController;

            model.ListEmployee = new List<Employee>();

            model.IsUseNewDate = listUpload.ListFilter.isusenewdate;

            if (listUpload.ListFilter.isall)
            {
                var customergroups = GetListChild("", listUpload.ListFilter.customergroupid);

                model.ListCustomer = _tblCustomerService.GetAllByFirst(listUpload.ListFilter.key, "", customergroups, "", "", listUpload.ListFilter.accesslevelids);
            }
            else
            {
                model.ListCustomer = _tblCustomerService.GetAllActiveByListIdForUpload(listUpload.ListCustomerId).ToList();
            }

            if (model.ListCustomer.Any())
            {
                foreach (var itemCustomer in model.ListCustomer)
                {
                    foreach (var itemController in model.ListController)
                    {
                        var map = new Employee();

                        map.CardNumber = "0";

                        map.AccessLevelID = itemCustomer.AccessLevelID.Trim();

                        map.ControllerIDs = itemController.ControllerID.ToString();

                        map.UserID = user.Id;

                        map.UserIDofFinger = itemCustomer.UserIDofFinger;

                        map.Fingers1 = itemCustomer.Finger1;

                        map.Fingers2 = itemCustomer.Finger2;

                        map.VerifyTypeID = 0;

                        if (listUpload.ListFilter.isusenewdate)
                        {
                            map.ExpireDate = Convert.ToDateTime(listUpload.ListFilter.newdateexpire).ToString("yyyyMMdd").Trim();
                        }
                        else
                        {
                            map.ExpireDate = Convert.ToDateTime(itemCustomer.AccessExpireDate).ToString("yyyyMMdd").Trim();
                        }

                        model.ListEmployee.Add(map);
                    }
                }
            }

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content, RequestMessage = Request };
        }

        /// <summary>
        /// Danh sách menu cấp cha
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <returns></returns>
        private List<SelectListModel> GetMenuList()
        {
            var list = new List<SelectListModel>();
            //{
            //    new SelectListModel {  ItemValue = "", ItemText = "- Chọn danh mục -" }
            //};
            var MenuList = _tblCustomerGroupService.GetAllActive().ToList();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.SortOrder))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new SelectListModel { ItemValue = item1.ItemValue.ToString(), ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Đệ quy để lấy danh sách con
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private List<SelectListModel> Children(string parentID)
        {
            //Khai báo danh sách
            List<SelectListModel> lst = new List<SelectListModel>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _tblCustomerGroupService.GetAllChildByParentID(parentID.ToString()).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new SelectListModel { ItemValue = item1.ItemValue, ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                    }
                }
            }
            return lst;
        }

        private string GetListChild(string str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += id + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.CustomerGroupID.ToString() + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }
    }
}