using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using System.Collections.Generic;
using System.Web;
using System;
using System.Web.Mvc;
using Kztek.Web.Core.Helpers;
using System.Linq;
using Kztek.Model.CustomModel;
using System.Configuration;
using Kztek.Web.Models;
using Kztek.Web.Core.Models;
/* **************************************
* HỆ THỐNG GENCODE TỰ ĐỘNG
* CREATE: 09/05/2018 4:47:36 PM
* AUTHOR: HNG-0988388000
*/
namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblDoorController : Controller
    {
        private ItblAccessDoorService _tblAccessDoorService;
        private ItblAccessControllerService _tblAccessControllerService;
        private static string url = "";

        public tblDoorController(ItblAccessDoorService _tblAccessDoorService, ItblAccessControllerService _tblAccessControllerService)
        {
            this._tblAccessDoorService = _tblAccessDoorService;
            this._tblAccessControllerService = _tblAccessControllerService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string controllerid, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblAccessDoorService.GetAllPagingByFirst(key, controllerid, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblAccessDoor>.GetPage(list, page, pageSize);

            //ViewBag
            ViewBag.Key = string.IsNullOrWhiteSpace(key) ? "" : key;
            ViewBag.ControllerIDValue = controllerid;
            ViewBag.Controllers = GetControllerList();
            url = Request.Url.PathAndQuery;

            //Đưa ra giao diện
            return View(listModel);
        }

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(int ordering = 1)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();
            ViewBag.Ordering = ordering;

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblAccessDoor obj, string TypeSelect, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.DoorName))
            {
                ModelState.AddModelError("", "Vui lòng nhập Output");
                return View(obj);
            }

            obj.DoorID = Guid.NewGuid();
            var report = _tblAccessDoorService.Create(obj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.DoorID.ToString(), obj.DoorName, "tblAccessDoor", ConstField.AccessControlCode, ActionConfigO.Create);

                TempData["Success"] = report.Message;
                if (SaveAndCountinue)
                {
                    return RedirectToAction("Create", new { ordering = obj.Ordering + 1 });
                }

                return Redirect(url);
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
            var obj = _tblAccessDoorService.GetById(Guid.Parse(id));
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(tblAccessDoor obj, string TypeSelect)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.DoorName))
            {
                ModelState.AddModelError("", "Vui lòng nhập Output");
                return View(obj);
            }

            var oldobj = _tblAccessDoorService.GetById(obj.DoorID);

            if (oldobj == null) return View(obj);

            oldobj.DoorName = obj.DoorName;
            oldobj.ControllerID = obj.ControllerID;
            oldobj.ReaderIndex = obj.ReaderIndex;
            oldobj.Inactive = obj.Inactive;

            var report = _tblAccessDoorService.Update(oldobj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), oldobj.DoorID.ToString(), oldobj.DoorName, "tblAccessDoor", ConstField.AccessControlCode, ActionConfigO.Update);

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }
        }
        [CheckAuthorize]
        public JsonResult Delete(string id)
        {
            var report = _tblAccessDoorService.DeleteById(id);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        private List<tblAccessController> GetControllerList()
        {
            return _tblAccessControllerService.GetAllActive().ToList();
        }
    }
}
