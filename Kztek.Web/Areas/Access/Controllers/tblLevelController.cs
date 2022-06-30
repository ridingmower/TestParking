using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblLevelController : Controller
    {
        #region Khai báo services

        private ItblAccessLevelService _tblAccessLevelService;
        private ItblAccessLevelDetailService _tblAccessLevelDetailService;
        private ItblAccessDoorService _tblAccessDoorService;
        private ItblAccessTimezoneService _tblAccessTimezoneService;
        private ItblAccessControllerService _tblAccessControllerService;
        private static string url = "";
        private ItblUpdateActionsService _tblUpdateActionsService;
        public tblLevelController(ItblUpdateActionsService _tblUpdateActionsService,ItblAccessLevelService _tblAccessLevelService, ItblAccessLevelDetailService _tblAccessLevelDetailService, ItblAccessDoorService _tblAccessDoorService, ItblAccessTimezoneService _tblAccessTimezoneService, ItblAccessControllerService _tblAccessControllerService)
        {
            this._tblUpdateActionsService = _tblUpdateActionsService;
            this._tblAccessLevelService = _tblAccessLevelService;
            this._tblAccessLevelDetailService = _tblAccessLevelDetailService;
            this._tblAccessDoorService = _tblAccessDoorService;
            this._tblAccessTimezoneService = _tblAccessTimezoneService;
            this._tblAccessControllerService = _tblAccessControllerService;
        }

        #endregion Khai báo services

        #region Danh sách

        /// <summary>
        /// Danh sách phân quyền truy cập
        /// </summary>
        /// <modified>
        /// Author             Date            Comments
        /// TrungNQ            17/09/2017      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>

        [CheckSessionLogin]
        public ActionResult Index(string key = "", int page = 1)
        {
            var pageSize = 20;

            var list = _tblAccessLevelService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblAccessLevel>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;

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
        /// TrungNQ             17/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.hidValueSelectedDoorValue = "";
            ViewBag.hidValueSelectedControllerTimeValue = "";

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             17/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="hidValueSelectedDoor">Danh sách cửa đã chọn + bộ điều khiển của cửa đó</param>
        /// <param name="hidValueSelectedControllerTime">Danh sách bộ điều khiển + timezone</param>
        /// <param name="SaveAndCountinue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblAccessLevel obj, string hidValueSelectedDoor = "", string hidValueSelectedControllerTime = "", bool SaveAndCountinue = false)
        {
            ViewBag.hidValueSelectedDoorValue = hidValueSelectedDoor;
            ViewBag.hidValueSelectedControllerTimeValue = hidValueSelectedControllerTime;

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            obj.AccessLevelID = Guid.NewGuid();

            //Lọc lấy controller + time
            if (!string.IsNullOrWhiteSpace(hidValueSelectedControllerTime) && !string.IsNullOrWhiteSpace(hidValueSelectedDoor))
            {
                //Tách các chuỗi theo dấu cách ,
                var idControllerTimes = hidValueSelectedControllerTime.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (idControllerTimes.Any())
                {
                    foreach (var item in idControllerTimes)
                    {
                        //Lọc lấy dữ liệu với dấu cách -
                        var mapObj = item.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        if (mapObj.Any() && mapObj.Length > 1)
                        {
                            var newObj = new tblAccessLevelDetail()
                            {
                                AccessLevelID = obj.AccessLevelID.ToString(),
                                ControllerID = mapObj[0].ToString(),
                                TimezoneID = mapObj[1].ToString(),
                                DoorIndexes = "",
                            };

                            //Lọc lấy danh sách door có dấu cách ,
                            var idDoor = hidValueSelectedDoor.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idDoor.Any())
                            {
                                var count = 0;

                                //var listDoor = "";
                                foreach (var item1 in idDoor.Where(n => n.Contains(newObj.ControllerID)))
                                {
                                    //if (item1.Contains(newObj.ControllerID))
                                    //{

                                    //}
                                    count++;

                                    var mapObj1 = item1.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (mapObj1.Any() && mapObj1.Length > 1)
                                    {
                                        newObj.DoorIndexes += string.Format("{0}{1}", mapObj1[1].ToString(), idDoor.Where(n => n.Contains(newObj.ControllerID)).Count() == count ? "" : ";");
                                    }
                                }

                                //Save vào bản detail
                                _tblAccessLevelDetailService.Create(newObj);
                            }
                        }
                    }
                }
            }

            //Thực hiện thêm mới
            var result = _tblAccessLevelService.Create(obj);
            if (result.isSuccess)
            {

                var objActions = _tblUpdateActionsService.GetAll().ToList();
                if ( objActions.Count > 0)
                {
                    // update
                    foreach (var item in objActions)
                    {
                        item.IsUpdateLevel = true;
                        _tblUpdateActionsService.Update(item);
                    }

                }
                else
                {
                    var objAction = new tblUpdateActions();
                    objAction.Id = Guid.NewGuid().ToString();
                    objAction.IsUpdateLevel = true;
                    _tblUpdateActionsService.Create(objAction);
                }
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.AccessLevelID.ToString(), obj.AccessLevelName, "tblAccessLevel", ConstField.AccessControlCode, ActionConfigO.Create);

                TempData["Success"] = result.Message;

                if (SaveAndCountinue)
                {
                    return RedirectToAction("Create");
                }

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
        /// Author              Date            Comments
        /// TrungNQ             18/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Update(string id)
        {
            var selectedControllerId = new List<string>();
            var selectedDoors = "";

            var obj = _tblAccessLevelService.GetById(Guid.Parse(id));

            if (obj != null)
            {
                //Danh sách custom
                var listCustomLevelDetail = new List<tblAccessLevelDetailCustom>();

                var details = _tblAccessLevelDetailService.GetAllByLevelId(obj.AccessLevelID.ToString());
                if (details.Any())
                {
                    foreach (var item in details)
                    {
                        selectedControllerId.Add(item.ControllerID);

                        var kl = item.DoorIndexes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var itemKL in kl)
                        {
                            selectedDoors += item.ControllerID + "#" + itemKL + ";";

                            //Gán lại vào trong customer để chỉ hiển thị 1 time zone + 1 bđk
                            var existed = listCustomLevelDetail.FirstOrDefault(n => n.ControllerID == item.ControllerID);
                            if (existed == null)
                            {
                                var modelC = new tblAccessLevelDetailCustom()
                                {
                                    Id = item.Id,
                                    ControllerID = item.ControllerID,
                                    AccessLevelID = item.AccessLevelID,
                                    DoorIndexes = "",
                                    TimezoneID = item.TimezoneID
                                };

                                listCustomLevelDetail.Add(modelC);
                            }
                        }
                    }
                }


                ViewBag.ListLevelDetail = listCustomLevelDetail;
            }

            ViewBag.ListTimeZone = _tblAccessTimezoneService.GetAllActive();
            ViewBag.ListController = _tblAccessControllerService.GetAllByListId(selectedControllerId);
            ViewBag.DoorIndexes = selectedDoors;

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             18/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">id bản ghi</param>
        /// <param name="hidValueSelectedDoor">Danh sách cửa đã chọn + bộ điều khiển của cửa đó</param>
        /// <param name="hidValueSelectedControllerTime">Danh sách bộ điều khiển + timezone</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(tblAccessLevel obj, string hidValueSelectedDoor = "", string hidValueSelectedControllerTime = "", int pageNumber = 1)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            var oldObj = _tblAccessLevelService.GetById(obj.AccessLevelID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Lọc lấy controller + time
            if (!string.IsNullOrWhiteSpace(hidValueSelectedControllerTime) || !string.IsNullOrWhiteSpace(hidValueSelectedDoor))
            {
                var details = _tblAccessLevelDetailService.GetAllByLevelId(obj.AccessLevelID.ToString());
                if (details.Any())
                {
                    foreach (var item in details)
                    {
                        _tblAccessLevelDetailService.DeleteById(item.Id);
                    }
                }

                //Tách các chuỗi theo dấu cách ,
                var idControllerTimes = hidValueSelectedControllerTime.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (idControllerTimes.Any())
                {
                    foreach (var item in idControllerTimes)
                    {
                        //Lọc lấy dữ liệu với dấu cách -
                        var mapObj = item.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        if (mapObj.Any() && mapObj.Length > 1)
                        {
                            var newObj = new tblAccessLevelDetail()
                            {
                                AccessLevelID = obj.AccessLevelID.ToString(),
                                ControllerID = mapObj[0].ToString(),
                                TimezoneID = mapObj[1].ToString(),
                                DoorIndexes = "",
                            };

                            //Lọc lấy danh sách door có dấu cách ,
                            var idDoor = hidValueSelectedDoor.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idDoor.Any())
                            {
                                var count = 0;

                                //var listDoor = "";
                                foreach (var item1 in idDoor.Where(n => n.Contains(newObj.ControllerID)))
                                {
                                    //if (item1.Contains(newObj.ControllerID))
                                    //{
                                        
                                    //}
                                    count++;

                                    var mapObj1 = item1.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (mapObj1.Any() && mapObj1.Length > 1)
                                    {
                                        newObj.DoorIndexes += string.Format("{0}{1}", mapObj1[1].ToString(), idDoor.Where(n => n.Contains(newObj.ControllerID)).Count() == count ? "" : ";");
                                    }
                                }

                                //Save vào bản detail
                                _tblAccessLevelDetailService.Create(newObj);
                            }
                        }
                    }
                }
            }

            //Gán giá trị
            oldObj.AccessLevelName = obj.AccessLevelName;
            oldObj.Description = obj.Description;
            oldObj.Inactive = obj.Inactive;

            //Thực hiện cập nhật
            var result = _tblAccessLevelService.Update(oldObj);
            if (result.isSuccess)
            {
                var objActions = _tblUpdateActionsService.GetAll().ToList();
                if ( objActions.Count > 0)
                {
                    // update
                    foreach (var item in objActions)
                    {
                        item.IsUpdateLevel = true;
                        _tblUpdateActionsService.Update(item);
                    }

                }
                else
                {
                    var objAction = new tblUpdateActions();
                    objAction.Id = Guid.NewGuid().ToString();
                    objAction.IsUpdateLevel = true;
                    _tblUpdateActionsService.Create(objAction);
                }
                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.AccessLevelID.ToString(), oldObj.AccessLevelName, "tblAccessLevel", ConstField.AccessControlCode, ActionConfigO.Update);

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
        /// Author              Date            Comments
        /// TrungNQ             17/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var result = _tblAccessLevelService.DeleteById(id);

            if (result.isSuccess)
            {
                var objActions = _tblUpdateActionsService.GetAll().ToList();
                if ( objActions.Count > 0)
                {
                    // update
                    foreach (var item in objActions)
                    {
                        item.IsUpdateLevel = true;
                        _tblUpdateActionsService.Update(item);
                    }

                }
                else
                {
                    var objAction = new tblUpdateActions();
                    objAction.Id = Guid.NewGuid().ToString();
                    objAction.IsUpdateLevel = true;
                    _tblUpdateActionsService.Create(objAction);
                }
                WriteLog.Write(result, GetCurrentUser.GetUser(), id, id, "tblAccessLevel", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        public PartialViewResult ControllerInDoor(List<string> controllerList)
        {
            ViewBag.ListTimeZone = _tblAccessTimezoneService.GetAllActive().ToList();

            var list = _tblAccessControllerService.GetAllByListId(controllerList).ToList();
            return PartialView(list);
        }

        public PartialViewResult ControllerInDoorOnUpdate(List<string> controllerList, string id = "")
        {
            var obj = _tblAccessLevelService.GetById(Guid.Parse(id));

            ViewBag.ListTimeZone = _tblAccessTimezoneService.GetAllActive().ToList();

            ViewBag.ListController = _tblAccessControllerService.GetAllByListId(controllerList);

            return PartialView(obj);
        }

        public PartialViewResult GetListDoor(string selected, string objId = "", bool isUpdateable = false)
        {
            ViewBag.objIdValue = objId;
            ViewBag.Updateable = isUpdateable;

            var listCustom = new List<SelectListModel5>();

            ViewBag.SelectedValues = selected;

            var list = _tblAccessDoorService.GetAllActive();
            if (list.Any())
            {
                var controllers = new List<string>();
                foreach (var item in list)
                {
                    if (!controllers.Contains(item.ControllerID))
                    {
                        controllers.Add(item.ControllerID);
                    }
                }

                var listController = _tblAccessControllerService.GetAllByListId_v4(controllers);
               

                foreach (var item in list)
                {
                    var controllerName = "";
                    if (listController != null && listController.Rows.Count > 0)
                    {
                        var objController = listController.AsEnumerable().FirstOrDefault(n => n["ControllerID"].ToString().Equals(item.ControllerID));

                        controllerName = objController != null ? objController["ControllerName"].ToString() : "";
                    }

                    var obj = new SelectListModel5();
                    obj.ItemText = string.Format("{0} ({1})", item.DoorName, controllerName);
                    obj.ItemValue = !string.IsNullOrEmpty(item.ReaderIndex) ? item.ReaderIndex.ToString() : "";
                    obj.ItemSecondValue = item.ControllerID;
                    listCustom.Add(obj);
                }
            }

            return PartialView(listCustom);
        }
    }
}