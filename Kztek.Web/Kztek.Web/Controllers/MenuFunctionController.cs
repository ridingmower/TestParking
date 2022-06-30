using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using Kztek.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Configuration;
using Kztek.Web.Core.Functions;

namespace Kztek.Web.Controllers
{
    public class MenuFunctionController : Controller
    {
        #region Khai báo services

        private IMenuFunctionService _MenuFunctionService;

        public MenuFunctionController(IMenuFunctionService _MenuFunctionService)
        {
            this._MenuFunctionService = _MenuFunctionService;
        }

        #endregion Khai báo services

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page, string group = "")
        {
            var lst = _MenuFunctionService.GetAllMenu(key, group);

            //Viewbag
            ViewBag.DDLActive = FunctionHelper.ActiveStatus();
            ViewBag.Keyword = key;
            //ViewBag.ListMenu = _MenuFunctionService.GetAll().ToList();

            ViewBag.GroupID = group;
            ViewBag.listGroupAllow = listGroupAllow();

            return View(lst);
        }

        #endregion Danh sách

        public PartialViewResult MenuChilden(List<MenuFunction> childList, List<MenuFunction> AllList, string group = "")
        {
            //Viewbag
            ViewBag.ListMenu = AllList;
            ViewBag.GroupID = group;

            return PartialView(childList);
        }

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string controllername, string parentid, string menytype, string grouplist, string ordernu = "1", string group = "")
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();
            ViewBag.controller = controllername;
            ViewBag.parent = parentid;
            ViewBag.menytypeValue = menytype;
            ViewBag.grouplistValue = grouplist;
            ViewBag.ordernuValue = ordernu;

            ViewBag.GroupID = group;

            return View();
        }

        [HttpPost]
        public ActionResult Create(MenuFunction obj, bool SaveAndCountinue = false, string group = "")
        {
            if (String.IsNullOrEmpty(obj.MenuName) || String.IsNullOrWhiteSpace(obj.MenuName))
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["menu_Name"]);
                ViewBag.DDLMenu = GetMenuList();
                ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
                ViewBag.DDLMenuType = FunctionHelper.MenuType();
                ViewBag.GroupID = group;
                return View(obj);
            }
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();

            ViewBag.GroupID = group;

            if (ModelState.IsValid)
            {
                obj.Id = Common.GenerateId();
                obj.ControllerName = obj.ControllerName != null ? obj.ControllerName : string.Format("controller_{0}", obj.Id);
                obj.ActionName = obj.ActionName != null ? obj.ActionName : string.Format("action_{0}", obj.Id);
                obj.Url = string.Format("/{0}/{1}", obj.ControllerName, obj.ActionName);
                obj.Deleted = false;
                bool isSuccess = _MenuFunctionService.Create(obj);
                if (isSuccess)
                {
                    //For cache
                    CacheLayer.ClearAll();

                    //Write report
                    MessageReport report = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"]);
                    WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.MenuName, "MenuFunction");

                    if (SaveAndCountinue)
                    {
                        TempData["Success"] = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"];
                        return RedirectToAction("Create", "MenuFunction", new { controllername = obj.ControllerName, parentid = obj.ParentId, menytype = obj.MenuType, grouplist = obj.MenuGroupListId, group = group, ordernu = obj.OrderNumber + 1 });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { group = group });
                    }
                }
                else
                {
                    ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["ErMenu"]);
                    return View(obj);
                }
            }

            return View();
        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id, string group = "")
        {
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();

            ViewBag.GroupID = group;

            var obj = _MenuFunctionService.getById(id);
            if (obj != null)
            {
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Update(MenuFunction obj, string group = "")
        {
            //ViewBag
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();

            ViewBag.GroupID = group;

            if (String.IsNullOrEmpty(obj.MenuName) || String.IsNullOrWhiteSpace(obj.MenuName))
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["menu_Name"]);
                ViewBag.DDLMenu = GetMenuList();
                ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
                ViewBag.DDLMenuType = FunctionHelper.MenuType();
                ViewBag.GroupID = group;
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                obj.ControllerName = obj.ControllerName != null ? obj.ControllerName : string.Format("controller_{0}", obj.Id);
                obj.ActionName = obj.ActionName != null ? obj.ActionName : string.Format("action_{0}", obj.Id);
                obj.Url = string.Format("/{0}/{1}", obj.ControllerName, obj.ActionName);

                bool isSuccess = _MenuFunctionService.Update(obj);
                ViewBag.DDLMenu = GetMenuList();

                if (isSuccess)
                {
                    CacheLayer.ClearAll();
                    MessageReport report = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"]);
                    WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.MenuName, "MenuFunction");

                    return RedirectToAction("Index", new { group = group });
                }
                else
                {
                    ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["ErMenu"]);
                    return View(obj);
                }
            }
            return View();
        }

        #endregion Cập nhật

        #region Xóa nhiều

        public JsonResult MutilDelete(string lstId)
        {
            bool isSucccess = _MenuFunctionService.DeleteByIds(lstId);
            if (isSucccess)
            {
                CacheLayer.ClearAll();

                MessageReport report = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"]);
                WriteLog.Write(report, GetCurrentUser.GetUser(), lstId, lstId, "MenuFunction");
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa nhiều

        #region Chuyển đổi trạng thái

        public JsonResult Active(string lstId, string nhaptrangthai)
        {
            bool isSuccess = _MenuFunctionService.ActiveByIds(lstId, nhaptrangthai);

            if (isSuccess)
            {
                CacheLayer.ClearAll();
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Chuyển đổi trạng thái

        private List<MenuFunctionSubmit> GetMenuList()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "menuFunction");
            var list = new List<MenuFunctionSubmit>
            {
                new MenuFunctionSubmit {  Id = "0", MenuName = Dictionary["menuFunction"] }
            };
            var MenuList = _MenuFunctionService.GetAllActive();
            var parent = MenuList.Where(c => c.ParentId == "0");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.OrderNumber))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                }
            }
            return list;
        }

        private List<MenuFunctionSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<MenuFunctionSubmit> lst = new List<MenuFunctionSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _MenuFunctionService.GetAllChildByParentId(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                    }
                }
            }
            return lst;
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

        public IList<string> listGroupAllow()
        {
            var rtnList = new List<string>();
            var listGroup = ConfigurationManager.AppSettings["FunctionGroupAllow"];
            if (!string.IsNullOrWhiteSpace(listGroup))
            {
                var arr = listGroup.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in arr)
                {
                    rtnList.Add(s);
                }
            }

            return rtnList;

        }
    }
}