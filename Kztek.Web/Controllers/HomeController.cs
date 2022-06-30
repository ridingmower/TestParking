using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kztek.Web.Models;
using System.Configuration;
using Kztek.Data.SqlHelper;
using System.Data;
using Kztek.Security;

namespace Kztek.Web.Controllers
{
    public class HomeController : Controller
    {
        private IMenuFunctionService _MenuFunctionService;
        private IMenuFunctionConfigService _MenuFunctionConfigService;
        private ISystemRecordService _SystemRecordService;
        private ItblSystemConfigService _tblSystemConfigService;

        public HomeController(IMenuFunctionService _MenuFunctionService, IMenuFunctionConfigService _MenuFunctionConfigService, ISystemRecordService _SystemRecordService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._MenuFunctionService = _MenuFunctionService;
            this._MenuFunctionConfigService = _MenuFunctionConfigService;
            this._SystemRecordService = _SystemRecordService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        public ActionResult Index()
        {
            var area = ConfigurationManager.AppSettings["FunctionGroupViewDefault"].ToString();
            return RedirectToAction("Index", "Home", new { Area = area });
        }

        [CheckSessionLogin]
        public ActionResult ViewIndex()
        {
            return View();
        }

        [CheckSessionLogin]
        public ActionResult Header(string controllerName = "", string actionName = "", string group = "")
        {
            //Lấy dữ liệu
            var groupId = ConfigurationManager.AppSettings["FunctionGroupAllow"].ToString();

            //Điều hướng
            string groupname = "";
            string direct = FunctionHelper.DirectActionByGroup(group, ref groupname);

            var list = FunctionHelper.GroupMenuList().Where(n => groupId.Contains(n.ItemValue));

            ViewBag.DirectValue = direct;
            ViewBag.GroupID = group;

            return PartialView(list);
        }

        /// <summary>
        /// Thanh điều hướng
        /// </summary>
        /// <param name="controller">Tên controller</param>
        /// <param name="action">Tên action</param>
        /// <returns></returns>
        [CheckSessionLogin]
        public PartialViewResult Breadcrumb(string controller = "", string action = "", string group = "")
        {
            //Điều hướng
            string groupname = "";
            string direct = FunctionHelper.DirectActionByGroup(group, ref groupname);
            ViewBag.DirectValue = direct;

            controller = FunctionHelper.ConverControllerInGroup(controller);


            //Danh sách menu
            var list = new List<MenuFunction>();
            if (CacheLayer.Exists(ConstField.AllListMenuFunctionCache))
            {
                list = CacheLayer.Get<List<MenuFunction>>(ConstField.AllListMenuFunctionCache);
            }
            else
            {
                list = _MenuFunctionService.GetAllActive().ToList();
                CacheLayer.Add(ConstField.AllListMenuFunctionCache, list, ConstField.TimeCache);
            }

            var listModel = new List<SelectListModel2>();

            var obj = list.FirstOrDefault(n => n.ControllerName.Equals(controller) && n.ActionName.Equals(action));

            var breadcrumb = GetBreadcrumb(obj != null ? obj.ParentId.ToString() : "0", "");

            if (!string.IsNullOrWhiteSpace(breadcrumb))
            {
                var id = breadcrumb.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (id.Any())
                {
                    foreach (var item in id.Reverse())
                    {
                        var objF = list.FirstOrDefault(n => n.Id.Equals(item));
                        if (objF != null)
                        {
                            listModel.Add(new SelectListModel2 { ItemText = objF.MenuName, ItemValue = controller, ItemSecondValue = objF.isSystem ? 1 : 0 });
                        }
                    }
                }
            }

            ViewBag.ObjName = obj != null ? obj.MenuName : "";
            ViewBag.isSystem = obj != null ? obj.isSystem : false;

            ViewBag.GroupIDValue = group;

            return PartialView(listModel);
        }

        /// <summary>
        /// Danh sách sidebar
        /// </summary>
        /// <param name="actionName">Tên action</param>
        /// <param name="controllerName">Tên controller</param>
        /// <param name="openTab">Mở tab lớn khi có chọn con</param>
        /// <returns></returns>
        [CheckSessionLogin]
        public PartialViewResult Sidebar(string actionName = "Index", string controllerName = "Home", string openTab = "", string group = "")
        {
            // Current User
            var user = GetCurrentUser.GetUser();

            //
            controllerName = FunctionHelper.ConverControllerInGroup(controllerName);

            //Điều hướng
            string groupname = "";
            string direct = FunctionHelper.DirectActionByGroup(group, ref groupname);
            ViewBag.DirectValue = direct;

            // get all Role menu buy User
            var model = _MenuFunctionService.GetAllMenuByPermisstion(user.Id, user.Admin).Distinct().ToList();

            if (!string.IsNullOrWhiteSpace(group))
            {
                model = model.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(group)).ToList();
            }
            else
            {
                model = model.Where(n => n.isSystem == true).ToList();
            }

            ViewBag.Controller = controllerName;
            ViewBag.Action = actionName;
            ViewBag.GroupID = group;

            var obj = model.FirstOrDefault(n => n.ControllerName.Equals(controllerName) && n.ActionName.Equals(actionName));

            var breadcrumb = GetBreadcrumb(obj != null ? obj.Breadcrumb : "");
            ViewBag.Breadcrumb = breadcrumb;
            ViewBag.GroupNameValue = !string.IsNullOrWhiteSpace(groupname) ? groupname : "Trang chủ";

            return PartialView(model);
        }

