using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class tblPCController : Controller
    {
        private ItblLockerPCService _tblLockerPCService;
        private static string url = "";

        public tblPCController(ItblLockerPCService _tblLockerPCService)
        {
            this._tblLockerPCService = _tblLockerPCService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblLockerPCService.GetAllPagingByFirst(key, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblLockerPC>.GetPage(list, page, pageSize);

            //ViewBag
            ViewBag.keyValue = key;

            url = Request.Url.PathAndQuery;

            //Đưa ra giao diện
            return View(listModel);
        }

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create()
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblLockerPC obj, string TypeSelect, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }

            obj.Id = Guid.NewGuid().ToString();

            var report = _tblLockerPCService.Create(obj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.PCName, "tblLockerPC", ConstField.LockerCode, ActionConfigO.Create);

                TempData["Success"] = "Thêm mới thành công";

                if (SaveAndCountinue)
                {
                    return RedirectToAction("Create", "tblPC");
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
            var obj = _tblLockerPCService.GetById(id);
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(tblLockerPC obj)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldobj = _tblLockerPCService.GetById(obj.Id);

            if (oldobj == null) return View(obj);

            oldobj.Description = obj.Description;
            oldobj.Active = obj.Active;
            oldobj.IPAddress = obj.IPAddress;
            oldobj.PCName = obj.PCName;

            var report = _tblLockerPCService.Update(oldobj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), oldobj.Id.ToString(), oldobj.PCName, "tblLockerPC", ConstField.LockerCode, ActionConfigO.Update);

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(oldobj);
            }
        }

        public JsonResult Delete(string id)
        {
            var report = _tblLockerPCService.DeleteById(id);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), id, id, "tblLockerPC", ConstField.LockerCode, ActionConfigO.Delete);
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}