using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblAccessControllerGroupController : Controller
    {
        private ItblAccessControllerGroupService _tblAccessControllerGroupService;
        private static string url = "";

        public tblAccessControllerGroupController(ItblAccessControllerGroupService _tblAccessControllerGroupService)
        {
            this._tblAccessControllerGroupService = _tblAccessControllerGroupService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblAccessControllerGroupService.GetAllPagingByFirst(key, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblAccessControllerGroup>.GetPage(list, page, pageSize);

            //ViewBag
            ViewBag.keyValue = key;

            url = Request.Url.PathAndQuery;

            //Đưa ra giao diện
            return View(listModel);
        }

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string sort)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            var newobj = new tblAccessControllerGroup
            {
                SortOrder = !string.IsNullOrEmpty(sort) ? Convert.ToInt32(sort) + 1 : 0
            };

            return View(newobj);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblAccessControllerGroup obj, string TypeSelect, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            if (string.IsNullOrEmpty(obj.Name))
            {
                ModelState.AddModelError("Name", "Vui lòng nhập tên danh mục");
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }

           

            obj.Id = Common.GenerateId();

            var report = _tblAccessControllerGroupService.Create(obj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "tblAccessControllerGroup", ConstField.AccessControlCode, ActionConfigO.Create);

                TempData["Success"] = "Thêm mới thành công";

                if (SaveAndCountinue)
                {
                    return RedirectToAction("Create", "tblAccessControllerGroup",new { sort = obj.SortOrder});
                }

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }
        }

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id)
        {
            var obj = _tblAccessControllerGroupService.GetById(id);
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(tblAccessControllerGroup obj)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            if (string.IsNullOrEmpty(obj.Name))
            {
                ModelState.AddModelError("Name", "Vui lòng nhập tên danh mục");
                return View(obj);
            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldobj = _tblAccessControllerGroupService.GetById(obj.Id);

            if (oldobj == null) return View(obj);

            oldobj.Description = obj.Description;
            oldobj.Name = obj.Name;
            oldobj.SortOrder = obj.SortOrder;

            var report = _tblAccessControllerGroupService.Update(oldobj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), oldobj.Id.ToString(), oldobj.Name, "tblAccessControllerGroup", ConstField.AccessControlCode, ActionConfigO.Update);

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(oldobj);
            }
        }

        public JsonResult Delete(string id)
        {
            var report = _tblAccessControllerGroupService.DeleteById(id);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), id, id, "tblAccessControllerGroup", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}