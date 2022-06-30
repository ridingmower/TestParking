using Kztek.Model.CustomModel;
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
    public class tblLockerController : Controller
    {
        private ItblLockerService _tblLockerService;
        private ItblLockerControllerService _tblLockerControllerService;
        private static string url = "";

        public tblLockerController(ItblLockerService _tblLockerService, ItblLockerControllerService _tblLockerControllerService)
        {
            this._tblLockerService = _tblLockerService;
            this._tblLockerControllerService = _tblLockerControllerService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string controllerid, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblLockerService.GetAllPagingByFirst(key, controllerid, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblLocker>.GetPage(list, page, pageSize);

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
        public ActionResult Create()
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblLocker obj, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
                return View(obj);
            }

            var existed = _tblLockerService.GetByName(obj.Name);
            if (existed != null)
            {
                ModelState.AddModelError("Name", "Tên tủ đồ đã bị trùng");
                return View(obj);
            }

            var existedIndex = _tblLockerService.GetByControllerID_ReaderIndex(obj.ControllerID, obj.ReaderIndex);
            if (existedIndex != null)
            {
                ModelState.AddModelError("ReaderIndex", "Tên tủ đồ đang để trùng địa chỉ reader");
                return View(obj);
            }

            obj.Id = Guid.NewGuid().ToString();
            obj.DateCreated = DateTime.Now;
            obj.LockerType = "0";

            var report = _tblLockerService.Create(obj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "tblLocker", ConstField.LockerCode, ActionConfigO.Create);

                TempData["Success"] = report.Message;
                if (SaveAndCountinue)
                {
                    return RedirectToAction("Create");
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
            var obj = _tblLockerService.GetById(id);
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(tblLocker obj, string TypeSelect)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.Controllers = GetControllerList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var existed = _tblLockerService.GetByName_Id(obj.Name, obj.Id);
            if (existed != null)
            {
                ModelState.AddModelError("Name", "Tên tủ đồ đã bị trùng");
                return View(obj);
            }

            var existedIndex = _tblLockerService.GetByControllerID_ReaderIndex_Id(obj.ControllerID, obj.ReaderIndex, obj.Id);
            if (existedIndex != null)
            {
                ModelState.AddModelError("ReaderIndex", "Tên tủ đồ đang để trùng địa chỉ reader");
                return View(obj);
            }

            var oldobj = _tblLockerService.GetById(obj.Id);

            if (oldobj == null) return View(obj);

            oldobj.Name = obj.Name;
            oldobj.ControllerID = obj.ControllerID;
            oldobj.ReaderIndex = obj.ReaderIndex;

            var report = _tblLockerService.Update(oldobj);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), oldobj.Id.ToString(), oldobj.Name, "tblLocker", ConstField.LockerCode, ActionConfigO.Update);

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
            var report = _tblLockerService.DeleteById(id);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        private List<Kztek.Model.Models.tblLockerController> GetControllerList()
        {
            return _tblLockerControllerService.GetAllActive().ToList();
        }

        public PartialViewResult ModelCreateQuick()
        {
            ViewBag.Controllers = GetControllerList();

            return PartialView();
        }

        public JsonResult CreateQuick(string prefix = "", int startNumber = 0, int maxNumber = 0, string controllerid = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Check
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    result = new MessageReport(false, "Tiền tố không thể để trống");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(controllerid))
                {
                    result = new MessageReport(false, "Vui lòng chọn bộ điều khiển");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (maxNumber > 32)
                {
                    result = new MessageReport(false, "Mỗi BĐK tối đa nhận 32 tủ");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //Gán
                for (int i = 1; i <= maxNumber; i++)
                {
                    var obj = new tblLocker()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CardNo = "",
                        CardNumber = "",
                        ControllerID = controllerid,
                        DateCreated = DateTime.Now,
                        LockerType = "0",
                        Name = string.Format("{0} {1}", prefix, startNumber),
                        ReaderIndex = i
                    };

                    startNumber++;

                    //Kiểm tra đã tồn tại chưa
                    var exited = _tblLockerService.GetByControllerID_ReaderIndex(controllerid, i);
                    if (exited == null)
                    {
                        _tblLockerService.CreateSQL(obj);
                    }
                }

                //
                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}