        public PartialViewResult Child(string id, List<MenuFunction> listMenu, string controllerName, string actionName, string breadCrumb, string group = "")
        {
            controllerName = FunctionHelper.ConverControllerInGroup(controllerName);

            //Điều hướng
            string groupname = "";
            string direct = FunctionHelper.DirectActionByGroup(group, ref groupname);
            ViewBag.DirectValue = direct;

            //Danh sách menu
            var list = listMenu;

            ViewBag.BreadcrumbValue = breadCrumb;
            ViewBag.Controller = controllerName;
            ViewBag.Action = actionName;
            ViewBag.MenuFunctions = list;
            ViewBag.GroupID = group;

            list = list.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(group) && n.ParentId.Equals(id)).ToList();

            return PartialView(list);
        }

        /// <summary>
        /// Chứa danh sách các actions có trong hệ thống + phân quyền
        /// </summary>
        /// <param name="ControllerName">Tên controller</param>
        /// <param name="ActionName">Tên action</param>
        /// <param name="id">Id đối tượng thao tác</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        public PartialViewResult Actions(string ControllerName, string ActionName, string id = "", int pageNumber = 1, string group = "")
        {
            ViewBag.ControllerName = ControllerName;
            ViewBag.ActionName = ActionName;
            ViewBag.Id = id;
            ViewBag.PN = pageNumber;

            return PartialView();
        }

        /// <summary>
        /// Giao diện footer
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public PartialViewResult Buttons(string controller, string action, int pageNumber = 1)
        {
            ViewBag.Controller = controller;
            ViewBag.Action = action;
            ViewBag.PN = pageNumber;
            return PartialView();
        }


