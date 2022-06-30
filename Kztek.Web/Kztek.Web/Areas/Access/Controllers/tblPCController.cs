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

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblPCController : Controller
    {
        private ItblAccessPCService _tblAccessPCService;
        private static string url = "";

        public tblPCController(ItblAccessPCService _tblAccessPCService)
        {
            this._tblAccessPCService = _tblAccessPCService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblAccessPCService.GetAllPagingByFirst(key, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblAccessPC>.GetPage(list, page, pageSize);

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
        public ActionResult Create(tblAccessPC obj, string TypeSelect, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }

            obj.PCID = Guid.NewGuid();

            var report = _tblAccessPCService.Create(obj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.PCID.ToString(), obj.PCName, "tblAccessPC", ConstField.AccessControlCode, ActionConfigO.Create);

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
            var obj = _tblAccessPCService.GetById(Guid.Parse(id));
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(tblAccessPC obj)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldobj = _tblAccessPCService.GetById(obj.PCID);

            if (oldobj == null) return View(obj);

            oldobj.Description = obj.Description;
            oldobj.Inactive = obj.Inactive;
            oldobj.IPAddress = obj.IPAddress;
            oldobj.PCName = obj.PCName;

            var report = _tblAccessPCService.Update(oldobj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), oldobj.PCID.ToString(), oldobj.PCName, "tblAccessPC", ConstField.AccessControlCode, ActionConfigO.Update);

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
            var report = _tblAccessPCService.DeleteById(id);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), id, id, "tblAccessPC", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}