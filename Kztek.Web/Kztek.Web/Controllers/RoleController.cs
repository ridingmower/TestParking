using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using Kztek.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class RoleController : Controller
    {
        #region Khai báo services

        private IRoleService _RoleService;
        private IRoleMenuService _RoleMenuService;
        private IMenuFunctionService _MenuFunctionService;
        private IUserRoleService _UserRoleService;

        public RoleController(IRoleService _RoleService, IRoleMenuService _RoleMenuService, IMenuFunctionService _MenuFunctionService, IUserRoleService _UserRoleService)
        {
            this._RoleService = _RoleService;
            this._RoleMenuService = _RoleMenuService;
            this._MenuFunctionService = _MenuFunctionService;
            this._UserRoleService = _UserRoleService;
        }

        #endregion Khai báo services

        private static string GroupID = "";

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string group = "")
        {
            ViewBag.GroupID = group;
            GroupID = group;
            var list = _RoleService.GetAllByFirst(key);
            return View(list);
        }

        #endregion Danh sách

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string group = "")
        {
            ViewBag.Selected = "";
            ViewBag.GroupID = group;

            ViewBag.MenuFunctionList = _MenuFunctionService.GetAllActive().ToList();
            return View();
        }

        public ActionResult Create(Role obj, string menufunctionvalues, bool SaveAndCountinue = false, string group = "")
        {
            if (String.IsNullOrEmpty(obj.RoleName) || String.IsNullOrWhiteSpace(obj.RoleName))
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Role_Name"]);
                ViewBag.Selected = "";
            ViewBag.GroupID = group;

            ViewBag.MenuFunctionList = _MenuFunctionService.GetAllActive().ToList();
                return View(obj);
            }
            //
            ViewBag.Selected = menufunctionvalues;
            ViewBag.GroupID = group;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Common.GenerateId();

            //Thêm danh sách cây menu
            if (!string.IsNullOrWhiteSpace(menufunctionvalues))
            {
                var ids = menufunctionvalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = obj.Id;
                        objRoleMenu.MenuId = id;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            //Thực hiện thêm mới
            var result = _RoleService.Create(obj);
            if (result)
            {
                var mes = new MessageReport();
                mes.isSuccess = true;
                mes.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["Role_New"];

                var name = FunctionHelper.getCurrentGroup(group);
                WriteLog.Write(mes, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role", name, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"];
                    return RedirectToAction("Create", new { group = group });
                }
                return RedirectToAction("Index", new { group = group });
            }
            else
            {
                return View(obj);
            }
        }

        public JsonResult CreateRole(Role obj, string lstId)
        {
            bool isSuccess = false;
            obj.Id = Common.GenerateId();

            if (ModelState.IsValid)
            {
                isSuccess = _RoleService.Create(obj);
            }

            if (isSuccess)
            {
                MessageReport report = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"]);
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role");
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids != null)
                {
                    foreach (var item in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = obj.Id;
                        objRoleMenu.MenuId = item;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id, string group = "")
        {
            ViewBag.GroupID = group;
            string str = "";
            var obj = _RoleService.GetById(id);

            var t = _RoleMenuService.GetAllByRoleId(id).ToList();
            if (t != null && t.Any())
            {
                foreach (var item in t)
                {
                    str += item.MenuId + ",";
                }
            }

            ViewBag.Selected = str;

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(Role obj, string menufunctionvalues, string group = "")
        {
            if (String.IsNullOrEmpty(obj.RoleName) || String.IsNullOrWhiteSpace(obj.RoleName))
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Role_Name"]);
                Update( obj.Id,  group = "");
            }
            //
            ViewBag.Selected = menufunctionvalues;
            ViewBag.GroupID = group;

            //Kiểm tra
            var oldObj = _RoleService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                Update(obj.Id, group = "");
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.RoleName = obj.RoleName;
            oldObj.Description = obj.Description;
            oldObj.Active = obj.Active;

            //Cập nhật lại danh sách menu
            var listmenu = _MenuFunctionService.GetAllMenu("",group).Select(n => n.Id).ToList();
            var list = _RoleMenuService.GetAllByRoleId(obj.Id, listmenu).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    _RoleMenuService.DeleteById(item.Id);
                }
            }

            if (!string.IsNullOrWhiteSpace(menufunctionvalues))
            {
                string[] ids = menufunctionvalues.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids != null)
                {
                    foreach (var item in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = oldObj.Id;
                        objRoleMenu.MenuId = item;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            //Thực hiện cập nhật
            var result = _RoleService.Update(oldObj);
            if (result)
            {
                var mes = new MessageReport();
                mes.isSuccess = true;
                mes.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["Role_update"];

                var formatRole = string.Format("{0}_{1}", ConstField.ListRoleMenu, oldObj.Id);
                CacheLayer.Clear(formatRole);

                var name = FunctionHelper.getCurrentGroup(group);
                WriteLog.Write(mes, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role", name, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group });
            }
            else
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["ErMenu"]);
                return View(oldObj);
            }
        }

        //[HttpPost]
        //public ActionResult Update(Role obj)
        //{
        //    //ViewBag.MenuFunctionList = _MenuFunctionService.GetAllActive().ToList();
        //    if (ModelState.IsValid)
        //    {
        //        bool isSuccess = _RoleService.Update(obj);
        //        if (isSuccess)
        //        {
        //            MessageReport report = new MessageReport(true, "Cập nhật thành công");
        //            WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role");

        //            FunctionHelper.ClearCache(ConstField.ListRoleMenu, obj.Id);

        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Có lỗi xảy ra");
        //            return View(obj);
        //        }
        //    }
        //    return View(obj);
        //}

        public JsonResult UpdateRole(string lstId, string RoleId)
        {
            bool isSuccess = false;
            try
            {
                var list = _RoleMenuService.GetAllByRoleId(RoleId).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        _RoleMenuService.DeleteById(item.Id);
                    }
                }

                if (!string.IsNullOrWhiteSpace(lstId))
                {
                    string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            RoleMenu objRoleMenu = new RoleMenu();
                            objRoleMenu.Id = Common.GenerateId();
                            objRoleMenu.RoleId = RoleId;
                            objRoleMenu.MenuId = item;
                            isSuccess = _RoleMenuService.Create(objRoleMenu);
                        }
                    }
                }
                else
                {
                    isSuccess = true;
                }

                if (isSuccess)
                {
                    FunctionHelper.ClearCache(ConstField.ListRoleMenu, RoleId);
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Cập nhật

        #region Xóa
        /// <summary>
        /// ///Xóa 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckAuthorize]
        public JsonResult Delete(string id)
        {
            MessageReport report = new MessageReport(false, "Có lỗi xảy ra");

            var obj = _RoleService.GetById(id);

            bool isSuccess = _RoleService.DeleteById(id);
            if (isSuccess)
            {
                report = new MessageReport(true, "Xóa thành công");

                var name = FunctionHelper.getCurrentGroup(GroupID);
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role", name, ActionConfigO.Delete);

                FunctionHelper.ClearCache(ConstField.ListRoleMenu, id);

                var listRoleMenu = _RoleMenuService.GetAllByRoleId(id);
                if (listRoleMenu.Any())
                {
                    foreach (var item in listRoleMenu.ToList())
                    {
                        _RoleMenuService.DeleteById(item.Id);
                    }
                }

                var listUserRole = _UserRoleService.GetAllByRoleId(id);
                if (listUserRole.Any())
                {
                    foreach (var item in listUserRole.ToList())
                    {
                        _UserRoleService.DeleteById(item.Id);
                    }
                }
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        public PartialViewResult MenuFunctionParent(string roleid)
        {
            var listRoleMenu = _RoleMenuService.GetAllByRoleId(roleid).ToList();

            ViewBag.RoleMenuList = listRoleMenu;
            ViewBag.RoleId = roleid;

            var list = _MenuFunctionService.GetAllActive().ToList();
            return PartialView(list);
        }

        public PartialViewResult MenuFunctionChild(string parentid, string roleid)
        {
            var listRoleMenu = _RoleMenuService.GetAllByRoleId(roleid).ToList();

            ViewBag.RoleMenuList = listRoleMenu;
            ViewBag.RoleId = roleid;
            ViewBag.MenuList = _MenuFunctionService.GetAllActive().ToList();

            var list = _MenuFunctionService.GetAllActiveChildByParentId(parentid).ToList();
            return PartialView(list);
        }

        #region Cây menu

        /// <summary>
        /// Cây menu hệ thống
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="str">List đã chọn</param>
        /// <returns></returns>
        public PartialViewResult MenuFunctionList(string str, string gId)
        {
            ViewBag.Selected = str;

            var list = _MenuFunctionService.GetAllParentActiveByGroupId(gId).ToList();

            ViewBag.gIdValue = gId;
            return PartialView(list);
        }

        public PartialViewResult Child(string parentId, string selectedId, string gId)
        {
            ViewBag.Selected = selectedId;
            var list = _MenuFunctionService.GetAllChildActiveByParentId(parentId, gId).ToList();
            return PartialView(list);
        }

        #endregion Cây menu

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
    }
}