        public JsonResult DeleteCached()
        {
            bool isSucccess = true;
            try
            {
                CacheLayer.ClearAll();
            }
            catch (Exception)
            {
                isSucccess = false;
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }

        private string GetBreadcrumb(string parentid, string lastvalue)
        {
            var list = _MenuFunctionService.GetAllActive().ToList();
            var objParent = list.FirstOrDefault(n => n.Id.Equals(parentid));
            if (objParent != null)
            {
                lastvalue += objParent.Id + ",";

                var str = GetBreadcrumb(objParent.ParentId.ToString(), lastvalue);

                lastvalue = str;
            }

            return lastvalue;
        }

        private string GetBreadcrumb(string breadcrumb)
        {
            var lastvalue = "";

            var list = _MenuFunctionService.GetAllActive().ToList();
            list = list.Where(n => breadcrumb.Contains(n.Breadcrumb)).ToList();

            foreach (var item in list)
            {
                lastvalue += item.Id + ",";
            }

            return lastvalue;
        }

        #region Super admin
        [CheckSessionSuperAdminLogin]
        public ActionResult IndexAdmin()
        {
            ViewBag.SystemConfigValue = _tblSystemConfigService.GetDefault();


            var list = _MenuFunctionConfigService.GetAll().ToList();
            return View(list);
        }


        /// <summary>
        /// Danh sách sidebar
        /// </summary>
        /// <param name="actionName">Tên action</param>
        /// <param name="controllerName">Tên controller</param>
        /// <param name="openTab">Mở tab lớn khi có chọn con</param>
        /// <returns></returns>
        [CheckSessionSuperAdminLogin]
        public PartialViewResult SidebarAdmin(string actionName, string controllerName)
        {
            ViewBag.Controller = controllerName;

            return PartialView();
        }

        /// <summary>
        /// Thanh điều hướng
        /// </summary>
        /// <param name="controller">Tên controller</param>
        /// <param name="action">Tên action</param>
        /// <returns></returns>
        [CheckSessionSuperAdminLogin]
        public PartialViewResult BreadcrumbAdmin(string controller = "", string action = "", string group = "")
        {
            ViewBag.Controller = controller;

            ViewBag.Action = action;

            return PartialView();
        }


        public ActionResult HeaderAdmin()
        {
            return PartialView();
        }

        public PartialViewResult MenuFunctionList(List<MenuFunctionConfig> selected)
        {
            var list = _MenuFunctionService.GetAllMenu("").ToList();

            if (selected.Count == 0)
            {
                selected = _MenuFunctionConfigService.GetAll().ToList();
            }

            ViewBag.selectedValues = selected;

            return PartialView(list);
        }

        public PartialViewResult MenuFunctionChild(List<MenuFunction> childList, List<MenuFunction> AllList, List<MenuFunctionConfig> selected)
        {
            ViewBag.ListMenu = AllList;

            ViewBag.selectedValues = selected;

            return PartialView(childList);
        }

        public PartialViewResult ShowGroupName(string groupList, bool issystem = false)
        {
            string list = "&nbsp;";
            //< span class="label label-danger">

            //        </span>
            if (!string.IsNullOrWhiteSpace(groupList))
            {
                var ids = groupList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var item in ids)
                    {
                        var obj = FunctionHelper.GroupMenuList().FirstOrDefault(n => n.ItemValue.Equals(item));
                        if (obj != null)
                        {
                            switch (obj.ItemValue)
                            {
                                case "67810176":
                                    list += "<span class='label label-danger label-sm'> Tòa nhà </span> &nbsp;";
                                    break;

                                case "98818976":
                                    list += "<span class='label label-warning label-sm'> Vào ra </span> &nbsp;";
                                    break;

                                case "12878956":
                                    list += "<span class='label label-success label-sm'> Bãi xe </span> &nbsp;";
                                    break;

                                default:
                                    list += "<span class='label label-danger label-sm'> Tòa nhà </span> &nbsp;";
                                    break;
                            }
                        }
                    }
                }
            }

            if (issystem)
            {
                list += "<span class='label label-info label-sm'> Hệ thống </span> &nbsp;";
            }

            ViewBag.VALUE = list;

