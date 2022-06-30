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
    public class BM_ApartmentRoleController : Controller
    {
        #region Khai báo services

        public static string url;
        public static string objId;
        private IBM_ApartmentRoleService _BM_ApartmentRoleService;
      

        public BM_ApartmentRoleController(IBM_ApartmentRoleService _BM_ApartmentRoleService)
        {
            this._BM_ApartmentRoleService = _BM_ApartmentRoleService;
          
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

            var list = _BM_ApartmentRoleService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<BM_ApartmentRole>.GetPage(list, page, pageSize);

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
            return View();
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
        public ActionResult Create(BM_ApartmentRole obj, string txtFeeLevel = "", string unit = "", string period = "", bool SaveAndCountinue = false)
        {
         
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

            obj.Id = Common.GenerateId();
            obj.IsDeleted = false;
            obj.DateCreated = DateTime.Now;

            var existed = _BM_ApartmentRoleService.GetByName(obj.Name);
            if (existed != null)
            {
                ModelState.AddModelError("Name", "Thông tin đã tồn tại");
                return View(obj);
            }

     
            //Thực hiện thêm mới
            var result = _BM_ApartmentRoleService.Create(obj);
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
            var obj = _BM_ApartmentRoleService.GetById(id);

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
        public ActionResult Update(BM_ApartmentRole obj, string objId, int pageNumber = 1)
        {
            //Danh sách sử dụng
         
            //Kiểm tra
            var oldObj = _BM_ApartmentRoleService.GetById(objId);
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

            var existed = _BM_ApartmentRoleService.GetByName(obj.Name);
            if (existed != null && existed.Id != objId)
            {
                ModelState.AddModelError("Name", "Thông tin đã tồn tại");
                return View(obj);
            }

            //Gán giá trị
            oldObj.Name = obj.Name;
            oldObj.Description = obj.Description;
                  

            //Thực hiện cập nhật
            var result = _BM_ApartmentRoleService.Update(oldObj);
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
            var obj = new BM_ApartmentRole();

            var result = _BM_ApartmentRoleService.DeleteById(id, ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}