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

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblLineController : Controller
    {
        #region Khai báo services

        private ItblAccessLineService _tblAccessLineService;
        private ItblAccessPCService _tblAccessPCService;
        private static string url = "";

        public tblLineController(ItblAccessLineService _tblAccessLineService, ItblAccessPCService _tblAccessPCService)
        {
            this._tblAccessLineService = _tblAccessLineService;
            this._tblAccessPCService = _tblAccessPCService;
        }

        #endregion Khai báo services

        #region Danh sách

        /// <summary>
        /// Danh sách kết nối
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="pc">Id máy tính</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", string pc = "", int page = 1)
        {
            int pageSize = 20;

            var list = _tblAccessLineService.GetAllPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblAccessLine>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            ViewBag.PCs = GetPCList();
            ViewBag.PCID = pc;

            url = Request.Url.PathAndQuery;

            return View(gridModel);
        }

        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PCs = GetPCList();
            ViewBag.Communications = GetCommunicationList();
            ViewBag.LineTypes = GetLineList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         16/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Tiếp tục không</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblAccessLine obj, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Danh sách sử dụng
            ViewBag.PCs = GetPCList();
            ViewBag.Communications = GetCommunicationList();
            ViewBag.LineTypes = GetLineList();

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị

            obj.LineID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblAccessLineService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LineID.ToString(), obj.LineName, "tblAccessLine", ConstField.AccessControlCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return Redirect("Index");
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Update(string id)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            ViewBag.PCs = GetPCList();
            ViewBag.Communications = GetCommunicationList();
            ViewBag.LineTypes = GetLineList();

            var obj = _tblAccessLineService.GetById(Guid.Parse(id));
            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(tblAccessLine obj)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Danh sách sử dụng
            ViewBag.PCs = GetPCList();
            ViewBag.Communications = GetCommunicationList();
            ViewBag.LineTypes = GetLineList();

            //Kiểm tra
            var oldObj = _tblAccessLineService.GetById(obj.LineID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.DownloadTime = obj.DownloadTime;
            oldObj.Baudrate = obj.Baudrate;
            oldObj.CommunicationType = obj.CommunicationType;
            oldObj.Comport = obj.Comport;
            oldObj.Inactive = obj.Inactive;
            oldObj.LineTypeID = obj.LineTypeID;
            oldObj.PCID = obj.PCID;
            oldObj.LineTypeID = obj.LineTypeID;
            oldObj.LineName = obj.LineName;

            //Thực hiện cập nhật
            var result = _tblAccessLineService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.LineID.ToString(), oldObj.LineName, "tblAccessLine", ConstField.AccessControlCode, ActionConfigO.Update);

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var report = _tblAccessLineService.DeleteById(id);
            if (report.isSuccess)
            {
                WriteLog.Write(report, GetCurrentUser.GetUser(), id, id, "tblAccessLine", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private List<tblAccessPC> GetPCList()
        {
            return _tblAccessPCService.GetAll().ToList();
        }

        /// <summary>
        /// Danh sách loại truyền thông
        /// </summary>
        /// <returns></returns>
        private List<SelectListModel6> GetCommunicationList()
        {
            return FunctionHelper.Communication1();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private List<SelectListModel> GetLineList()
        {
            return FunctionHelper.LineTypes2();
        }
    }
}