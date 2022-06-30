using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.CustomModel.iLocker;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class UploadController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;

        private ItblLockerPCService _tblLockerPCService;
        private ItblLockerSelfHostService _tblLockerSelfHostService;
        private ItblLockerLineService _tblLockerLineService;
        private ItblLockerProcessService _tblLockerProcessService;
        private ItblLockerControllerService _tblLockerControllerService;
        private ItblLockerService _tblLockerService;

        public UploadController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerService _tblCustomerService, ItblCustomerGroupService _tblCustomerGroupService, ItblLockerPCService _tblLockerPCService, ItblLockerSelfHostService _tblLockerSelfHostService, ItblLockerLineService _tblLockerLineService, ItblLockerControllerService _tblLockerControllerService, ItblLockerService _tblLockerService, ItblLockerProcessService _tblLockerProcessService)
        {
            this._tblCardService = _tblCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCustomerGroupService = _tblCustomerGroupService;

            this._tblLockerPCService = _tblLockerPCService;
            this._tblLockerSelfHostService = _tblLockerSelfHostService;
            this._tblLockerLineService = _tblLockerLineService;
            this._tblLockerControllerService = _tblLockerControllerService;
            this._tblLockerService = _tblLockerService;
            this._tblLockerProcessService = _tblLockerProcessService;
        }

        #region Phần xử lý nạp hủy thẻ, tủ đồ
        // GET: Locker/Upload
        [CheckSessionLogin]
        public ActionResult Index()
        {
            var model = new UploadModel()
            {
                TaskViewId = Common.GenerateId(),
                tblLockerLines = GetListLockerLine(),
                tblLockerControllers = GetListLockerController(),
                CardGroupDT = GetListCardGroupDT(),
                CustomerGroupDT = GetListCustomerGroupDT()
            };

            return View(model);
        }

        //Xử lý với bộ điều khiển
        #region Controller
        public PartialViewResult PartialListController(List<Kztek.Model.Models.tblLockerController> data = null, string lineid = "", string taskid = "")
        {
            //Dữ liệu
            var list = new List<Kztek.Model.Models.tblLockerController>();

            if (data == null)
            {
                list = _tblLockerControllerService.GetAllActiveByLineId(lineid).ToList();
            }
            else
            {
                list = data;
            }

            //
            var model = new LockerControllerModel()
            {
                Data = list,
                Selected = GetSetDataControllerSession(taskid, "", "0")
            };

            return PartialView(model);
        }

        public List<Kztek.Model.Models.tblLockerController> GetSetDataControllerSession(string taskid, string controllerid = "", string actionV = "0")
        {
            var host = Request.Url.Host;

            var data = (List<Kztek.Model.Models.tblLockerController>)Session[string.Format("{0}_{1}_{2}", SessionConfig.ControllerLockerSession, host, taskid)];

            if (data == null)
            {
                data = new List<Model.Models.tblLockerController>();
            }

            switch (actionV)
            {
                case "1": //Thêm mới

                    if (!data.Any(n => n.Id == controllerid))
                    {
                        var objController = _tblLockerControllerService.GetById(controllerid);
                        if (objController != null)
                        {
                            data.Add(objController);
                        }
                    }

                    break;

                case "2": //Xóa

                    var objControlleDelete = data.FirstOrDefault(n => n.Id == controllerid);
                    if (objControlleDelete != null)
                    {
                        data.Remove(objControlleDelete);
                    }

                    break;

                case "3": //Xóa hết

                    data = new List<Model.Models.tblLockerController>();

                    break;

                default:
                    break;
            }

            //Gán lại vào session
            Session[string.Format("{0}_{1}_{2}", SessionConfig.ControllerLockerSession, host, taskid)] = data;

            return data;
        }

        public JsonResult LockerControllerSelected(List<string> listId, bool isAdd = false, string taskid = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                foreach (var item in listId)
                {
                    GetSetDataControllerSession(taskid, item, isAdd ? "1" : "2");
                }

                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Xử lý với selfhost
        #region SelfHost
        public PartialViewResult PartialListSelfHost(string taskid = "")
        {
            var data = GetSetDataSelfHostSession(taskid);

            return PartialView(data);
        }

        public List<tblLockerSelfHost> GetSetDataSelfHostSession(string taskid = "")
        {
            var data = new List<tblLockerSelfHost>();

            var listController = GetSetDataControllerSession(taskid, "", "0");

            var k = listController.Select(n => n.LineID).ToList();

            data = _tblLockerSelfHostService.GetAllActiveByListLineId(k).ToList();

            return data;
        }

        public JsonResult DataSelfHost(string taskid = "")
        {
            var data = GetSetDataSelfHostSession(taskid);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ListSelfHostProcess(string taskid = "", string address = "", int curr = 1, int total = 1)
        {
            //Check cho selfhost
            var list = GetSetDataSelfHostSession(taskid);

            var percent = (double)((double)curr / (double)total * 100);

            ViewBag.percentValue = percent;

            return PartialView(list);
        }
        #endregion

        //Xử lý với locker theo controller
        #region Locker
        public PartialViewResult PartialListLocker(string taskid = "")
        {
            var data = GetSetDataLockerSession(taskid);
            return PartialView(data);
        }

        public List<tblLocker> GetSetDataLockerSession(string taskid = "")
        {
            var data = new List<tblLocker>();

            var listController = GetSetDataControllerSession(taskid, "", "0");

            //0 - Chưa sử dụng / 1 - Cố định
            data = _tblLockerService.GetAllByType_Controllers("0,1", listController.Select(n => n.Id).ToList()).ToList();

            return data;
        }
        #endregion

        //Xử lý với thẻ
        #region Card
        public PartialViewResult PartialListCard(string key, string cardgroupids, string customergroupid, string taskid, int page = 1)
        {
            //Load danh sách thẻ đã chọn
            ViewBag.selectedCard = GetSetDataCardSession(taskid, "", "0");

            //
            var pageSize = 10;
            var customergroups = GetListChild("", customergroupid);

            var list = _tblCardService.GetAllPagingByFirstForUploadLocker(key, "", cardgroupids, customergroups, "", "", page, pageSize);

            var str = new List<string>();
            foreach (var item in list)
            {
                str.Add(item.CardNumber);
            }

            var LockerData = _tblLockerService.GetAllByCards(str);

            foreach (var item in list)
            {
                var kl = LockerData.Where(n => n.CardNumber == item.CardNumber).ToList();

                if (kl.Any())
                {
                    var desc = "";
                    var count = 0;
                    foreach (var itemLocker in kl)
                    {
                        count++;
                        desc += string.Format("{0}{1}", itemLocker.Name, count == kl.Count ? "" : ",");
                    }

                    item.LockerInfo = desc;
                }
                else
                {
                    item.LockerInfo = "Chưa gắn tủ";
                }
            }

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize);

            return PartialView(gridModel);
        }

        /// <summary>
        /// Get + Set vào session
        /// </summary>
        /// <param name="list">Danh sách thẻ</param>
        /// <returns></returns>
        private List<string> GetSetDataCardSession(string taskid, string cardnumber = "", string actionV = "0")
        {
            var host = Request.Url.Host;

            var data = (List<string>)Session[string.Format("{0}_{1}_{2}", SessionConfig.CardLockerSession, host, taskid)];

            if (data == null)
            {
                data = new List<string>();
            }

            switch (actionV)
            {
                case "1": //Thêm mới

                    if (!data.Any(n => n == cardnumber))
                    {
                        data.Add(cardnumber);
                    }

                    break;

                case "2": //Xóa

                    data.Remove(cardnumber);

                    break;

                case "3": //Xóa hết

                    data = new List<string>();

                    break;

                default:
                    break;
            }


            Session[string.Format("{0}_{1}_{2}", SessionConfig.CardLockerSession, host, taskid)] = data;

            return data;
        }

        public JsonResult CardSelected(List<string> listId, string isAdd = "0", string taskid = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                if (isAdd == "3")
                {
                    GetSetDataCardSession(taskid, "", "3");
                }
                else
                {
                    foreach (var item in listId)
                    {
                        GetSetDataCardSession(taskid, item, isAdd);
                    }
                }

                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Xử lý gắn thẻ với tủ
        #region LockerRegister
        public PartialViewResult PartialLockerRegister(string dt, string taskid)
        {
            var data = JsonConvert.DeserializeObject<tblCardExtend>(dt);

            var model = new LockerCard()
            {
                Data = new List<tblLocker>(),
                CardNumber = data.CardNumber,
                CardGroupName = data.CardGroupName,
                CardNo = data.CardNo,
                CustomerGroupName = data.CustomerGroupName,
                CustomerName = data.CustomerName,
                TaskViewId = taskid
            };

            var str = new List<string>();
            str.Add(model.CardNumber);

            model.Data = _tblLockerService.GetAllByCards(str).ToList();

            return PartialView(model);
        }

        public PartialViewResult PartialLockerModel(string dataObj = "", string cardnumber = "", string taskid = "")
        {
            var data = new tblLocker();

            if (!string.IsNullOrWhiteSpace(dataObj))
            {
                data = JsonConvert.DeserializeObject<tblLocker>(dataObj);
            }
            else
            {
                data = new tblLocker()
                {
                    CardNo = "",
                    CardNumber = "",
                    ControllerID = "",
                    DateCreated = DateTime.Now
                };
            }

            ViewBag.cardnumberValue = cardnumber;
            ViewBag.lockerData = GetListLockerByControllerSelected(taskid);

            return PartialView(data);
        }

        public JsonResult RegisterLockerWithCardNumber(string lockerid, string cardnumber)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var objLocker = _tblLockerService.GetById(lockerid);
                if (objLocker != null)
                {
                    var objCard = _tblCardService.GetByCardNumber(cardnumber);
                    if (objCard != null)
                    {
                        objLocker.LockerType = "1"; //Cố định
                        objLocker.CardNumber = objCard.CardNumber;
                        objLocker.CardNo = objCard.CardNo;

                        result = _tblLockerService.Update(objLocker);
                    }
                }
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveRegisterLockerWithCardNumber(string lockerid, string cardnumber)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var objLocker = _tblLockerService.GetById(lockerid);
                if (objLocker != null)
                {
                    //var objController = _tblLockerControllerService.GetById(objLocker.ControllerID);
                    //if (objController != null)
                    //{
                    //    var objLine = _tblLockerLineService.GetById(objController.LineID);
                    //    if (objLine != null)
                    //    {
                    //        var objSelfHost = _tblLockerSelfHostService.GetByPCID(objLine.PCID);
                    //        if (objSelfHost != null)
                    //        {
                    //            var uri = string.Format("http://{0}:8081/api/register/deletebycontroller", objSelfHost.Address);

                    //            var map = new Employee()
                    //            {
                    //                CardNumber = objLocker.CardNumber,
                    //                AccessLevelID = objLocker.ReaderIndex.ToString(),
                    //                ControllerIDs = objLocker.ControllerID,
                    //                UserID = "",
                    //                UserIDofFinger = 0,
                    //                ExpireDate = "20991231"
                    //            };

                    //            result = ApiService<Employee>.PostObjReturnObj(uri, map);
                    //        }
                    //    }
                    //}

                    //if (result.isSuccess)
                    //{
                    objLocker.LockerType = "0"; //Chưa sử dụng
                    objLocker.CardNumber = "";
                    objLocker.CardNo = "";

                    result = _tblLockerService.Update(objLocker);

                    ////Lưu sự kiện vào tblLockerProcess

                    //}
                }
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDataLockerCard(string cardnumber)
        {
            var str = new List<string>();
            str.Add(cardnumber);

            var data = _tblLockerService.GetAllByCards(str).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Xử lý nạp, hủy xuống bộ điều khiển (Giao tiếp thiết bị)
        #region Xử lý nạp, hủy
        public PartialViewResult ModalConfirm(string key, string cardgroupids, string customergroupid, string taskid, bool isAll = false, string actionTake = "", string name = "", int totalItem = 0)
        {
            //
            var model = new LockerConfirm();

            //Lấy danh sách bộ điều khiển
            model.DataController = GetSetDataControllerSession(taskid, "", "0");

            //Lấy danh sách thẻ
            if (isAll)
            {
                var cards = _tblCardService.GetAllByFirstForUploadLocker(key, "", cardgroupids, customergroupid, "", "").ToList();

                model.CardCount = cards.Count;

                //Lấy danh sách locker
                var lockers = _tblLockerService.GetAllByCards_Controllers(cards, model.DataController.Select(n => n.Id).ToList()).ToList();

                model.LockerCount = lockers.Count;

                model.DataLocker = JsonConvert.SerializeObject(lockers);
            }
            else
            {
                var cards = GetSetDataCardSession(taskid, "", "0");

                model.CardCount = cards.Count;

                //Lấy danh sách locker
                var lockers = _tblLockerService.GetAllByCards_Controllers(cards, model.DataController.Select(n => n.Id).ToList()).ToList();

                model.LockerCount = lockers.Count;

                model.DataLocker = JsonConvert.SerializeObject(lockers);
            }

            //Lấy danh sách locker
            var str = new StringBuilder();

            str.AppendLine(string.Format("<p>Bạn có chắc chắn muốn {0} <strong>{1}</strong> {2}</p>", actionTake, model.LockerCount, name));
            str.AppendLine("<p> Xuống ");

            var count = 0;
            foreach (var item in model.DataController)
            {
                count++;

                str.AppendLine(string.Format("<strong>{0}</strong>{1}", item.ControllerName, count == model.DataController.Count ? "" : ";"));
            }

            str.AppendLine("</p>");

            model.Description = str.ToString();
            model.ActionTake = actionTake;

            return PartialView(model);
        }

        public JsonResult DataSendToApp(tblLocker model)
        {
            var map = new Employee()
            {
                CardNumber = model.CardNumber,
                AccessLevelID = model.ReaderIndex.ToString(),
                ControllerIDs = model.ControllerID,
                UserID = "",
                UserIDofFinger = 0,
                ExpireDate = "20991231"
            };

            return Json(map, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ProgressBar(string address, int curr = 1, int total = 1)
        {
            var percent = (double)((double)curr / (double)total * 100);

            ViewBag.addressValue = address;

            return PartialView(percent);
        }

        /// <summary>
        /// Lưu lại sự kiện locker
        /// </summary>
        /// <param name="model"></param>
        /// <param name="actionV"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public JsonResult SaveEvent(tblLocker model, string actionV, string message)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            //Lưu tblLockerProcess

            result = _tblLockerProcessService.CreateSql(model, actionV, message, "1");

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Danh sách hỗ trợ
        #region Danh sách hỗ trợ
        public JsonResult GetTimeSetting()
        {
            var u1 = GetTimeIntevalSetting();
            var u2 = GetTimeOutSetting();

            return Json(new { u1 = Convert.ToInt32(u1), u2 = Convert.ToInt32(u2) }, JsonRequestBehavior.AllowGet);
        }

        private string GetTimeIntevalSetting()
        {
            var str = "";

            try
            {
                str = ConfigurationManager.AppSettings["LockerTimeIntevalSetting"].ToString();
            }
            catch (Exception ex)
            {
                str = "500";
            }

            return str;
        }

        private string GetTimeOutSetting()
        {
            var str = "";

            try
            {
                str = ConfigurationManager.AppSettings["LockerTimeOutSetting"].ToString();
            }
            catch (Exception ex)
            {
                str = "300000";
            }

            return str;
        }

        public List<tblLockerLine> GetListLockerLine()
        {
            return _tblLockerLineService.GetAllActive().ToList();
        }

        public List<Kztek.Model.Models.tblLockerController> GetListLockerController()
        {
            return _tblLockerControllerService.GetAllActive().ToList();
        }

        public List<tblLocker> GetListLockerByControllerSelected(string taskid)
        {
            var dt = GetSetDataControllerSession(taskid, "", "0");


            return _tblLockerService.GetAllByType_Controllers("0", dt.Select(n => n.Id).ToList()).OrderBy(n => n.ControllerID).ThenBy(n => n.ReaderIndex).ToList();
        }

        public DataTable GetListCardGroupDT()
        {
            return _tblCardGroupService.GetAllActive().ToDataTableNullable();
        }

        public DataTable GetListCustomerGroupDT()
        {
            return GetMenuList().ToDataTableNullable();
        }

        /// <summary>
        /// Danh sách menu cấp cha
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <returns></returns>
        private List<SelectListModel> GetMenuList()
        {
            var list = new List<SelectListModel>();
            //{
            //    new SelectListModel {  ItemValue = "", ItemText = "- Chọn danh mục -" }
            //};
            var MenuList = _tblCustomerGroupService.GetAllActive().ToList();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.SortOrder))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new SelectListModel { ItemValue = item1.ItemValue.ToString(), ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Đệ quy để lấy danh sách con
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private List<SelectListModel> Children(string parentID)
        {
            //Khai báo danh sách
            List<SelectListModel> lst = new List<SelectListModel>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _tblCustomerGroupService.GetAllChildByParentID(parentID.ToString()).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new SelectListModel { ItemValue = item1.ItemValue, ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                    }
                }
            }
            return lst;
        }

        private string GetListChild(string str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += id + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.CustomerGroupID.ToString() + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }

        public PartialViewResult ControllerStatus(string lineid, string controllerid)
        {
            var url = "";

            var t = _tblLockerLineService.GetById(lineid);
            if (t != null)
            {
                var k = _tblLockerSelfHostService.GetByPCID(t.PCID);
                if (k != null)
                {
                    url = k.Address;
                }
            }

            var result = FunctionHelper.CheckConnectController(url, controllerid);

            var model = new SelectListModel()
            {
                ItemValue = result
            };

            return PartialView(model);
        }
        #endregion 
        #endregion

        public ActionResult IndexOpenLockerManual()
        {
            var model = new LockerOpenManual()
            {
                DataController = GetListLockerController(),
                TaskViewId = Common.GenerateId()
            };

            return View(model);
        }

        public PartialViewResult PartialLocker(string key, string controllerid, int page = 1, string taskid = "")
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblLockerService.GetAllPagingByFirst(key, controllerid, page, pageSize);

            //Đổ lên grid
            var listModel = PageModelCustom<tblLocker>.GetPage(list, page, pageSize);

            ViewBag.selectedLocker = GetSetDataLockerSession(taskid, "", "0");

            return PartialView(listModel);
        }

        public List<tblLocker> GetSetDataLockerSession(string taskid, string lockerid = "", string actionV = "")
        {
            var host = Request.Url.Host;

            var data = (List<Kztek.Model.Models.tblLocker>)Session[string.Format("{0}_{1}_{2}", SessionConfig.LockerSession, host, taskid)];

            if (data == null)
            {
                data = new List<Model.Models.tblLocker>();
            }

            switch (actionV)
            {
                case "1": //Thêm mới

                    if (!data.Any(n => n.Id == lockerid))
                    {
                        var objLocker = _tblLockerService.GetById(lockerid);
                        if (objLocker != null)
                        {
                            data.Add(objLocker);
                        }
                    }

                    break;

                case "2": //Xóa

                    var objControlleDelete = data.FirstOrDefault(n => n.Id == lockerid);
                    if (objControlleDelete != null)
                    {
                        data.Remove(objControlleDelete);
                    }

                    break;

                case "3": //Xóa hết

                    data = new List<Model.Models.tblLocker>();

                    break;

                default:
                    break;
            }

            //Gán lại vào session
            Session[string.Format("{0}_{1}_{2}", SessionConfig.LockerSession, host, taskid)] = data;

            return data;
        }

        public JsonResult LockerSelected(List<string> listId, string isAdd = "0", string taskid = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                if (isAdd == "3")
                {
                    GetSetDataLockerSession(taskid, "", "3");
                }
                else
                {
                    foreach (var item in listId)
                    {
                        GetSetDataLockerSession(taskid, item, isAdd);
                    }
                }

                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnlockSelectedLocker(string taskid = "")
        {
            var newLis = new List<Employee>();
            var listController = new List<string>();

            var list = GetSetDataLockerSession(taskid);
            foreach (var item in list)
            {
                listController.Add(item.ControllerID);

                var map = new Employee()
                {
                    CardNumber = item.CardNumber,
                    AccessLevelID = item.ReaderIndex.ToString(),
                    ControllerIDs = item.ControllerID,
                    UserID = "",
                    UserIDofFinger = 0,
                    ExpireDate = "20991231"
                };

                newLis.Add(map);
            }

            //Danh sách controllers
            var dataController = _tblLockerControllerService.GetAllActiveByListId(listController).ToList();

            //Danh sách line
            var dataLine = _tblLockerLineService.GetAllActiveByListLine(dataController.Select(n => n.LineID).ToList()).ToList();

            //Danh sách selfhost
            var dataSelfHost = _tblLockerSelfHostService.GetAllActiveByListLineId(dataLine.Select(n => n.Id).ToList()).ToList();

            return Json(new { dataSend = newLis, dataAddress = dataSelfHost.Select(n => n.Address).ToList() });
        }

        public JsonResult UnlockLocker(string ModelJson)
        {
            var tblLocker = JsonConvert.DeserializeObject<tblLocker>(ModelJson);

            var newLis = new List<Employee>();
            var listController = new List<string>();

            var map = new Employee()
            {
                CardNumber = tblLocker.CardNumber,
                AccessLevelID = tblLocker.ReaderIndex.ToString(),
                ControllerIDs = tblLocker.ControllerID,
                UserID = "",
                UserIDofFinger = 0,
                ExpireDate = "20991231"
            };

            newLis.Add(map);

            //Danh sách controllers
            var dataController = _tblLockerControllerService.GetAllActiveByListId(listController).ToList();

            //Danh sách line
            var dataLine = _tblLockerLineService.GetAllActiveByListLine(dataController.Select(n => n.LineID).ToList()).ToList();

            //Danh sách selfhost
            var dataSelfHost = _tblLockerSelfHostService.GetAllActiveByListLineId(dataLine.Select(n => n.Id).ToList()).ToList();

            return Json(new { dataSend = newLis, dataAddress = dataSelfHost.Select(n => n.Address).ToList() });
        }

        public JsonResult SaveLockerUnlockProcess(Employee model, string message)
        {
            var tblLocker = _tblLockerService.GetByControllerID_ReaderIndex(model.ControllerIDs, Convert.ToInt32(model.AccessLevelID));

            if (tblLocker != null)
            {
                _tblLockerProcessService.CreateSql(tblLocker, "OPEN", "Mở tủ: " + message, "4");
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}