using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_Building_ServiceController : Controller
    {
        #region Khai báo services

        public static string url;
        public static string objId;
        private IBM_Building_ServiceService _BM_Building_ServiceService;


        public BM_Building_ServiceController(IBM_Building_ServiceService _BM_Building_ServiceService)
        {
            this._BM_Building_ServiceService = _BM_Building_ServiceService;

        }

        #endregion Khai báo services
        #region Danh sách

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="key"></param>
        /// <param name="cardgroup"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", int page = 1)
        {
            //var totalPage = 0;
            //var totalItem = 0;
            var pageSize = 20;

            var list = _BM_Building_ServiceService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<BM_Building_Service>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;


            url = Request.Url.AbsoluteUri;

            ViewBag.objId = objId;

            return View(gridModel);
        }

        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new BM_Building_Service
            {
                Day = 0
            };
            ViewBag.ScheduleType = FunctionHelper.ScheduleType();
            return View(model);
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="txtFeeLevel">Phí</param>
        /// <param name="SaveAndCountinue">Tiếp tục hay không</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(BM_Building_Service obj, string txtPrice = "", bool SaveAndCountinue = false)
        {
            ViewBag.ScheduleType = FunctionHelper.ScheduleType();
            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                ModelState.AddModelError("Name", "Vui lòng nhập tên");
                return View(obj);
            }

            var existed = _BM_Building_ServiceService.GetByName(obj.Name);
            if (existed != null)
            {
                ModelState.AddModelError("Name", "Thông tin đã tồn tại");
                return View(obj);
            }


            obj.Id = Common.GenerateId();
            obj.IsDeleted = false;
            obj.DateCreated = DateTime.Now;
            obj.Price = !string.IsNullOrEmpty(txtPrice) ? Convert.ToDecimal(txtPrice.Replace(",", "").Replace(".", "")) : 0;
          

            //Thực hiện thêm mới
            var result = _BM_Building_ServiceService.Create(obj);
            if (result.isSuccess)
            {
                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">ID bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int pageNumber = 1)
        {
            var obj = _BM_Building_ServiceService.GetById(id);
            ViewBag.ScheduleType = FunctionHelper.ScheduleType();
            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="txtFeeLevel">Phí</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(BM_Building_Service obj,string txtPrice, string objId, int pageNumber = 1)
        {
            //Danh sách sử dụng
            ViewBag.ScheduleType = FunctionHelper.ScheduleType();
            //Kiểm tra
            var oldObj = _BM_Building_ServiceService.GetById(objId);
            if (oldObj == null)
            {
                ViewBag.Error = "Thông tin không tồn tại";
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                ModelState.AddModelError("Name", "Vui lòng nhập tên");
                return View(obj);
            }

            var existed = _BM_Building_ServiceService.GetByName(obj.Name);
            if (existed != null && existed.Id != objId)
            {
                ModelState.AddModelError("Name", "Thông tin đã tồn tại");
                return View(obj);
            }

            //Gán giá trị
            oldObj.Name = obj.Name;
            oldObj.Description = obj.Description;
            oldObj.SchedulePay = obj.SchedulePay;
            oldObj.ScheduleType = obj.ScheduleType;
            oldObj.Day = obj.Day;
            oldObj.Price = !string.IsNullOrEmpty(txtPrice) ? Convert.ToDecimal(txtPrice.Replace(",", "").Replace(".", "")) : 0;

            //Thực hiện cập nhật
            var result = _BM_Building_ServiceService.Update(oldObj);
            if (result.isSuccess)
            {

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
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
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var obj = new Model.Models.BM_Building_Service();

            var result = _BM_Building_ServiceService.DeleteById(id, ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}