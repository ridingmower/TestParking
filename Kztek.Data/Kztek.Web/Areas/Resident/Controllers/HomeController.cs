using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class HomeController : Controller
    {
        private IMenuFunctionService _MenuFunctionService;
    

        public HomeController(IMenuFunctionService _MenuFunctionService)
        {
            this._MenuFunctionService = _MenuFunctionService;
         
        }

        private const string groupId = ConstField.ResidentID;

        [CheckSessionLogin]
        public ActionResult Index(string fromdate, string todate)
        {
            fromdate = !string.IsNullOrWhiteSpace(fromdate) ? Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy 00:00") : DateTime.Now.ToString("dd/MM/yyyy 00:00");

            todate = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy 00:00") : DateTime.Now.ToString("dd/MM/yyyy 23:59");

            ViewBag.fromdateValue = fromdate;

            ViewBag.todateValue = todate;

            return View();
        }

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
        public PartialViewResult Breadcrumb(string controller = "", string action = "", string group = "")
        {
            //Điều hướng
            string groupname = "";
            string direct = FunctionHelper.DirectActionByGroup(group, ref groupname);
            ViewBag.DirectValue = direct;

            controller = FunctionHelper.ConverControllerInGroup(controller);

            //Load file ngôn ngữ
            var DictionaryFunction = FunctionHelper.GetLocalizeDictionary("Home", "MenuFunction");

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

                        //Đổi text theo ngôn ngữ
                        var _menuName = "";
                        if (objF.ControllerName.Contains("controller_"))
                        {
                            DictionaryFunction.TryGetValue(objF.ControllerName, out _menuName);
                        }
                        else
                        {
                            DictionaryFunction.TryGetValue($"{objF.ControllerName}_{objF.ActionName}", out _menuName);
                        }
                        objF.MenuName = !string.IsNullOrWhiteSpace(_menuName) ? _menuName : objF.MenuName;

                        if (objF != null)
                        {
                            listModel.Add(new SelectListModel2 { ItemText = objF.MenuName, ItemValue = controller, ItemSecondValue = objF.isSystem ? 1 : 0 });
                        }
                    }
                }
            }


            if (obj != null)
            {
                var _actionName = "";

                if (obj.ParentId == "0")
                {
                    DictionaryFunction.TryGetValue($"{obj.ControllerName}", out _actionName);
                }
                else
                {
                    DictionaryFunction.TryGetValue($"{obj.ControllerName}_{obj.ActionName}", out _actionName);
                }
                obj.MenuName = !string.IsNullOrWhiteSpace(_actionName) ? _actionName : obj.MenuName;
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

            var breadcrumb = GetBreadcrumb(obj != null ? obj.ParentId.ToString() : "0", "");
            ViewBag.Breadcrumb = string.Format("{0},{1}", obj != null ? obj.Id : "", !string.IsNullOrWhiteSpace(breadcrumb) ? breadcrumb : "");
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
            var versionStr = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

            var versionArr = versionStr.Split('.');

            ViewBag.Version = $"{versionArr[0]}.{versionArr[1]}.{versionArr[2]}";

            return PartialView();
        }

        public PartialViewResult Buttons(string controller, string action, int pageNumber = 1, string url = "")
        {
            ViewBag.Controller = controller;
            ViewBag.Action = action;
            ViewBag.PN = pageNumber;
            ViewBag.urlValue = url;

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
    }
}