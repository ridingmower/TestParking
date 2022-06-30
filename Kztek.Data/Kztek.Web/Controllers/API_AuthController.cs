using Kztek.Model.Models.API;
using Kztek.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class API_AuthController : Controller
    {
        private IAPI_AuthService _API_AuthService;

        public API_AuthController(IAPI_AuthService _API_AuthService)
        {
            this._API_AuthService = _API_AuthService;
        }

        [HttpGet]
        public ActionResult Index(string group = "")
        {
            var model = _API_AuthService.GetDefault();
            ViewBag.GroupID = group;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(API_Auth obj, string group = "")
        {
            ViewBag.GroupID = group;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _API_AuthService.GetDefault();
            if (oldObj != null)
            {
                oldObj.AccessToken = obj.AccessToken;
            }

            var result = _API_AuthService.Update(oldObj);
            if (result.isSuccess)
            {
                TempData["Success"] = result.Message;
                return View(obj);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
    }
}