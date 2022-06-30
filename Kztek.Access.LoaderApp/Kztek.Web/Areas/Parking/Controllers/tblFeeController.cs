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

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblFeeController : Controller
    {
        #region Khai báo services

        public static string url;
        public static string objId;
        private ItblFeeService _tblFeeService;
        private ItblCardGroupService _tblCardGroupService;

        public tblFeeController(ItblFeeService _tblFeeService, ItblCardGroupService _tblCardGroupService)
        {
            this._tblFeeService = _tblFeeService;
            this._tblCardGroupService = _tblCardGroupService;
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
        public ActionResult Index(string key = "", string cardgroup = "", int page = 1)
        {
            //var totalPage = 0;
            //var totalItem = 0;
            var pageSize = 20;

            var list = _tblFeeService.GetAllCustomPagingByFirst(key, cardgroup, page, pageSize);

            var gridModel = PageModelCustom<FeeCustom>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            ViewBag.CardGroupValue = cardgroup;
            ViewBag.CardGroups = GetCardGroupList();

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
            ViewBag.CardGroups = GetCardGroupList();
            ViewBag.TimePeriodTypes = GetTimeList();

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
        public ActionResult Create(tblFee obj, string txtFeeLevel = "", string unit = "", string period = "", bool SaveAndCountinue = false)
        {
            //Danh sách sử dụng
            ViewBag.CardGroups = GetCardGroupList();
            ViewBag.TimePeriodTypes = GetTimeList();

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.FeeName))
            {
                ModelState.AddModelError("FeeName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(obj);
            }

            var existed = _tblFeeService.GetByName(obj.FeeName);
            if (existed != null)
            {
                ModelState.AddModelError("FeeName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["exists_name"]);
                return View(obj);
            }

            //Gán giá trịs

            if (!string.IsNullOrWhiteSpace(txtFeeLevel))
            {
                txtFeeLevel = txtFeeLevel.Replace(".", "");
                obj.FeeLevel = Convert.ToInt32(txtFeeLevel);
            }

            if (!string.IsNullOrWhiteSpace(unit) && !string.IsNullOrWhiteSpace(period))
            {
                obj.Units = $"{unit}_{period}";
            }

            //Thực hiện thêm mới
            var result = _tblFeeService.Create(obj);
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
            var obj = _tblFeeService.GetById(Convert.ToInt32(id));

            objId = obj.FeeID.ToString();

            ViewBag.CardGroups = GetCardGroupList();
            ViewBag.TimePeriodTypes = GetTimeList();
            ViewBag.PN = pageNumber;

            var _unitInfo = obj.Units.Split('_');
            if (_unitInfo.Count() == 2)
            {
                ViewBag.Unit = _unitInfo[0];
                ViewBag.Period = _unitInfo[1];
            }
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
        public ActionResult Update(tblFee obj, string txtFeeLevel, string objId, string unit = "", string period = "", int pageNumber = 1)
        {
            //Danh sách sử dụng
            ViewBag.CardGroups = GetCardGroupList();
            ViewBag.TimePeriodTypes = GetTimeList();
            ViewBag.PN = pageNumber;
           

            //Kiểm tra
            var oldObj = _tblFeeService.GetById(Convert.ToInt32(objId));
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["card_does_not_exist_in_the_system"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.FeeName))
            {
                ModelState.AddModelError("FeeName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(oldObj);
            }

            var existed = _tblFeeService.GetByName_Id(obj.FeeName, Convert.ToInt32(objId)); ;
            if (existed != null)
            {
                ModelState.AddModelError("FeeName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["exists_name"]);
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.CardGroupID = obj.CardGroupID;
            oldObj.FeeName = obj.FeeName;
            oldObj.IsUseExtend = obj.IsUseExtend;
            //oldObj.Units = obj.Units;

            if (!string.IsNullOrWhiteSpace(txtFeeLevel))
            {
                txtFeeLevel = txtFeeLevel.Replace(".", "");
                oldObj.FeeLevel = Convert.ToInt32(txtFeeLevel);
            }

            if (!string.IsNullOrWhiteSpace(unit) && !string.IsNullOrWhiteSpace(period))
            {
                oldObj.Units = $"{unit}_{period}";
            }

            //Thực hiện cập nhật
            var result = _tblFeeService.Update(oldObj);
            if (result.isSuccess)
            {
                objId = oldObj.FeeID.ToString();

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
            var obj = new tblFee();

            var result = _tblFeeService.DeleteById(Convert.ToInt32(id), ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        private IEnumerable<tblCardGroup> GetCardGroupList()
        {
            //Thuê bao
            return _tblCardGroupService.GetAllActiveByType(0);
        }

        private List<SelectListModel> GetTimeList()
        {
            return FunctionHelper.TimePeriodType();
        }
    }
}