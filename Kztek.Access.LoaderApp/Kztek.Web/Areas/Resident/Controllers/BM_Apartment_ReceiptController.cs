using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_Apartment_ReceiptController : Controller
    {
        private IBM_Apartment_ReceiptService _BM_Apartment_ReceiptService;
        private IBM_BuildingService _BM_BuildingService;
        private IBM_FloorService _BM_FloorService;
        private IBM_ApartmentService _BM_ApartmentService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCardService _tblCardService;


        public BM_Apartment_ReceiptController(IBM_Apartment_ReceiptService _BM_Apartment_ReceiptService,IBM_BuildingService _BM_BuildingService,
        ItblSystemConfigService _tblSystemConfigService, ItblCardService _tblCardService, IBM_FloorService _BM_FloorService, IBM_ApartmentService _BM_ApartmentService)
        {
            this._BM_Apartment_ReceiptService = _BM_Apartment_ReceiptService;
            this._BM_BuildingService = _BM_BuildingService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCardService = _tblCardService;
            this._BM_FloorService = _BM_FloorService;
            this._BM_ApartmentService = _BM_ApartmentService;
        }

        #region Danh sách
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", string buildingIdSearch = "",string floorIdSearch = "", string apartmentIdSearch = "", string residentIdSearch = "", string typeSearch = "", string statusSearch = "", string userSearch = "", string userCreatedIdSearch = "", string fromDatePaid = "", string toDatePaid = "", string chkExport = "0", int page = 1, string selectedId = "")
        {
            int pageSize = 20;
            int total = 0;
            var list = _BM_Apartment_ReceiptService.GetAllPagingByFirstTSQL(key, buildingIdSearch,floorIdSearch, apartmentIdSearch, residentIdSearch, typeSearch, statusSearch, userSearch, userCreatedIdSearch, fromDatePaid, toDatePaid, page, pageSize, ref total);
            
            var gridModel = PageModelCustom<BM_Apartment_ReceiptView>.GetPage(list, page, pageSize, total);

            //search value
            ViewBag.keyValue = key;
            ViewBag.buildingIdSearch = buildingIdSearch;
            ViewBag.apartmentIdSearch = apartmentIdSearch;
            ViewBag.residentIdSearch = residentIdSearch;
            ViewBag.typeSearch = typeSearch;
            ViewBag.statusSearch = statusSearch;
            ViewBag.userSearch = userSearch;
            ViewBag.userCreatedIdSearch = userCreatedIdSearch;
            ViewBag.fromDatePaid = fromDatePaid;
            ViewBag.toDatePaid = toDatePaid;

            //list - selectlist
            ViewBag.selectedIdValue = selectedId;
            //gọi bên building Service
            ViewBag.BuildingSelectList = _BM_BuildingService.BuildingIdToDDL();
            ViewBag.FloorSelectList = _BM_FloorService.FloorToDDL(buildingIdSearch);
            ViewBag.ApartmentSelectList = _BM_ApartmentService.ApartmentToDDL(buildingIdSearch, floorIdSearch);
            return View(gridModel);
        }
        #endregion

        #region Thêm mới
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key = "", int page = 1)
        {
            ViewBag.keyValue = key;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BM_Apartment_Receipt obj, bool SaveAndCountinue = false, string key = "")
        {
            ViewBag.keyValue = key;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }


            //Gán giá trị
            obj.Id = Guid.NewGuid().ToString();
            obj.DateCreated = DateTime.UtcNow;
            obj.IsDeleted = false;

            //Thực hiện thêm mới
            var result = _BM_Apartment_ReceiptService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Code, "BM_Apartment_Receipt", ConstField.ResidentCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key });
                }

                return RedirectToAction("Index", new { key = key });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
        #endregion

        #region Cập nhật
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key = "", string residentGroup = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.ResidentGroupValue = residentGroup;
            ViewBag.PN = page;

            var obj = _BM_Apartment_ReceiptService.GetById(id);

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(BM_Apartment_Receipt obj, string key = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.PN = page;

            //Kiểm tra
            var oldObj = _BM_Apartment_ReceiptService.GetById(obj.Id);
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
            oldObj.Code = obj.Code;
            oldObj.Title = obj.Title;
            oldObj.Description = obj.Description;
            oldObj.UserCreatedId = obj.UserCreatedId;
            oldObj.DatePaid = obj.DatePaid;
            oldObj.ApartmentId = obj.ApartmentId;
            oldObj.ResidentId = obj.ResidentId;
            oldObj.PayerName = obj.PayerName;
            oldObj.PayerMobile = obj.PayerMobile;
            oldObj.UserId = obj.UserId;
            oldObj.Money = obj.Money;
            oldObj.PayType = obj.PayType;
            oldObj.Type = obj.Type;
            oldObj.Status = obj.Status;
            oldObj.Note = obj.Note;

            //Thực hiện cập nhật
            var result = _BM_Apartment_ReceiptService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Code, "BM_Apartment_Receipt", ConstField.ResidentCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { page = page, key = key });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }
        #endregion

        #region Xóa
        public JsonResult Delete(string id)
        {
            var obj = new BM_Apartment_Receipt();

            var result = _BM_Apartment_ReceiptService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Code, "BM_Apartment_Receipt", ConstField.ResidentCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload file
        private void UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                string error = "";

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];

                Common.UploadFile(out error, Server.MapPath(url), fileUpload);
            }
        }
        #endregion


        #region SelectList


        #endregion

    }
}