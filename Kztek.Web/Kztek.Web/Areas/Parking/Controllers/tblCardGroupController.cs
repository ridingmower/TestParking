using Kztek.Model.CustomModel;
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

namespace Kztek.Web.Areas.Parking.Controllers
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
            ViewBag.CardTypes = GetCardType();
            ViewBag.VehicleGroups = GetVehicleGroup();
            ViewBag.Formulation = GetFormulation();
            ViewBag.Url = url;

            var obj = new tblCardGroup();
            obj.DayTimeFrom = "00:00";
            obj.DayTimeTo = "00:00";
            obj.CardType = 0;
            obj.VehicleGroupID = "";
            obj.Description = "";
            obj.EnableFree = false;
            obj.Inactive = false;
            obj.IsCheckPlate = false;
            obj.IsHaveMoneyExcessTime = false;
            obj.IsHaveMoneyExpiredDate = false;
            obj.LaneIDs = "";
            
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
        public ActionResult Create(tblCardGroup obj, string listLanes = "", string selectValueBlockTime = "", string txtEachFee = "", bool SaveAndCountinue = false)
        {
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            //Danh sách sử dụng
            ViewBag.CardTypes = GetCardType();
            ViewBag.VehicleGroups = GetVehicleGroup();
            ViewBag.Formulation = GetFormulation();
            ViewBag.Url = url;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if(string.IsNullOrEmpty(obj.CardGroupName))
            {
                ModelState.AddModelError("CardGroupName", DictionaryAction["enter_CardGrp_name"]);
                return View(obj);
            }

            if (string.IsNullOrEmpty(listLanes))
            {
                ModelState.AddModelError("LaneIDs", DictionaryAction["CardGrp_lane"]);
                return View(obj);
            }


            var cgroup = _tblCardGroupService.GetByName(obj.CardGroupName);

            if (cgroup != null)
            {
                ModelState.AddModelError("CardGroupName", DictionaryAction["CardGrp_already_exists"]);
                return View(obj);
            }

            //Gán giá trị
            obj.CardGroupID = Guid.NewGuid();
          
            obj.LaneIDs = listLanes;

            if (!string.IsNullOrWhiteSpace(txtEachFee))
            {
                txtEachFee = txtEachFee.Replace(".", "");
                obj.EachFee = Convert.ToInt32(txtEachFee);
            }

            if (string.IsNullOrEmpty(obj.TimePeriods))
            {
                obj.TimePeriods = "00:00-00:00-00:00";
            }

            if (string.IsNullOrEmpty(obj.Costs))
            {
                obj.Costs = "0";
            }

            #region Block and time
            if (!string.IsNullOrWhiteSpace(selectValueBlockTime))
            {
                var arr = selectValueBlockTime.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Any())
                {
                    for(int i = 0; i < arr.Length; i++)
                    {
                        var objBT = arr[i].Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                        if(objBT.Any())
                        {
                            var block = !string.IsNullOrEmpty(objBT[0].ToString()) ? Convert.ToInt32(objBT[0].ToString().Replace(".", "")) : 0;
                            var time = !string.IsNullOrEmpty(objBT[1].ToString()) ? Convert.ToInt32(objBT[1].ToString()) : 0;

                            switch (i)
                            {
                                case 0:
                                    obj.Block0 = block;
                                    obj.Time0 = time;
                                    break;
                                case 1:
                                    obj.Block1 = block;
                                    obj.Time1 = time;
                                    break;
                                case 2:
                                    obj.Block2 = block;
                                    obj.Time2 = time;
                                    break;
                                case 3:
                                    obj.Block3 = block;
                                    obj.Time3 = time;
                                    break;
                                case 4:
                                    obj.Block4 = block;
                                    obj.Time4 = time;
                                    break;
                                case 5:
                                    obj.Block5 = block;
                                    obj.Time5 = time;
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

          
            //Thực hiện thêm mới
            var result = _tblCardGroupService.Create(obj);
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
        /// TrungNQ                 22/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1)
        {
            ViewBag.CardTypes = GetCardType();
            ViewBag.VehicleGroups = GetVehicleGroup();
            ViewBag.Formulation = GetFormulation();
            //ViewBag.PN = pageNumber;
            //ViewBag.Group = groupId;

            var obj = _tblCardGroupService.GetById(Guid.Parse(id));

            TempData["obj"] = obj;

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
        public ActionResult Update(tblCardGroup obj, string id, string listLanes = "", string selectValueBlockTime = "", string txtEachFee = "")
        {
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            ViewBag.CardTypes = GetCardType();
            ViewBag.VehicleGroups = GetVehicleGroup();
            ViewBag.Formulation = GetFormulation();
            //ViewBag.PN = pageNumber;
            //ViewBag.Group = groupId;

            //Kiểm tra
            var oldObj = _tblCardGroupService.GetById(Guid.Parse(id));
            if (oldObj == null)
            {
                ViewBag.Error = DictionaryAction["record_does_not_exist"];
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            if (string.IsNullOrEmpty(obj.CardGroupName))
            {
                ModelState.AddModelError("CardGroupName", DictionaryAction["enter_CardGrp_name"]);
                return View(oldObj);
            }

            if (string.IsNullOrEmpty(listLanes))
            {
                ModelState.AddModelError("LaneIDs", DictionaryAction["CardGrp_lane"]);
                return View(oldObj);
            }


            var cgroup = _tblCardGroupService.GetByName(obj.CardGroupName);

            if (cgroup != null && !cgroup.CardGroupID.Equals(Guid.Parse(id)))
            {
                ModelState.AddModelError("CardGroupName", DictionaryAction["CardGrp_already_exists"]);
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.CardType = obj.CardType;
            oldObj.FreeTime = obj.FreeTime;
            oldObj.DayTimeFrom = obj.DayTimeFrom;
            oldObj.DayTimeTo = obj.DayTimeTo;
            oldObj.Description = obj.Description;
            oldObj.EachFee = obj.EachFee;
            oldObj.EnableFree = obj.EnableFree;
            oldObj.Formulation = obj.Formulation;
            oldObj.Inactive = obj.Inactive;
            oldObj.IsCheckPlate = obj.IsCheckPlate;
            oldObj.IsHaveMoneyExcessTime = obj.IsHaveMoneyExcessTime;
            oldObj.IsHaveMoneyExpiredDate = obj.IsHaveMoneyExpiredDate;
            oldObj.CardGroupName = obj.CardGroupName;
            oldObj.SortOrder = obj.SortOrder;
            oldObj.VehicleGroupID = obj.VehicleGroupID;
            oldObj.LaneIDs = listLanes;
            oldObj.RestrictedNumber = obj.RestrictedNumber;
            oldObj.IsSpecialGroup = obj.IsSpecialGroup;
            oldObj.EachFee = obj.EachFee;

            if (!string.IsNullOrWhiteSpace(obj.TimePeriods))
            {
                oldObj.TimePeriods = obj.TimePeriods;
            }

            if (!string.IsNullOrWhiteSpace(obj.Costs))
            {
                oldObj.Costs = obj.Costs;
            }

            #region Block and time
            if (!string.IsNullOrWhiteSpace(selectValueBlockTime))
            {
                var arr = selectValueBlockTime.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Any())
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var objBT = arr[i].Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                        if (objBT.Any())
                        {
                            var block = !string.IsNullOrEmpty(objBT[0].ToString()) ? Convert.ToInt32(objBT[0].ToString().Replace(".", "")) : 0;
                            var time = !string.IsNullOrEmpty(objBT[1].ToString()) ? Convert.ToInt32(objBT[1].ToString()) : 0;

                            switch (i)
                            {
                                case 0:
                                    oldObj.Block0 = block;
                                    oldObj.Time0 = time;
                                    break;
                                case 1:
                                    oldObj.Block1 = block;
                                    oldObj.Time1 = time;
                                    break;
                                case 2:
                                    oldObj.Block2 = block;
                                    oldObj.Time2 = time;
                                    break;
                                case 3:
                                    oldObj.Block3 = block;
                                    oldObj.Time3 = time;
                                    break;
                                case 4:
                                    oldObj.Block4 = block;
                                    oldObj.Time4 = time;
                                    break;
                                case 5:
                                    oldObj.Block5 = block;
                                    oldObj.Time5 = time;
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrWhiteSpace(txtEachFee))
            {
                txtEachFee = txtEachFee.Replace(".", "");
                oldObj.EachFee = Convert.ToInt32(txtEachFee);
            }

            //Thực hiện cập nhật
            var result = _tblCardGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                // UpdateCardCustomer(oldObj._id.ToString(), oldObj);

                objId = oldObj.CardGroupID.ToString();

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

            if(TempData["obj"] != null)
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