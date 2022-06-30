using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service;
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

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ReduceParkingFeesController : Controller
    {
        private IReduceParkingFeesService _ReduceParkingFeesService;
        public ReduceParkingFeesController(IReduceParkingFeesService _ReduceParkingFeesService)
        {
            this._ReduceParkingFeesService = _ReduceParkingFeesService;
        }
        // GET: Parking/ReduceParkingFees
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            var pageSize = 20;

            var list = _ReduceParkingFeesService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblDiscountParking>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;

            //url = Request.Url.AbsoluteUri;

            //ViewBag.objId = objId;

            return View(gridModel);
        }


        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(tblDiscountParking obj)
        {
            ViewBag.DiscountModes = FunctionHelper.DiscountModes();
            var model = obj != null ? obj : null;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(tblDiscountParking obj, string key, bool SaveAndCountinue = false)
        {
            ViewBag.DiscountModes = FunctionHelper.DiscountModes();
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            if (obj.DiscountMode == 0)
            {
                if (obj.DiscountAmount < 0)
                {
                    obj.DiscountAmount = 0;
                }
                else if (obj.DiscountAmount > 100)
                {
                    obj.DiscountAmount = 100;
                }
            }

            if (string.IsNullOrWhiteSpace(obj.DiscountAmount.ToString()) || Convert.ToDouble(obj.DiscountAmount) <= 0)
            {
                obj.DiscountAmount = 0;
            }
         
            obj.Id = Guid.NewGuid().ToString();

            //Thực hiện thêm mới
            var result = _ReduceParkingFeesService.Create(obj);
            if (result.isSuccess)
            {
                //Log for hệ thống

                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.DCTypeCode.Trim(), "ReduceParkingFees", ConstField.ParkingCode, ActionConfigO.Create);




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


        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key, int page = 1)
        {
            ViewBag.DiscountModes = FunctionHelper.DiscountModes();
            ViewBag.keyValue = key;

            ViewBag.PN = page;
            var obj = _ReduceParkingFeesService.GetCustomById((id));
            if (obj != null)
            {

            }
                return View(obj);
        }
        [HttpPost]
        public ActionResult Update(tblDiscountParking_Submit obj, string key, int page = 1)
        {
            ViewBag.DiscountModes = FunctionHelper.DiscountModes();
            ViewBag.keyValue = key;


            ViewBag.PN = page;

            var oldObj = _ReduceParkingFeesService.GetById(( obj.Id));

            if (oldObj == null)
            {
                return View(obj);
            }
            if (obj.DiscountMode == 0)
            {
                if ( Convert.ToDouble( obj.AmountReduced ) < 0)
                {
                    obj.AmountReduced = "0";
                }
                else if (Convert.ToDouble( obj.AmountReduced ) > 100)
                {
                    obj.AmountReduced = "100";
                }
                if (obj.AmountReduced == null)
                {
                    obj.AmountReduced = "0";
                }
            }
           
            //Gán giá trị
            var result = new MessageReport();

            if (oldObj != null)
            {

                oldObj.Id = obj.Id;
                oldObj.DCTypeName = obj.NameDiscountType;
                oldObj.DCTypeCode = obj.CodeDiscountType;
                oldObj.DiscountMode = Convert.ToInt32(obj.DiscountMode);
                oldObj.DiscountAmount = Convert.ToDecimal( obj.AmountReduced);
                oldObj.Note = obj.Note;
                oldObj.Priority = Convert.ToInt16( obj.Priority);
                result = _ReduceParkingFeesService.Update(oldObj);
            }

            if (result.isSuccess)
            {

                //Log for hệ thống
                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.Id.ToString(), oldObj.DCTypeCode.Trim(), "ReduceParkingFees", ConstField.ParkingCode, ActionConfigO.Update);
                return RedirectToAction("Index", new { key = key, page = page });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblDiscountParking();

            var result = _ReduceParkingFeesService.DeleteById(id, ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}