            return PartialView();
        }

        public JsonResult saveNewConfig(List<string> str)
        {
            var result = new MessageReport();

            try
            {
                if (str.Any())
                {
                    _MenuFunctionConfigService.DeleteAll();

                    foreach (var item in str)
                    {
                        var obj = new MenuFunctionConfig();
                        obj.Id = Common.GenerateId();
                        obj.MenuFunctionId = item;

                        _MenuFunctionConfigService.Create(obj);
                    }
                }

                result.isSuccess = true;
                result.Message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFileUpdate()
        {
            var result = new MessageReport();
            var filename = "";

            try
            {
                string error = "";
                string fullfolder = ConfigurationManager.AppSettings["FileUploadDownload"];

                var httpPostedFile = Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    filename = Common.UploadFile(out error, Server.MapPath(fullfolder), httpPostedFile);

                    if (string.IsNullOrWhiteSpace(error))
                    {
                        var url = string.Format("{0}{1}", fullfolder, filename);

                        DataSet ds = new DataSet();
                        ds.ReadXml(Server.MapPath(url));

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            var dt = ds.Tables[0];

                            var code = dt.Rows[0]["Code"].ToString();

                            //Check code theo key
                            var decode = CryptoProvider.SimpleDecryptWithPassword(code, SecurityModel.Keypass);

                            if (decode != null)
                            {
                                var t = ExcuteSQL.Execute(decode);
                                if (t)
                                {
                                    result.isSuccess = true;
                                    result.Message = "Nạp thành công";

                                    if (result.isSuccess)
                                    {
                                        SaveHistory(dt, filename);
                                    }
                                }
                                else
                                {
                                    result.isSuccess = false;
                                    result.Message = "Nạp thất bại";
                                }
                            }
                            else
                            {
                                result.isSuccess = false;
                                result.Message = "Sai bảo mật file";
                            }
                        }
                        else
                        {
                            result.isSuccess = false;
                            result.Message = "Nạp thất bại";
                        }
                    }
                    else
                    {
                        result.isSuccess = false;
                        result.Message = error;
                    }
                }
                else
                {
                    result.isSuccess = false;
                    result.Message = "Vui lòng chọn file";
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult History()
        {
            var list = _SystemRecordService.GetAll();

            return PartialView(list);
        } 

        private void SaveHistory(DataTable dt, string filename)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                var obj = new SystemRecord();
                obj.Id = Common.GenerateId();
                obj.Filename = filename;
                obj.Description = dt.Rows[0]["Description"].ToString();
                obj.DateCreated = DateTime.Now;
                obj.ComputerName = Common.GetComputerName(Request.UserHostAddress);

                _SystemRecordService.Create(obj);
            }

            
        }

        /// <summary>
        /// kích hoạt/hủy phân quyền nhóm thẻ
        /// </summary>
        /// <param name="isAuthInView"></param>
        /// <returns></returns>
        public JsonResult AuthInView(bool isAuthInView)
        {
            var result = new MessageReport(false,"Có lỗi xảy ra");

            var objsystem = _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.isAuthInView = isAuthInView;
                result = _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.isAuthInView ? "Đã kích hoạt tính năng phân quyền nhóm thẻ" : "Đã hủy tính năng phân quyền nhóm thẻ");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// kích hoạt/hủy mở thẻ , khóa thẻ
        /// </summary>
        /// <param name="isDisplay"></param>
        /// <returns></returns>
        public JsonResult Display(bool isDisplay)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.IsDisplay = isDisplay;
                result = _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.IsDisplay ? "Đã kích hoạt tính năng hiển thị nút mở thẻ và khóa thẻ" : "Đã hủy tính năng hiển thị nút mở thẻ khóa thẻ");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// kích hoạt chụp ảnh tự động
        /// </summary>
        /// <param name="isAutoCapture"></param>
        /// <returns></returns>
        public JsonResult AutoCapture(bool isAutoCapture)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.IsAutoCapture = isAutoCapture;
                result = _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.IsAutoCapture ? "Đã kích hoạt tính năng chụp ảnh tự động" : "Đã hủy tính năng chụp ảnh tự động");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// kích hoạt/hủy sử dụng thông tin căn hộ
        /// </summary>
        /// <param name="isAuthInView"></param>
        /// <returns></returns>
        public JsonResult UseCompartment(bool isCompartment)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.isCompartment = isCompartment;
                result = _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, objsystem.isCompartment ? "Đã kích hoạt tính năng sử dụng thông tin căn hộ" : "Đã hủy tính năng sử dụng thông tin căn hộ");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ẩn sự kiện ra
        /// </summary>
        /// <param name="IsHideEventOut"></param>
        /// <returns></returns>
        public JsonResult HideEventOut(bool IsHideEventOut)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var objsystem = _tblSystemConfigService.GetDefault();

            if (objsystem != null)
            {
                objsystem.IsHideEventOut = IsHideEventOut;
                result = _tblSystemConfigService.Update(objsystem);

                if (result.isSuccess)
                {
                    result = new MessageReport(true, "Cập nhật thành công");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}