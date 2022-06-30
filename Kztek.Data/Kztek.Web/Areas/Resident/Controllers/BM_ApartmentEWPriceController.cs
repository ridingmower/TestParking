using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_ApartmentEWPriceController : Controller
    {
        private IBM_ApartmentEWPriceService _BM_ApartmentEWPriceService;

        public BM_ApartmentEWPriceController(IBM_ApartmentEWPriceService _BM_ApartmentEWPriceService)
        {
            this._BM_ApartmentEWPriceService = _BM_ApartmentEWPriceService;
        }

        [HttpGet]
        public ActionResult Index(string group = "")
        {
            ViewBag.GroupID = group;

            var obj = _BM_ApartmentEWPriceService.GetDefault();

            if (obj == null)
            {
                obj = new BM_ApartmentEWPrice();
                obj.Id = Common.GenerateId();
                obj.Electricity_Price = 0;
                obj.Water_Price = 0;
                obj.DateCreated = DateTime.Now;
                obj.IsDeleted = false;
                obj.Date_Price = DateTime.Now;

                _BM_ApartmentEWPriceService.Create(obj);
            }

            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(BM_ApartmentEWPrice obj,string txtWPrice,string txtEPrice,string dtpDate, string group = "")
        {
            ViewBag.GroupID = group;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _BM_ApartmentEWPriceService.GetDefault();
            if (oldObj == null)
            {
                oldObj = new BM_ApartmentEWPrice();

                _BM_ApartmentEWPriceService.Create(oldObj);
            }

            //Gán
            oldObj.Electricity_Price = !string.IsNullOrEmpty(txtEPrice) ? Convert.ToDecimal(txtEPrice.Replace(",", "").Replace(".", "")) : 0;
            oldObj.Water_Price = !string.IsNullOrEmpty(txtWPrice) ? Convert.ToDecimal(txtWPrice.Replace(",", "").Replace(".", "")) : 0;
            oldObj.Date_Price = !string.IsNullOrEmpty(dtpDate) ? Convert.ToDateTime(dtpDate) : DateTime.Now;

            var result = _BM_ApartmentEWPriceService.Update(oldObj);
            if (result.isSuccess)
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.SystemConfigSession, host)] = null;

                TempData["Success"] = result.Message;
                return View(oldObj);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
    }
}