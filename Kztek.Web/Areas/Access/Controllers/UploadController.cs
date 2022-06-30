using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class UploadController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblCardGroupService _tblCardGroupService;

        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;

        private ItblAccessPCService _tblAccessPCService;

        private ItblAccessControllerService _tblAccessControllerService;

        private ItblAccessLevelService _tblAccessLevelService;

        private ISelfHostConfigService _SelfHostConfigService;

        private ItblAccessLineService _tblAccessLineService;

        private ItblAccessUploadProcessService _tblAccessUploadProcessService;
        private ItblAccessControllerGroupService _tblAccessControllerGroupService;

        public UploadController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerGroupService _tblCustomerGroupService,
            ItblAccessPCService _tblAccessPCService, ItblAccessControllerService _tblAccessControllerService,
            ItblAccessLevelService _tblAccessLevelService, ISelfHostConfigService _SelfHostConfigService, ItblAccessLineService _tblAccessLineService,
            ItblAccessUploadProcessService _tblAccessUploadProcessService, ItblCustomerService _tblCustomerService, ItblAccessControllerGroupService _tblAccessControllerGroupService)
        {
            this._tblCardService = _tblCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblAccessPCService = _tblAccessPCService;
            this._tblAccessControllerService = _tblAccessControllerService;
            this._tblAccessLevelService = _tblAccessLevelService;
            this._SelfHostConfigService = _SelfHostConfigService;
            this._tblAccessLineService = _tblAccessLineService;
            this._tblAccessUploadProcessService = _tblAccessUploadProcessService;
            this._tblCustomerService = _tblCustomerService;
            this._tblAccessControllerGroupService = _tblAccessControllerGroupService;
        }
        public List<SelectListModel> LineToDDL(string pcid)  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Danh mục line --" }, };
            var listLine = _tblAccessLineService.GetAllActiveByListPC(pcid);
            if (listLine.Any())
            {
                foreach (var item in listLine)
                {
                    list.Add(new SelectListModel { ItemValue = item.LineID.ToString(), ItemText = item.LineName });
                }
            }
            return list;
        }

        [CheckSessionLogin]
        public ActionResult Index(string fromdate, string todate)
        {
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var model = new SelectListModelUpload()
            {
                dtComputer = _tblAccessPCService.GetAllActive().ToDataTableNullable(),
                dtCardGroup = _tblCardGroupService.GetAllActive().ToDataTableNullable(),
                dtCustomerGroup = GetMenuList().ToDataTableNullable(),
                dtAccessLevel = _tblAccessLevelService.GetAllActive().ToDataTableNullable()
            };
            ViewBag.GroupController = GetControllerGroupList();
            ViewBag.PageSize = FunctionHelper.PageSize();
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(model);
        }

        public PartialViewResult PartialLine(string pcid)
        {
            var dtline = LineToDDL(pcid);
            return PartialView(dtline);
        }

        #region Danh sách load dữ liệu phân trang
        /// <summary>
        /// Load danh sách bộ điều khiển
        /// </summary>
        /// <param name="computerids">Danh sách các bộ điều khiển tìm kiếm</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        public PartialViewResult ListController(string key, string computerids, string line, string groupControllerId, int page = 1)
        {
            ViewBag.selectedController = GetSetFromSessionController(null);
            var ControllerGroupList = _tblAccessControllerGroupService.GetAll();
            int pageSize = 25;

            var list = _tblAccessControllerService.GetAllPagingByFirst(key, computerids, line, groupControllerId, page, pageSize);
            foreach (var item in list)
            {
                if (!String.IsNullOrEmpty(item.ControllerGroupId))
                {
                    var ControllerGroupName = ControllerGroupList.Where(n => n.Id == item.ControllerGroupId).FirstOrDefault().Name;
                    item.ControllerGroupId = ControllerGroupName;
                }
            }
            var gridModel = PageModelCustom<tblAccessController>.GetPage(list, page, pageSize);

            return PartialView(gridModel);
        }

        public PartialViewResult ListSelfHost()
        {
            //Check cho selfhost
            var list = GetSetFromSessionSelfHost(GetSelfHostByController(GetSetFromSessionController(null)));

            return PartialView(list);
        }

        public PartialViewResult ListSelfHostProcess(string address, int curr = 1, int total = 1)
        {
            //Check cho selfhost
            var list = GetSetFromSessionSelfHost(GetSelfHostByController(GetSetFromSessionController(null)));

            var percent = (double)((double)curr / (double)total * 100);

            ViewBag.percentValue = percent;

            return PartialView(list);
        }

        /// <summary>
        /// Load danh sách thẻ
        /// </summary>
        /// <param name="cardgroupids">Danh sách nhóm thẻ chọn</param>
        /// <param name="customergroupid">Id nhóm khách hàng</param>
        /// <param name="accesslevelids">Danh sách quyền chọn</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        public PartialViewResult ListCard(string fromdate, string todate, string key, string cardgroupids, string customergroupid, string accesslevelids, int page = 1, string column = "", string sort = "", int pageSize = 10)
        {
            //Load danh sách thẻ đã chọn
            ViewBag.selectedCard = GetSetFromSessionCard(null);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            //        
            var customergroups = GetListChild("", customergroupid);

            var list = _tblCardService.GetAllPagingByFirstForUpload(key, "", cardgroupids, customergroups, "", todate, page, pageSize, accesslevelids, column, sort);

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize);

            return PartialView(gridModel);
        }

        //public PartialViewResult ListCardChoice(string key)
        //{
        //    //Load danh sách thẻ đã chọn
        //    var selectedCard = GetSetFromSessionCard(null);

        //    if (!string.IsNullOrWhiteSpace(key))
        //    {
        //        selectedCard = selectedCard.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key)).ToList();
        //    }

        //    return PartialView(selectedCard);
        //}

        /// <summary>
        /// Load danh sách khách hàng
        /// </summary>
        /// <param name="customergroupid">Id nhóm khách hàng</param>
        /// <param name="accesslevelids">Danh sách quyền chọn</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        public PartialViewResult ListCustomer(string key, string customergroupid, string accesslevelids, int page = 1, int pageSize = 10)
        {
            //Load danh sách thẻ đã chọn
            ViewBag.selectedCustomer = GetSetFromSessionCustomer(null);

            //       
            var customergroups = GetListChild("", customergroupid);

            var list = _tblCustomerService.GetAllPagingByFirstForUpload(key, "", customergroups, "", "", page, pageSize, accesslevelids);

            var gridModel = PageModelCustom<tblCustomerExtend>.GetPage(list, page, pageSize);

            return PartialView(gridModel);
        }
        #endregion

        #region Session selfhosts
        private List<SelfHostConfig> GetSelfHostByController(List<tblAccessController> list)
        {
            var listSelfHost = new List<SelfHostConfig>();

            if (list.Any())
            {
                var strLines = new List<string>();

                foreach (var item in list)
                {
                    strLines.Add(item.LineID.ToString());
                }

                listSelfHost = _SelfHostConfigService.GetAllActiveByListLineId(strLines).Distinct().ToList();
            }

            return listSelfHost;
        }

        private List<SelfHostConfig> GetSetFromSessionSelfHost(List<SelfHostConfig> list)
        {
            var host = Request.Url.Host;

            var listSelfHost = (List<SelfHostConfig>)Session[string.Format("{0}_{1}", SessionConfig.SelfHostActiveAccessSession, host)];

            if (listSelfHost != null && listSelfHost.Any())
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        if (!listSelfHost.Any(n => n.Id.ToString().Equals(item.Id.ToString())))
                        {
                            listSelfHost.Add(item);
                        }
                    }
                }
                else if (list != null)
                {
                    listSelfHost = list;
                }
            }
            else
            {
                if (list == null)
                {
                    list = new List<SelfHostConfig>();
                }

                listSelfHost = list;
            }

            Session[string.Format("{0}_{1}", SessionConfig.SelfHostActiveAccessSession, host)] = listSelfHost;

            return listSelfHost;
        }
        #endregion

        #region Session controllers
        /// <summary>
        /// Get + Set vào session
        /// </summary>
        /// <param name="list">Danh sách thẻ</param>
        /// <returns></returns>
        private List<tblAccessController> GetSetFromSessionController(List<tblAccessController> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listControllerChoice = (List<tblAccessController>)Session[string.Format("{0}_{1}", SessionConfig.ControllerActiveAccessSession, host)];

            if (listControllerChoice != null && listControllerChoice.Any())
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listControllerChoice.Any(n => n.ControllerID.ToString().Equals(item.ControllerID.ToString())))
                            {
                                listControllerChoice.Add(item);
                            }
                        }
                        else
                        {
                            if (listControllerChoice.Any(n => n.ControllerID.ToString().Equals(item.ControllerID.ToString())))
                            {
                                listControllerChoice.RemoveAll(n => n.ControllerID.ToString() == item.ControllerID.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                if (list == null)
                {
                    list = new List<tblAccessController>();
                }

                listControllerChoice = list;
            }

            Session[string.Format("{0}_{1}", SessionConfig.ControllerActiveAccessSession, host)] = listControllerChoice;



            return listControllerChoice;
        }

        /// <summary>
        /// Thêm thẻ chọn
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public JsonResult AddNewListController(List<string> listId, bool isAdd)
        {
            var result = new MessageReport();

            try
            {
                var list = _tblAccessControllerService.GetAllActiveByListId(listId).ToList();

                if (list.Any())
                {
                    GetSetFromSessionController(list, isAdd);
                }

                result.Message = "Thêm mới thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Xóa 1 thẻ chọn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteOneSelectedController(string id)
        {
            try
            {
                var host = Request.Url.Host;
                var listControllerChoice = (List<tblAccessController>)Session[string.Format("{0}_{1}", SessionConfig.ControllerActiveAccessSession, host)];
                var listMap = listControllerChoice.ToList();

                if (listControllerChoice.Any())
                {
                    foreach (var item in listMap)
                    {
                        if (item.ControllerID.ToString().Equals(id))
                        {
                            listControllerChoice.Remove(item);
                        }
                    }
                }

                Session[string.Format("{0}_{1}", SessionConfig.ControllerActiveAccessSession, host)] = listControllerChoice;

                var result = new MessageReport();
                result.Message = "Xóa thành công";
                result.isSuccess = true;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa tất cả
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteAllSelectedController()
        {
            try
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.ControllerActiveAccessSession, host)] = new List<tblAccessController>();

                var result = new MessageReport();
                result.Message = "Xóa tất cả danh sách thành công";
                result.isSuccess = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ControllerStatus(string lineid, string controllerid)
        {
            var url = "";

            var t = _tblAccessLineService.GetById(Guid.Parse(lineid));
            if (t != null)
            {
                var k = _SelfHostConfigService.GetByPCID(t.PCID);
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

        #region Session cards
        /// <summary>
        /// Get + Set vào session
        /// </summary>
        /// <param name="list">Danh sách thẻ</param>
        /// <returns></returns>
        private List<tblCardExtend> GetSetFromSessionCard(List<tblCardExtend> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)];
            if (listCardChoice != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            //if(listCardChoice.Count < 20)
                            //{
                            //    if (!listCardChoice.Any(n => n.CardID.ToString().Equals(item.CardID.ToString())))
                            //    {
                            //        listCardChoice.Add(item);
                            //    }
                            //}
                            if (!listCardChoice.Any(n => n.CardID.ToString().Equals(item.CardID.ToString())))
                            {
                                listCardChoice.Add(item);
                            }

                        }
                        else
                        {
                            if (listCardChoice.Any(n => n.CardID.ToString().Equals(item.CardID.ToString())))
                            {
                                listCardChoice.RemoveAll(n => n.CardID.ToString() == item.CardID.ToString());
                            }

                        }
                    }
                }
            }
            else
            {
                if (list == null)
                {
                    list = new List<tblCardExtend>();
                }

                listCardChoice = list;
            }

            Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)] = listCardChoice;

            return listCardChoice;
        }

        /// <summary>
        /// Thêm thẻ chọn
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public JsonResult AddNewListCard(List<string> listId, bool isAdd)
        {
            var result = new MessageReport();

            try
            {
                var list = _tblCardService.GetAllActiveByListIdForUpload(listId).ToList();

                if (list.Any())
                {
                    GetSetFromSessionCard(list, isAdd);
                }

                result.Message = "Thêm mới thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Xóa 1 thẻ chọn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteOneSelectedCard(string id)
        {
            try
            {
                var host = Request.Url.Host;
                var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)];
                var listMap = listCardChoice.ToList();

                if (listCardChoice.Any())
                {
                    foreach (var item in listMap)
                    {
                        if (item.CardID.ToString().Equals(id))
                        {
                            listCardChoice.Remove(item);
                        }
                    }
                }

                Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)] = listCardChoice;

                var result = new MessageReport();
                result.Message = "Xóa thành công";
                result.isSuccess = true;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa tất cả
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteAllSelectedCard()
        {
            try
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)] = new List<tblCardExtend>();

                var result = new MessageReport();
                result.Message = "Xóa tất cả danh sách thành công";
                result.isSuccess = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Session customers
        /// <summary>
        /// Get + Set vào session
        /// </summary>
        /// <param name="list">Danh sách thẻ</param>
        /// <returns></returns>
        private List<tblCustomerExtend> GetSetFromSessionCustomer(List<tblCustomerExtend> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listCustomerChoice = (List<tblCustomerExtend>)Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)];
            if (listCustomerChoice != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (listCustomerChoice.Count < 20)
                            {
                                if (!listCustomerChoice.Any(n => n.CustomerID.ToString().Equals(item.CustomerID.ToString())))
                                {
                                    listCustomerChoice.Add(item);
                                }
                            }

                        }
                        else
                        {
                            if (listCustomerChoice.Any(n => n.CustomerID.ToString().Equals(item.CustomerID.ToString())))
                            {
                                listCustomerChoice.RemoveAll(n => n.CustomerID.ToString() == item.CustomerID.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                if (list == null)
                {
                    list = new List<tblCustomerExtend>();
                }

                listCustomerChoice = list;
            }

            Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)] = listCustomerChoice;

            return listCustomerChoice;
        }

        /// <summary>
        /// Thêm thẻ chọn
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public JsonResult AddNewListCustomer(List<string> listId, bool isAdd)
        {
            var result = new MessageReport();

            try
            {
                var list = _tblCustomerService.GetAllActiveByListIdForUpload(listId).ToList();

                if (list.Any())
                {
                    GetSetFromSessionCustomer(list, isAdd);
                }

                result.Message = "Thêm mới thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Xóa 1 thẻ chọn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteOneSelectedCustomer(string id)
        {
            try
            {
                var host = Request.Url.Host;
                var listCustomerChoice = (List<tblCustomerExtend>)Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)];
                var listMap = listCustomerChoice.ToList();

                if (listCustomerChoice.Any())
                {
                    foreach (var item in listMap)
                    {
                        if (item.CustomerID.ToString().Equals(id))
                        {
                            listCustomerChoice.Remove(item);
                        }
                    }
                }

                Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)] = listCustomerChoice;

                var result = new MessageReport();
                result.Message = "Xóa thành công";
                result.isSuccess = true;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xóa tất cả
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteAllSelectedCustomer()
        {
            try
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)] = new List<tblCustomerExtend>();

                var result = new MessageReport();
                result.Message = "Xóa tất cả danh sách thành công";
                result.isSuccess = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public PartialViewResult ModalConfirm(bool isAll = false, int totalItem = 0, int totalItemCus = 0, string actionTake = "", string name = "")
        {
            var list = GetSetFromSessionController(null);
            var listCard = GetSetFromSessionCard(null);
            var listCustomer = GetSetFromSessionCustomer(null);

            totalItem = isAll ? totalItem : listCard.Count;

            totalItemCus = isAll ? totalItemCus : listCustomer.Count;

            ViewBag.isAllValue = isAll;

            var str = new StringBuilder();

            str.AppendLine(string.Format("<p>Bạn có chắc chắn muốn {0} <strong>{1}</strong> {2}</p>", actionTake, name == "thẻ" ? totalItem : totalItemCus, name));
            str.AppendLine("<p> Xuống ");

            var count = 0;
            foreach (var item in list)
            {
                count++;

                str.AppendLine(string.Format("<strong>{0}</strong>{1}", item.ControllerName, count == list.Count ? "" : ";"));
            }

            str.AppendLine("</p>");

            ViewBag.strValue = str.ToString();

            ViewBag.actionTakeValue = actionTake == "nạp" ? "UPLOAD" : "DELETE";
            ViewBag.nameValue = name == "thẻ" ? "CARD" : "FINGER";

            return PartialView();
        }

        #region Upload/Delete Card To Devices
        public JsonResult GetListCardWantToUse(SelectListModelCardUpload obj)
        {
            var totalPage = 0;
            var totalItem = 0;
            var pageSize = 0;

            var dtg = new List<AccessUploadMultiDevice>();

            //Lấy người dùng hiện tại
            var user = GetCurrentUser.GetUser();

            var model = new SelectListModelCardUploadReturn();
            model.ListSelfHost = GetSetFromSessionSelfHost(null);
            model.ListController = GetSetFromSessionController(null);


            if (obj.isall)
            {
                var customergroups = GetListChild("", obj.customergroupid);

                //      var list1 = _tblCardService.GetAllPagingByFirstForUpload(obj.key, "", obj.cardgroupids, customergroups, "", "", obj.pageIndex, obj.pageSize, obj.accesslevelids, "", "");
                //var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, obj.pageIndex, obj.pageSize);

                //totalPage = gridModel.TotalPage;
                //totalItem = gridModel.TotalItem;
                //pageSize = gridModel.PageSize;
                var list = _tblCardService.GetAllForUpload(obj.key, "", obj.cardgroupids, customergroups, "", "", obj.accesslevelids, "", "");
                model.ListCard = list;
                //model.ListCard = gridModel.Data.ToList();
            }
            else
            {
                model.ListCard = GetSetFromSessionCard(null);

                model.ListCard = _tblCardService.GetAllByFirst_v2(model.ListCard);
            }

            var listCustomerId = new List<string>();

            foreach (var item in model.ListCard)
            {
                if (!string.IsNullOrWhiteSpace(item.CustomerId))
                {
                    listCustomerId.Add(item.CustomerId);
                }
            }
            var listEmployee1 = new List<Employee>();
            var listEmployee = new List<Employee>();
            var listGrEmploy = new List<List<Employee>>();
            //Danh sách khách hàng
            var ListCustomer = _tblCustomerService.GetAllByListId(listCustomerId).ToList();

            if (model.ListCard.Any())
            {
                foreach (var item in model.ListSelfHost)
                {
                    var ko = new AccessUploadMultiDevice()
                    {
                        Address = item.Address,
                        pageIndex = obj.pageIndex,
                        totalPage = totalPage,
                        totalItem = totalItem,
                        pageSize = pageSize
                    };

                    var lines = _tblAccessLineService.GetAllActiveByListPC(item.PCID);

                    var ControllerByPC = model.ListController.Where(n => lines.Any(m => m.LineID.ToString() == n.LineID)).ToList();

                    model.ListEmployee = new List<Employee>();

                    //
                    ko.totalItem = ko.totalItem * ControllerByPC.Count;
                    ko.totalController = ControllerByPC.Count;

                    foreach (var itemCard in model.ListCard)
                    {
                        foreach (var itemController in ControllerByPC)
                        {

                            try
                            {
                                var map = new Employee();

                                map.CardNumber = itemCard.CardNumber.Trim();

                                map.CardType = itemCard.CardType;

                                map.AccessLevelID = itemCard.AccessLevelID.Trim();

                                map.ControllerIDs = itemController.ControllerID.ToString();

                                map.UserID = user.Id;

                                //Lấy khách hàng
                                if (!string.IsNullOrWhiteSpace(itemCard.CustomerId))
                                {
                                    if (itemCard.CustomerId != null)
                                    {
                                        var objCustomer = ListCustomer.FirstOrDefault(n => n.CustomerID.ToString().ToLower() == itemCard.CustomerId.ToLower());
                                        if (objCustomer != null)
                                        {
                                            map.UserIDofFinger = objCustomer.UserIDofFinger;

                                            map.Passwords = objCustomer.Password;
                                        }
                                        else
                                        {
                                            map.UserIDofFinger = 0;

                                            map.Passwords = "";
                                        }
                                    }

                                }
                                else
                                {
                                    map.UserIDofFinger = 0;

                                    map.Passwords = "";
                                }

                                if (obj.isusenewdate)
                                {
                                    map.ExpireDate = Convert.ToDateTime(obj.newdateexpire).ToString("yyyyMMdd").Trim();
                                }
                                else
                                {
                                    map.ExpireDate = Convert.ToDateTime(itemCard.AccessExpireDate).ToString("yyyyMMdd").Trim();
                                }
                                model.ListEmployee.Add(map);
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                    }

                    ko.DataSend = model.ListEmployee;



                    dtg.Add(ko);
                }
            }

            //var t = new SelectListModelCardUploadReturn2()
            //{
            //    ListEmployee = model.ListEmployee,
            //    ListSelfHost = model.ListSelfHost
            //};

            //var red = JsonConvert.SerializeObject(t);
            List<Employee[]> insertCMDs = new List<Employee[]>();

            Employee[] arrEmploye = new Employee[1000];
            int j = 0, i = 1;
            foreach (var item in dtg)
            {

                foreach (var item2 in item.DataSend)
                {

                    if (i <= (j + 1) * 1000)
                    {
                        if (i % 1000 == 0)
                        {
                            int x = 0;
                        }
                        arrEmploye[i % 1000 == 0 ? 1000 - 1 : i % 1000 - 1] = item2;
                    }
                    else
                    {
                        insertCMDs.Add(arrEmploye);
                        arrEmploye = new Employee[1000];
                        j++;
                        arrEmploye[0] = item2;
                    }
                    i++;
                }

            }
            if (insertCMDs.Count < j + 1)
            {
                insertCMDs.Add(arrEmploye);
            }
  
            bool check = InsertValue(insertCMDs, obj);
            if (check)
            {
                string updateRequestCMD = "UPDATE tblColtrolCardRequest SET ControlRequest = 1";
                ExcuteSQL.Execute(updateRequestCMD);

            }
            var a = new JsonResult { Data = dtg, MaxJsonLength = Int32.MaxValue };
            return a;
        }

        private bool InsertValue(List<Employee[]> listEmployee, SelectListModelCardUpload obj)
        {
            var str = new StringBuilder();

            var check = false;
            var count = 0;
            foreach (Employee[] item2 in listEmployee)
            {
                str.Append("Insert into tblWaitingControlCard  ([ControllerID],[isDownload],[isDelete],[Mode],[CardNumber],[AccessLevelId],[Finger1],[Finger2],[UserIdOfFinger],[CardType],[UserId],[Password])VALUES");
                foreach (var item1 in item2.ToList())
                {
                    count++;
                    if (item1 != null)
                    {
                        str.Append("(");
                        str.Append(string.Format("'{0}',", item1.ControllerIDs));
                        str.Append(string.Format("{0},", obj.action == "UPLOAD" ? 1 : 0));
                        str.Append(string.Format("{0},", obj.action == "DELETE" ? 1 : 0));
                        str.Append(string.Format("{0},", obj.type == "CARD" ? 0 : 1));
                        str.Append(string.Format("'{0}',", item1.CardNumber));
                        str.Append(string.Format("'{0}',", item1.AccessLevelID));
                        str.Append(string.Format("'{0}',", item1.Fingers1));
                        str.Append(string.Format("'{0}',", item1.Fingers2));
                        str.Append(string.Format("'{0}',", item1.UserIDofFinger));
                        str.Append(string.Format("{0},", item1.CardType));
                        str.Append(string.Format("'{0}',", item1.UserID));
                        str.Append(string.Format("'{0}'", item1.Passwords));
                        str.Append("),");
                    }

                }
                string cmd = str.ToString().Substring(0, str.ToString().Length - 1);
                check = ExcuteSQL.Execute(cmd);
                str.Clear();

            }

            return check;
        }

        public JsonResult GetListCustomerWantToUse(SelectListModelCardUpload obj)
        {
            var totalPage = 0;
            var totalItem = 0;
            var pageSize = 0;

            var dtg = new List<AccessUploadMultiDevice>();

            //Lấy người dùng hiện tại
            var user = GetCurrentUser.GetUser();

            var model = new SelectListModelCardUploadReturn();
            model.ListSelfHost = GetSetFromSessionSelfHost(null);
            model.ListController = GetSetFromSessionController(null);
            model.ListEmployee = new List<Employee>();
            model.IsUseNewDate = obj.isusenewdate;

            if (obj.isall)
            {
                var customergroups = GetListChild("", obj.customergroupid);

                var list = _tblCustomerService.GetAllPagingByFirstForUpload(obj.key, "", customergroups, "", "", obj.pageIndex, obj.pageSize, obj.accesslevelids);
                var gridModel = PageModelCustom<tblCustomerExtend>.GetPage(list, obj.pageIndex, obj.pageSize);

                totalPage = gridModel.TotalPage;
                totalItem = gridModel.TotalItem;
                pageSize = gridModel.PageSize;

                model.ListCustomer = gridModel.Data.ToList();

                //var customergroups = GetListChild("", obj.customergroupid);

                //model.ListCustomer = _tblCustomerService.GetAllByFirst(obj.key, "", customergroups, "", "", obj.accesslevelids);

                //totalPage = model.ListCustomer.Count / 10;
            }
            else
            {
                var models = GetSetFromSessionCustomer(null);
                model.ListCustomer = _tblCustomerService.GetAllByFirst(models);
            }

            if (model.ListCustomer.Any())
            {
                foreach (var item in model.ListSelfHost)
                {
                    var ko = new AccessUploadMultiDevice()
                    {
                        Address = item.Address,
                        pageIndex = obj.pageIndex,
                        totalPage = totalPage,
                        totalItem = totalItem,
                        pageSize = pageSize
                    };

                    var lines = _tblAccessLineService.GetAllActiveByListPC(item.PCID);

                    var ControllerByPC = model.ListController.Where(n => lines.Any(m => m.LineID.ToString() == n.LineID)).ToList();

                    model.ListEmployee = new List<Employee>();

                    ko.totalItem = ko.totalItem * ControllerByPC.Count;
                    ko.totalController = ControllerByPC.Count;

                    foreach (var itemCustomer in model.ListCustomer)
                    {
                        foreach (var itemController in ControllerByPC)
                        {
                            var map = new Employee();

                            map.CardNumber = "0";

                            map.AccessLevelID = itemCustomer.AccessLevelID.Trim();

                            map.ControllerIDs = itemController.ControllerID.ToString();

                            map.UserID = user.Id;

                            map.UserIDofFinger = itemCustomer.UserIDofFinger;

                            map.Fingers1 = itemCustomer.Finger1;

                            map.Fingers2 = itemCustomer.Finger2;

                            map.Passwords = itemCustomer.Password;

                            map.VerifyTypeID = 0;

                            if (obj.isusenewdate)
                            {
                                map.ExpireDate = Convert.ToDateTime(obj.newdateexpire).ToString("yyyyMMdd").Trim();
                            }
                            else
                            {
                                map.ExpireDate = Convert.ToDateTime(itemCustomer.AccessExpireDate).ToString("yyyyMMdd").Trim();
                            }

                            model.ListEmployee.Add(map);
                        }
                    }

                    ko.DataSend = model.ListEmployee;

                    dtg.Add(ko);
                }
            }

            var t = new SelectListModelCardUploadReturn2()
            {
                ListEmployee = model.ListEmployee,
                ListSelfHost = model.ListSelfHost
            };

            //var red = JsonConvert.SerializeObject(t);

            var a = new JsonResult { Data = dtg, MaxJsonLength = Int32.MaxValue };

            return a;

            //return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SaveEvent
        public JsonResult SaveEvent(SelectListModelUploadSubmit model)
        {
            var result = _tblAccessUploadProcessService.SaveProcess(model.objE, model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ProgressBar(string address, int curr = 1, int total = 1)
        {
            var percent = (double)((double)curr / (double)total * 100);

            ViewBag.addressValue = address;

            return PartialView(percent);
        }
        #endregion

        public JsonResult GetTimeSetting()
        {
            var u1 = GetTimeIntevalSetting();
            var u2 = GetTimeOutSetting();

            return Json(new { u1 = Convert.ToInt32(u1), u2 = Convert.ToInt32(u2) }, JsonRequestBehavior.AllowGet);
        }

        #region Danh sách hõ trợ
        public IEnumerable<tblCardGroup> GetCardGroup()
        {
            return _tblCardGroupService.GetAllActive();
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

        private string GetTimeIntevalSetting()
        {
            var str = "";

            try
            {
                str = ConfigurationManager.AppSettings["TimeIntevalSetting"].ToString();
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
                str = ConfigurationManager.AppSettings["TimeOutSetting"].ToString();
            }
            catch (Exception ex)
            {
                str = "300000";
            }

            return str;
        }


        private List<tblAccessControllerGroup> GetControllerGroupList()
        {
            return _tblAccessControllerGroupService.GetAll().OrderBy(n => n.SortOrder).ToList();
        }

        #endregion

        /// <summary>
        /// đếm số lượng thông tin được chọn
        /// </summary>
        /// <param name="type">1 là card, 2 là customer</param>
        /// <returns></returns>
        public JsonResult GetCountListSession(string type)
        {
            var result = new MessageReport();
            var host = Request.Url.Host;
            int count = 0;
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type.Equals("1"))
                    {
                        var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveAccessSession, host)];
                        if (listCardChoice != null)
                        {
                            count = listCardChoice.Count;
                        }
                    }
                    else
                    {
                        var listCustomerChoice = (List<tblCustomerExtend>)Session[string.Format("{0}_{1}", SessionConfig.CustomerActiveAccessSession, host)];
                        if (listCustomerChoice != null)
                        {
                            count = listCustomerChoice.Count;
                        }
                    }
                }

                if (count < 20)
                {
                    result.Message = "";
                    result.isSuccess = true;
                }
                else
                {
                    result.Message = "";
                    result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}