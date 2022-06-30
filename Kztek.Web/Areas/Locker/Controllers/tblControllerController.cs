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
    public class tblControllerController : Controller
    {
        #region Khai báo services

        private ItblLockerControllerService _tblLockerControllerService;
        private ItblLockerLineService _tblLockerLineService;
        private static string url = "";

        public tblControllerController(ItblLockerControllerService _tblLockerControllerService, ItblLockerLineService _tblLockerLineService)
        {
            this._tblLockerControllerService = _tblLockerControllerService;
            this._tblLockerLineService = _tblLockerLineService;
        }

        #endregion Khai báo services

        #region Danh sách

        /// <summary>
        /// Danh sách bộ điều khiển
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="pc">Id máy tính</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        public ActionResult Index(string key = "", string line = "", int page = 1)
        {
            //var totalPage = 0;
            //var totalItem = 0;
            var pageSize = 20;

            var list = _tblLockerControllerService.GetAllPagingByFirst(key, line, page, pageSize);

            var gridModel = PageModelCustom<Kztek.Model.Models.tblLockerController>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            ViewBag.Lines = GetLineList();
            ViewBag.LineID = line;

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
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Lines = GetLineList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Thêm liên tục hay không?</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Kztek.Model.Models.tblLockerController obj, bool SaveAndCountinue = false)
        {
            //Danh sách sử dụng
            ViewBag.Lines = GetLineList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Guid.NewGuid().ToString();

            //Thực hiện thêm mới
            var result = _tblLockerControllerService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.ControllerName, "tblLockerController", ConstField.LockerCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return Redirect(url);
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
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Update(string id)
        {
            ViewBag.Lines = GetLineList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            var obj = _tblLockerControllerService.GetById(id);


            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(Kztek.Model.Models.tblLockerController obj)
        {
            //Danh sách sử dụng
            ViewBag.Lines = GetLineList();
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            var oldObj = _tblLockerControllerService.GetById(obj.Id);
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
            oldObj.ControllerName = obj.ControllerName;
            oldObj.Active = obj.Active;
            oldObj.LineID = obj.LineID;
            oldObj.Address = obj.Address;

            //Thực hiện cập nhật
            var result = _tblLockerControllerService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.Id.ToString(), oldObj.ControllerName, "tblLockerController", ConstField.LockerCode, ActionConfigO.Update);

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
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var result = _tblLockerControllerService.DeleteById(id);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), id, id, "tblLockerController", ConstField.LockerCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        private List<tblLockerLine> GetLineList()
        {
            return _tblLockerLineService.GetAllActive().ToList();
        }
    }
}