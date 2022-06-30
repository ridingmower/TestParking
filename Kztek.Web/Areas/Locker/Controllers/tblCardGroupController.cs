using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class tblCardGroupController : Controller
    {
        public static string url;
        public static string objId;
        private ItblCardGroupService _tblCardGroupService;
        private ItblLaneService _tblLaneService;
        private ItblVehicleGroupService _tblVehicleGroupService;

        public tblCardGroupController(ItblCardGroupService _tblCardGroupService, ItblLaneService _tblLaneService, ItblVehicleGroupService _tblVehicleGroupService)
        {
            this._tblCardGroupService = _tblCardGroupService;
            this._tblLaneService = _tblLaneService;
            this._tblVehicleGroupService = _tblVehicleGroupService;
        }

        /// <summary>
        /// Danh sách nhóm thẻ
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             22/09/2017      Tạo mới
        /// </modified>
        /// <param name="key"> Từ khóa </param>
        /// <param name="page"> Trang hiện tại </param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            var pageSize = 20;

            var list = _tblCardGroupService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblCardGroup>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;

            url = Request.Url.AbsoluteUri;

            ViewBag.objId = objId;

            return View(gridModel);
        }

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 22/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Url = url;

            var obj = new tblCardGroup();

            return View(obj);
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 22/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="listLanes">Danh sách làn</param>
        /// <param name="selectValueBlockTime">Danh sách block time</param>
        /// <param name="txtEachFee">Tiền mỗi lượt</param>
        /// <param name="SaveAndCountinue">Tiếp tục hay không ?</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblCardGroup obj, bool SaveAndCountinue = false)
        {
            //Danh sách sử dụng
            ViewBag.Url = url;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.CardGroupName))
            {
                ModelState.AddModelError("CardGroupName", "Vui lòng nhập tên nhóm thẻ");
                return View(obj);
            }

            var cgroup = _tblCardGroupService.GetByName(obj.CardGroupName);

            if (cgroup != null)
            {
                ModelState.AddModelError("CardGroupName", "Nhóm thẻ đã tồn tại");
                return View(obj);
            }

            //Gán giá trị
            obj.CardGroupID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblCardGroupService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CardGroupID.ToString(), obj.CardGroupName, "tblCardGroup", ConstField.LockerCode, ActionConfigO.Create);

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
        /// TrungNQ                 22/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id)
        {
            var obj = _tblCardGroupService.GetById(Guid.Parse(id));

            objId = obj.CardGroupID.ToString();

            return View(obj);
        }
        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 22/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="listLanes">Danh sách làn</param>
        /// <param name="selectValueBlockTime">Danh sách block time</param>
        /// <param name="txtEachFee">Phí mỗi lượt</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        [HttpPost]
        public ActionResult Update(tblCardGroup obj)
        {
            //Kiểm tra
            var oldObj = _tblCardGroupService.GetById(obj.CardGroupID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            if (string.IsNullOrEmpty(obj.CardGroupName))
            {
                ModelState.AddModelError("CardGroupName", "Vui lòng nhập tên nhóm thẻ");
                return View(oldObj);
            }

            var cgroup = _tblCardGroupService.GetByName(obj.CardGroupName);

            if (cgroup != null && !cgroup.CardGroupID.Equals(obj.CardGroupID))
            {
                ModelState.AddModelError("CardGroupName", "Nhóm thẻ đã tồn tại");
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.Description = obj.Description;
            oldObj.Inactive = obj.Inactive;
            oldObj.CardGroupName = obj.CardGroupName;
            oldObj.SortOrder = obj.SortOrder;

            if (!string.IsNullOrWhiteSpace(obj.TimePeriods))
            {
                oldObj.TimePeriods = obj.TimePeriods;
            }

            if (!string.IsNullOrWhiteSpace(obj.Costs))
            {
                oldObj.Costs = obj.Costs;
            }

            //Thực hiện cập nhật
            var result = _tblCardGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                objId = oldObj.CardGroupID.ToString();

                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CardGroupID.ToString(), obj.CardGroupName, "tblCardGroup", ConstField.LockerCode, ActionConfigO.Update);

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

        #endregion Cập nhật

        public JsonResult Delete(string id)
        {
            var obj = new tblCardGroup();

            var result = _tblCardGroupService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CardGroupID.ToString(), obj.CardGroupName, "tblCardGroup", ConstField.LockerCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult LaneByCardGroup(string laneSelected)
        {
            var list = _tblLaneService.GetAllActive();

            ViewBag.Selected = laneSelected;

            return PartialView(list);
        }

        public PartialViewResult FormulationEachTurn(int value = 0)
        {
            ViewBag.ValueText = value;
            return PartialView();
        }

        public PartialViewResult FormulationBlock(string value = "")
        {
            var _obj = new tblCardGroup();

            if (TempData["obj"] != null)
            {
                _obj = (tblCardGroup)TempData["obj"];
            }

            return PartialView(_obj);
        }

        public PartialViewResult FormulationNewBlock(int numberIndex = 0)
        {
            ViewBag.Index = numberIndex;
            return PartialView();
        }

        public PartialViewResult FormulationTimePeriod(string period = "", string cost = "")
        {
            ViewBag.TimePeriod = period;
            ViewBag.CostValue = cost;

            return PartialView();
        }

        private List<SelectListModel> GetCardType()
        {
            return FunctionHelper.CardTypes();
        }

        private IEnumerable<tblVehicleGroup> GetVehicleGroup()
        {
            return _tblVehicleGroupService.GetAllActive();
        }

        private List<SelectListModel> GetFormulation()
        {
            return FunctionHelper.FormulationList();
        }
    }
}