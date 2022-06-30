
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking.Event;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using Kztek.Service;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ReportController : Controller
    {
        #region Services
        public static int count_TX = 0;
        public static int count_NotTX = 0;
        private ItblAlarmService _tblAlarmService;
        private IUserService _UserService;
        private ItblLaneService _tblLaneService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblVehicleGroupService _tblVehicleGroupService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCustomerService _tblCustomerService;
        private IReportService _ReportService;
        private ItblCompartmentService _tblCompartmentService;
        private ItblCardEventService _tblCardEventService;
        private ItblLoopEventService _tblLoopEventService;
        private IMenuFunctionService _MenuFunctionService;
        private IExcelColumnService _ExcelColumnService;
        private IPublicEventService _PublicEventService;
        private ItblLogService _tblLogService;

        private ItblCardService _tblCardService;
        private IReduceParkingFeesService _ReduceParkingFeesService;

        public ReportController(IReduceParkingFeesService _ReduceParkingFeesService, ItblAlarmService _tblAlarmService, IUserService _UserService, ItblLaneService _tblLaneService, ItblSystemConfigService _tblSystemConfigService, ItblCardGroupService _tblCardGroupService, IReportService _ReportService, ItblVehicleGroupService _tblVehicleGroupService, ItblCustomerGroupService _tblCustomerGroupService, ItblCustomerService _tblCustomerService, ItblCompartmentService _tblCompartmentService, ItblCardEventService _tblCardEventService, ItblLoopEventService _tblLoopEventService, IMenuFunctionService _MenuFunctionService, IExcelColumnService _ExcelColumnService, IPublicEventService _PublicEventService, ItblLogService _tblLogService, ItblCardService _tblCardService)
        {
            this._tblAlarmService = _tblAlarmService;
            this._UserService = _UserService;
            this._ReduceParkingFeesService = _ReduceParkingFeesService;
            this._tblLaneService = _tblLaneService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblVehicleGroupService = _tblVehicleGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._ReportService = _ReportService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCompartmentService = _tblCompartmentService;
            this._tblCardEventService = _tblCardEventService;
            this._tblLoopEventService = _tblLoopEventService;
            this._MenuFunctionService = _MenuFunctionService;
            this._ExcelColumnService = _ExcelColumnService;
            this._PublicEventService = _PublicEventService;
            this._tblLogService = _tblLogService;
            this._tblCardService = _tblCardService;
        }
        #endregion

        #region Xe trong bãi hiện tại

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportIn(string isCheckPlate,string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string excelcol = "", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportIn_Excel(isCheckPlate, key, chkCheckByTime, fromdate, todate, cardgroup, lane, user, strCG, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString(), item["Nhóm thẻ"].ToString());

                    var _lane = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_lane != null)
                    {
                        item["Làn vào"] = _lane.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }
                    var _cardgroup = GetCardGroupListNew().FirstOrDefault(n => n.ItemValue.Equals(item["Nhóm thẻ"].ToString()));
                    if (_cardgroup != null)
                    {
                        item["Nhóm thẻ"] = _cardgroup.ItemText;
                    }
                    else
                    {
                        item["Nhóm thẻ"] = "";
                    }
                    var _user = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_user != null)
                    {
                        item["Giám sát vào"] = _user.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }


                }

                //Xuất file theo format
                ReportInFormatCell(listExcel, excelcol, Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportIn");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetReportIn(isCheckPlate, key, chkCheckByTime, fromdate, todate, cardgroup, lane, user, strCG, page, pageSize, ref totalItem).ToList();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportIn>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CheckPlateDT = GetCheckPlate().ToDataTableNullable();
            ViewBag.checkPlate = isCheckPlate;
            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportInFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số hợp lệ", ItemValue = "Biển số hợp lệ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["expiredDate"], ItemValue = "Ngày hết hạn" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["remainingTime"], ItemValue = "Số ngày còn lại" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        public string GetExpireDate(string CardNumber)
        {
            DataTable dtCard = ExcuteSQL.GetDataSet(string.Format("select ExpireDate from tblCard where CardNumber = '{0}'", CardNumber), false).Tables[0];

            if (dtCard != null && dtCard.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCard.Rows[0]["ExpireDate"].ToString()))
                    return DateTime.Parse(dtCard.Rows[0]["ExpireDate"].ToString()).ToString("dd/MM/yyyy");
                else
                    return dtCard.Rows[0]["ExpireDate"].ToString();
            }

            return "";
        }
        /// <summary>
        /// Lấy type card
        /// </summary>
        /// <param name="CardNumber"></param>
        /// <returns></returns>
        public string GetTypeCard(string cardgroupId)
        {
            //DataTable dtCard = ExcuteSQL.GetDataSet(string.Format("select CardGroupID from tblCard where CardNumber = '{0}'", CardNumber), false).Tables[0];

            //var cardGroupID = "";

            //if (dtCard != null && dtCard.Rows.Count > 0)
            //{
            //    cardGroupID = dtCard.Rows[0]["CardGroupID"].ToString();
            //}

            if (!string.IsNullOrEmpty(cardgroupId) && !cardgroupId.Equals("LOOP_D") && !cardgroupId.Equals("LOOP_M"))
            {
                DataTable dtCardGroup = ExcuteSQL.GetDataSet(string.Format("select CardType from tblCardGroup where CardGroupID = CONVERT(nvarchar(50), '{0}')", cardgroupId), false).Tables[0];
                if (dtCardGroup != null && dtCardGroup.Rows.Count > 0)
                {
                    return dtCardGroup.Rows[0]["CardType"].ToString();
                }
            }


            if (!string.IsNullOrEmpty(cardgroupId) && (cardgroupId.Equals("LOOP_D") || cardgroupId.Equals("LOOP_M")))
            {
                return cardgroupId;
            }

            return "";
        }
        /// <summary>
        /// Lấy ngày còn lại
        /// </summary>
        /// <param name="CardNumber"></param>
        /// <returns></returns>
        public string GetDays(string CardNumber, string cardgroupId)
        {
            double days = 0;

            string endDay = GetExpireDate(CardNumber);

            if (!string.IsNullOrEmpty(endDay))
            {
                days = (Convert.ToDateTime(endDay).Date - DateTime.Now.Date).TotalDays;
            }

            if (days < 0)
                days = 0;

            string typeCard = GetTypeCard(cardgroupId);

            if (typeCard.Equals("1") || string.IsNullOrEmpty(typeCard) || typeCard.Equals("LOOP_D"))
            {
                return "";
            }
            else
            {
                return days.ToString();
            }

        }
        #endregion

        #region Tổng hợp xe trong bãi thời điểm bất kỳ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleInAnyTime(string datefrompicker = "", string fromdate = "", string chkExport = "0", int page = 1)
        {
            var pageSize = 20;
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportVehicleInAnyTime");
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleInAnyTime(fromdate, page, pageSize);

                DataColumn Col = listExcel.Columns.Add("RowNumber", typeof(int));

                Col.SetOrdinal(0);
                int count = 0;
                foreach (DataRow dr in listExcel.Rows)
                {
                    count++;
                    dr["RowNumber"] = count;
                }

                listExcel.Columns.Remove("VehicleGroupID");
                //Xuất file theo format
                ReportVehicleInAnyTimeFormatCell(listExcel, Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], fromdate);

                return RedirectToAction("ReportVehicleInAnyTime");
            }
            #endregion


            var list = _ReportService.GetReportVehicleInAnyTime(fromdate, page, pageSize);

            ViewBag.DateFromPickerValue = !string.IsNullOrWhiteSpace(fromdate) ? Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy HH:mm:59");

            var table = Data.Event.SqlHelper.ExcuteSQLEvent.ConvertTo<ReportVehicleInAnyTime>(list);

            return View(table);
        }

        #region Format cell
        private void ReportVehicleInAnyTimeFormatCell(DataTable dt, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportVehicleInAnyTime");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["groupName"], ItemValue = "VehicleGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["quantity"], ItemValue = "VehicleCount" });

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Chi tiết xe trong bãi thời điểm bất kỳ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailVehicleInAnyTime(string customergroup = "", string key = "", string user = "", string cardgroup = "", string vehiclegroupid = "", string lane = "", string chkExport = "0", string fromdate = "", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (!string.IsNullOrWhiteSpace(fromdate))
            {
                datefrompicker = fromdate;
            }

            var lstVehicleGroupID = new List<string>();
            if (!string.IsNullOrEmpty(vehiclegroupid))
            {
                var t = vehiclegroupid.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    foreach (var item in t)
                    {
                        lstVehicleGroupID.Add(item);
                    }
                }
            }
            //if (!string.IsNullOrEmpty(vehiclegroupid.Trim()))
            //{
            //    lstVehicleGroupID.Add(vehiclegroupid);
            //}
            //else
            //{
            //    DataTable dtVehicleGroup = GetVehicleGroupList().ToDataTableNullable();
            //    if (dtVehicleGroup != null && dtVehicleGroup.Rows.Count > 0)
            //    {
            //        foreach (DataRow item in dtVehicleGroup.Rows)
            //        {
            //            if (!string.IsNullOrEmpty(item["VehicleGroupID"].ToString().Trim()))
            //            {
            //                lstVehicleGroupID.Add(item["VehicleGroupID"].ToString());
            //            }

            //        }
            //    }
            //}

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportDetailVehicleInAnyTime_Excel(key, fromdate, cardgroup, lstVehicleGroupID, lane, user, strCG, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    if (!string.IsNullOrEmpty(item["Làn vào"].ToString()))
                    {
                        var _lane = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                        if (_lane != null)
                        {
                            item["Làn vào"] = _lane.LaneName;
                        }
                        else
                        {
                            item["Làn vào"] = "";
                        }
                    }


                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _user = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_user != null)
                    {
                        item["Giám sát vào"] = _user.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }

                    // item["DateTimeIn"] = item["DateTimeIn"].ToString();
                }

                //Xuất file theo format
                ReportDetailVehicleInAnyTimeFormatCell(listExcel, "Chi_tiết_xe_trong_bãi_tại_thời_điểm_bất_kỳ", "Sheet1", "", "Chi tiết xe trong bãi tại thời điểm bất kỳ", datefrompicker);

                return RedirectToAction("ReportDetailVehicleInAnyTime");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetReportDetailVehicleInAnyTime(key, fromdate, cardgroup, lstVehicleGroupID, lane, user, strCG, page, pageSize, ref totalItem).ToList();

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportIn>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            //ViewBag.VehicleGroups = GetVehicleGroupList().ToList();
            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            //ViewBag.VehicleGroups = GetVehicleGroupList().ToList();
            //ViewBag.VehicleGroupId = vehiclegroupid;

            //ViewBag.DateFromPickerValue = datefrompicker 
            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = !string.IsNullOrWhiteSpace(fromdate) ? Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy HH:mm:59"); ;

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportDetailVehicleInAnyTimeFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = " đến " + titleTime.Split(new[] { '-' })[0];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "Giám sát vào" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Xe ra khỏi bãi

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportInOut(string isCheckPlate,string number1 ,string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string excelcol = "", bool IsFilterByTimeIn = false, string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportInOut");
            var strCG = new List<string>();
            //var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":","") : String.Empty;
            GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportInOut_Excel(isCheckPlate, number1,key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }
                    //////////////
                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }
                    var checkPlate = item["Biển số hợp lệ"].ToString();
                    //item["Biển số hợp lệ"] = item["Biển số hợp lệ"].ToString() == "FALSE" ? 0 : 1;
                    //if (checkPlate === )
                    //{

                    //}
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                }

                //Xuất file theo format
                ReportInOutFormatCell(listExcel, excelcol, Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportInOut");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportInOut(isCheckPlate, number1 ,key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize, ref totalItem).ToList();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }

            var gridModel = PageModelCustom<ReportInOut>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Vouchers = GetVouchers().ToList();
            ViewBag.VouchersDT = GetVouchers().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;
            ViewBag.CheckPlateDT = GetCheckPlate().ToDataTableNullable();
            ViewBag.checkPlate = isCheckPlate;
            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;
            ViewBag.Number = number1;
            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.isFilterByTimeIn = IsFilterByTimeIn;

            return View(gridModel);
            #endregion
        }
        public List<SelectListModel> GetCheckPlate()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            list.Add(new SelectListModel { ItemValue = "0", ItemText = "Không hợp lệ" });
            list.Add(new SelectListModel { ItemValue = "1", ItemText = "Hợp lệ" });

            return list;
        }

        private void ReportInOutFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportInOut");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số hợp lệ", ItemValue = "Biển số hợp lệ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Loại giảm phí", ItemValue = "Loại giảm phí" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền giảm", ItemValue = "Số tiền giảm" });
            listColumn.Add(new SelectListModel { ItemText = "Tiền thu thực tế", ItemValue = "Tiền thu thực tế" });
            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }


        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportInOutByFilter(string fromdate = "05/01/2021 00:00:00", string todate = "05/01/2022 23:59:59", int page = 1, string chkExport = "0", string key = "", string parkingFee = "", string DCTypeNames = "", string LaneIns = "" , string LaneOuts = "", string CardGr = "")
        {
            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.Cardgrid = CardGr;

            ViewBag.LanIns = GetLaneListNew().ToList();
            ViewBag.LaneInDT = GetLaneListNew().ToDataTableNullable();
            ViewBag.LaneIn = LaneIns;
            ViewBag.LaneOut = LaneOuts;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();

            ViewBag.CustomerGroups = GetMenuList();

            //ViewBag.DCType = GetAllDiscountParking().ToList();
            //ViewBag.DCTypeDT = GetAllDiscountParking().ToDataTableNullable();
            //ViewBag.DiscountParkingId = parkingFee;

            ViewBag.DCType = GetAllDiscountParking().ToList();
            ViewBag.DCTypeDT = GetAllDiscountParking().ToDataTableNullable();
            ViewBag.DiscountParkingId = parkingFee;
            var datefrompicker = "";
            if (!string.IsNullOrEmpty(key))
            {
                var arr = key.Split('|');

                if (arr != null && arr.Length > 0)
                {
                    foreach (var item in arr)
                    {
                        var mo = item.Split('$');

                        if (mo.Length == 2)
                        {
                            switch (mo[0])
                            {
                                case "DateTimeIn":

                                    if (!string.IsNullOrWhiteSpace(mo[1]))
                                    {
                                        var arrTime = mo[1].Split('-');

                                        if (arrTime != null && arrTime.Length > 1)
                                        {


                                            if (!string.IsNullOrEmpty(arrTime[0]))
                                            {
                                                fromdate = Convert.ToDateTime(arrTime[0].Trim()).ToString("yyyy/MM/dd HH:mm:ss");
                                            }
                                            if (!string.IsNullOrEmpty(arrTime[1]))
                                            {
                                                todate = Convert.ToDateTime(arrTime[1].Trim()).ToString("yyyy/MM/dd HH:mm:ss");
                                            }

                                        }
                                    }
                                    break;


                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportInOut");
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var lst = new List<ReportInOut_Excel>();
                var listExcel = _ReportService.GetReportFilterInOut_Excel(key, DCTypeNames , CardGr, LaneIns,LaneOuts);
                foreach (var item in listExcel)
                {
                    var obj = new ReportInOut_Excel();
                    obj.RowNumber = item.RowNumber;
                    obj.CardNo = item.CardNo;
                    obj.CardNumber = item.CardNumber;
                    obj.DateTimeIn = item.DateTimeIn;
                    obj.DateTimeOut = item.DateTimeOut;
                    obj.Plate = item.Plate;
                    obj.CustomerName = item.CustomerName;
                    obj.DCTypeName = item.DCTypeName;
                    obj.Moneys = long.Parse(item.Moneys).ToString("###,###.##") != "" ? long.Parse(item.Moneys).ToString("###,###.##") : "0";
                    obj.ReducedMoney = long.Parse(item.ReducedMoney).ToString("###,###.##") != "" ? long.Parse(item.ReducedMoney).ToString("###,###.##") : "0";
                    obj.RestMoney = (long.Parse(item.Moneys) - long.Parse(item.ReducedMoney)).ToString("###,###.##") != "" ? (long.Parse(item.Moneys) - long.Parse(item.ReducedMoney)).ToString("###,###.##") : "0";
                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item.LaneIDIn));
                    if (_laneIn != null)
                    {

                        obj.LaneInName = _laneIn.LaneName;
                    }
                    else
                    {
                        obj.LaneInName = "";
                    }

                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item.LaneIDOut));
                    if (_laneOut != null)
                    {
                        obj.LaneOutName = _laneOut.LaneName;
                    }
                    else
                    {
                        obj.LaneOutName = "";
                    }

                    if (item.CardGroupID != null)
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item.CardGroupID));
                        if (_cardgroup != null)
                        {
                            obj.CardGroupName = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            obj.CardGroupName = "";
                        }

                    }


                    var _userIn = _UserService.GetById((item.UserIDIn));
                    if (_userIn != null)
                    {
                        obj.UserInName = _userIn.Username;
                    }
                    else
                    {
                        obj.UserInName = "";
                    }
                    var _userOut = _UserService.GetById(item.UserIDOut);
                    if (_userOut != null)
                    {
                        obj.UserOutName = _userOut.Username;
                    }
                    else
                    {
                        obj.UserOutName = "";
                    }
                    lst.Add(obj);
                }
                double totalMoney = 0;
                double totalRedueMoney = 0;
                double TotalRestMoney = 0;
                if (lst != null && lst.Count > 0)
                {



                    foreach (var item in lst)
                    {
                        if (!string.IsNullOrEmpty(item.Moneys))
                        {
                            var money = item.Moneys.Replace(".", "").Replace(",", "");

                            totalMoney += Convert.ToDouble(money);
                        }
                        if (!string.IsNullOrEmpty(item.ReducedMoney))
                        {
                            var money1 = item.ReducedMoney.Replace(".", "").Replace(",", "");

                            totalRedueMoney += Convert.ToDouble(money1);
                        }
                        TotalRestMoney = totalMoney - totalRedueMoney;
                    }

                    var totalExcel = new ReportInOut_Excel()
                    {
                        CardNo = "Tổng",
                        Moneys = totalMoney > 0 ? totalMoney.ToString("###,###") : "0",
                        ReducedMoney = totalRedueMoney > 0 ? totalRedueMoney.ToString("###,###") : "0",
                        RestMoney = TotalRestMoney > 0 ? TotalRestMoney.ToString("###,###") : "0",

                    };


                    lst.Add(totalExcel);
                }
                DataTable dt = lst.ToDataTableNullable();
                //Xuất file theo format
                ReportInOutFormatCell(dt, "", Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportInOutByFilter");
            }
            #endregion
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View();
        }

        private IEnumerable<tblDiscountParking> GetAllDiscountParking()
        {
            return _ReduceParkingFeesService.GetAll();
        }

        public PartialViewResult Partial_InOutFilter(string key = "", int page = 1, string discount = "" , string LanIns ="" , string LaneOuts = "" , string Cardgrs ="")
        {
            var strCG = new List<string>();

            var totalItem = 0;
            var pageSize = 20;

            var list = _ReportService.ReportInOutFilter(key, discount,LanIns , LaneOuts, Cardgrs, page, pageSize, ref totalItem).ToList();
            double totalMoney = 0;
            double totalRedueMoney = 0;
            double TotalRestMoney = 0;
            if (list != null && list.Count > 0)
            {



                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.Moneys))
                    {
                        var money = item.Moneys.Replace(".", "").Replace(",", "");

                        totalMoney += Convert.ToDouble(money);
                    }
                    if (!string.IsNullOrEmpty(item.ReducedMoney))
                    {
                        var money1 = item.ReducedMoney.Replace(".", "").Replace(",", "");

                        totalRedueMoney += Convert.ToDouble(money1);
                    }
                    TotalRestMoney = totalMoney - totalRedueMoney;
                }

                //var totalExcel = new AEON_ReportOutDetail_Excel
                //{
                //    CardNo = "Tổng",
                //    MoneyValue = total > 0 ? total.ToString("###,###") : "0"
                //};

                ViewBag.totalMoney = totalMoney;
                ViewBag.totalRedue = totalRedueMoney;
                ViewBag.totalRest = TotalRestMoney;
                //listExcel.Add(totalExcel);
            }

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }

            var gridModel = PageModelCustom<ReportInOut>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();

            ViewBag.Users = GetUserList().ToList();

            return PartialView(gridModel);
        }

        #endregion

        #region Xe vào bãi

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleComeIn(string isCheckPlate, string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var strCG = new List<string>();
            //var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":","") : String.Empty;
            GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleComeInExcel(isCheckPlate,key, strCG, fromdate, todate, cardgroup, lane, user, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                  
                }

                //Xuất file theo format
                ReportVehicleComeInFormatCell(listExcel, Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportVehicleComeIn");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportVehicleComeIn(isCheckPlate,key, strCG, fromdate, todate, cardgroup, lane, user, page, pageSize, ref totalItem).ToList();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportVehicleComeIn>.GetPage(list, page, pageSize, totalItem);
            ViewBag.CheckPlateDT = GetCheckPlate().ToDataTableNullable();
            ViewBag.checkPlate = isCheckPlate;
            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }

        private void ReportVehicleComeInFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số hợp lệ", ItemValue = "Biển số hợp lệ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Chi tiết thu tiền thẻ lượt
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailMoneyCardDay(string vouchers ,string   number1 = "" ,string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");

            if (vouchers == "System.Collections.Generic.List`1[Kztek.Model.CustomModel.SelectListModel]")
            {
                vouchers = "";
            }
            var totalItem = 0;
           
            long totalMoneytitular = 0;
            long totalMoneyReduce = 0;
            long totalMoneyReal = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportDetailMoneyCardDayExcel(vouchers, number1, key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var s = item["Nhóm thẻ"].ToString();
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                    long _totalMoneytitular = 0;
                    long _totalMoneyReduce = 0;
                    long _totalMoneyReal = 0;

                    long.TryParse(item["Tiền danh nghĩa"].ToString(), out _totalMoneytitular); 

                    long.TryParse(item["Số tiền giảm"].ToString(), out _totalMoneyReduce);
                    _totalMoneyReal = _totalMoneytitular - _totalMoneyReduce;
                    item["Số tiền thực tế"] = _totalMoneyReal.ToString();
                   


                    totalMoneytitular += _totalMoneytitular;
                    totalMoneyReduce += _totalMoneyReduce;
                    totalMoneyReal += _totalMoneyReal;
                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát ra"] = Dictionary["total"];
                totalMoneyRow["Tiền danh nghĩa"] = totalMoneytitular.ToString();
                totalMoneyRow["Số tiền giảm"] = totalMoneyReduce.ToString();
                totalMoneyRow["Số tiền thực tế"] = totalMoneyReal.ToString();
                listExcel.Rows.Add(totalMoneyRow);


                //Xuất file theo format
                ReportDetailMoneyCardDayFormatCell(listExcel, "", Dictionary["titleExcell"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportDetailMoneyCardDay");
            }
            #endregion

            //if (chkExport == "2")
            //{
            //    return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            //}

            #region Giao diện
            var list = _ReportService.GetReportDetailMoneyCardDay(vouchers, number1, key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoneytitular , ref totalMoneyReduce);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActive().ToList();
              
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.TotalMoney = totalMoneytitular;
            ViewBag.TotalReduce = totalMoneyReduce;
          

            ViewBag.Vouchers = GetVouchers().ToList();
            ViewBag.VouchersDT = GetVouchers().ToDataTableNullable();
            ViewBag.VoucherId = vouchers;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoneytitular;
            ViewBag.Number = number1;
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            return View(gridModel);
            #endregion
        }

        public ActionResult PrintReportDetailMoneyCardDay(string vouchers, string number1, string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            long totalMoney = 0;
            long totalReduce = 0;
            var list = _ReportService.GetReportDetailMoneyCardDay(vouchers, number1,key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney, ref totalReduce);

            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;
            ViewBag.TotalRedueMoney = totalReduce;
            ViewBag.FilterLink = $"/Parking/Report/PrintReportDetailMoneyCardDay?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";
            ViewBag.System = _tblSystemConfigService.GetDefault();
            ViewBag.PageSize = pageSize;

            return View(gridModel);
        }

        private void ReportDetailMoneyCardDayFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " :" + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Tên voucher", ItemValue = "Tên voucher" });
            
             listColumn.Add(new SelectListModel { ItemText = "Tiền danh nghĩa", ItemValue = "Tiền danh nghĩa" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền giảm", ItemValue = "Số tiền giảm" });
             listColumn.Add(new SelectListModel { ItemText = "Số tiền thực tế", ItemValue = "Số tiền thực tế" });

            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeAll"], ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file

            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username, 200);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }
        #endregion

        #region Chi tiết thu tiền thẻ lượt TRANSERCO
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailMoneyCardDayTRANSERCO(string voucher ,string number1, string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var totalItem = 0;
            long totalMoney = 0;
            long totalReduceMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            string number = "";
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportDetailMoneyCardDayExcel(voucher, number,key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    // var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                    long _totalMoney = 0;

                    long.TryParse(item["Tiền"].ToString(), out _totalMoney);

                    totalMoney += _totalMoney;
                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát ra"] = "Tổng tiền";
                totalMoneyRow["Tiền"] = totalMoney.ToString();
                listExcel.Rows.Add(totalMoneyRow);

                var titlereport = _tblLaneService.GetTitle(lane);

                //Xuất file theo format
                ReportDetailMoneyCardDayFormatCell(listExcel, "", string.Format("Chi_tiết_thu_tiền_thẻ_lượt{0}", string.IsNullOrEmpty(lane) ? "" : "_" + titlereport.Replace(' ', '_').Replace('-', '_')), "Sheet1", "", string.Format("Chi tiết thu tiền thẻ lượt {0}", string.IsNullOrEmpty(lane) ? "" : titlereport), datefrompicker);

                return RedirectToAction("ReportDetailMoneyCardDayTRANSERCO");
            }
            #endregion

            //if (chkExport == "2")
            //{
            //    return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            //}

            #region Giao diện
            var list = _ReportService.GetReportDetailMoneyCardDay(voucher,number1, key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney,ref totalReduceMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            ViewBag.selectedEventValue = GetSetFromSessionCardDay(null);

            return View(gridModel);
            #endregion
        }

        //TRƯỜNG CHINH
        public ActionResult PrintReportDetailMoneyCardDayTRANSERCO(string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            long totalMoney = 0;

            var list = _ReportService.GetReportDetailMoneyCardDayTRANSERCO(key, user, fromdate, todate, cardgroup, lane, ref totalItem, ref totalMoney);

            //var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;
            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalMoney, " đồng");

            ViewBag.TitleReport = string.IsNullOrEmpty(lane) ? "" : _tblLaneService.GetTitle(lane);

            ViewBag.FilterLink = $"/Parking/Report/PrintReportDetailMoneyCardDayTRANSERCO?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";

            ViewBag.PageSize = pageSize;

            return View(list);
        }

        private void ReportDetailMoneyCardDayFormatCellTRANSERCO(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian ra", ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = "Làn ra", ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát ra", ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền", ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng thời gian", ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }


        public JsonResult AddOrRemoveOneAllCardDaySeleted(List<string> Id, bool isAdd)
        {
            GetSetFromSessionCardDay(Id, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSessionCardDay(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listEventId = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.EventIdCardDayActionParkingSession, host)];
            if (listEventId != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Add(item);
                            }
                        }
                        else
                        {
                            if (listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listEventId = list;
                }
                else
                {
                    listEventId = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.EventIdCardDayActionParkingSession, host)] = listEventId;

            return listEventId;
        }

        public PartialViewResult ModalSelectedCardDay(int totalItem = 0, string url = "")
        {
            var listSelected = GetSetFromSessionCardDay(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listSelected);
        }

        public JsonResult RemoveAllCardDaySeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.EventIdCardDayActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult FreePermission()
        {
            var listSelected = GetSetFromSessionCardDay(null);

            _ReportService.UpdateFreeMoneyEvent(listSelected);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult PayLater()
        {
            var listSelected = GetSetFromSessionCardDay(null);

            _ReportService.UpdateEventPayLater(listSelected);

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách sự kiện trả sau TRANSERCO
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportEventPayLaterTRANSERCO(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportEventPayLaterTRANSERCOExcel(key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    // var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                    long _totalMoney = 0;

                    long.TryParse(item["Tiền"].ToString(), out _totalMoney);

                    totalMoney += _totalMoney;
                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát ra"] = "Tổng tiền";
                totalMoneyRow["Tiền"] = totalMoney.ToString();
                listExcel.Rows.Add(totalMoneyRow);


                //Xuất file theo format
                ReportEventPayLaterFormatCellTRANSERCO(listExcel, "", "Danh_sách_sự_kiện_trả_sau", "Sheet1", "", "Danh sách sự kiện trả sau", datefrompicker);

                return RedirectToAction("ReportEventPayLaterTRANSERCO");
            }
            #endregion        

            #region Giao diện
            var list = _ReportService.GetReportEventPayLaterTRANSERCO(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            ViewBag.selectedEventValue = GetSetFromSessionCardDay(null);

            return View(gridModel);
            #endregion
        }

        //TRƯỜNG CHINH
        public ActionResult PrintReportEventPayLaterTRANSERCO(string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            long totalMoney = 0;

            var list = _ReportService.GetReportEventPayLaterTRANSERCO_Print(key, user, fromdate, todate, cardgroup, lane, ref totalItem, ref totalMoney);

            //var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;
            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalMoney, " đồng");

            ViewBag.FilterLink = $"/Parking/Report/PrintReportEventPayLaterTRANSERCO?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";

            ViewBag.PageSize = pageSize;

            return View(list);
        }

        private void ReportEventPayLaterFormatCellTRANSERCO(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian ra", ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = "Làn ra", ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát ra", ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền", ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng thời gian", ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }


        public JsonResult AddOrRemoveOneAllPayLaterSeleted(List<string> Id, bool isAdd)
        {
            GetSetFromSessionPayLater(Id, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSessionPayLater(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listEventId = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.EventIdPayLaterActionParkingSession, host)];
            if (listEventId != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Add(item);
                            }
                        }
                        else
                        {
                            if (listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listEventId = list;
                }
                else
                {
                    listEventId = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.EventIdPayLaterActionParkingSession, host)] = listEventId;

            return listEventId;
        }

        public PartialViewResult ModalSelectedPayLater(int totalItem = 0, string url = "")
        {
            var listSelected = GetSetFromSessionPayLater(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listSelected);
        }

        public JsonResult RemoveAllPayLaterSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.EventIdPayLaterActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemovePayLater()
        {
            var listSelected = GetSetFromSessionPayLater(null);

            _ReportService.RemoveEventPayLater(listSelected);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Tổng hợp thu tiền thẻ lượt theo nhóm thẻ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyByCardGroup(string cardgroup = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {

            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByCardGroup");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportTotalMoneyByCardGroupExcel(cardgroup, fromdate, todate, true);

                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach (DataRow dr in listExcel.Rows)
                    {
                        if (dr["Moneys"] != null && long.Parse(dr["Moneys"].ToString()) > 0)
                        {
                            dr["Moneys"] = string.Format("{0:n0}", long.Parse(dr["Moneys"].ToString()));
                        }

                        if (dr["Count"] != null && long.Parse(dr["Count"].ToString()) > 0)
                        {
                            dr["Count"] = string.Format("{0:n0}", long.Parse(dr["Count"].ToString()));
                        }
                    }
                }

                //Xuất file theo format
                ReportTotalMoneyByGroupFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyByCardGroup");
            }
            #endregion

            //Print
            if (chkExport == "2")
            {
                return RedirectToAction("PrintReportTotalMoneyByCardGroup", new { cardgroup = cardgroup, fromdate = fromdate, todate = todate });
            }

            #region Giao diện

            var list = _ReportService.GetReportTotalMoneyByCardGroup(cardgroup, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByCardGroup>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }
        private void ReportTotalMoneyByGroupFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByCardGroup");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["quantity"], ItemValue = "Số lượng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Số tiền" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }

        public ActionResult PrintReportTotalMoneyByCardGroup(string cardgroup = "", string fromdate = "", string todate = "")
        {
            var totalItem = 0;
            var pageSize = 50;

            var list = _ReportService.GetReportTotalMoneyByCardGroup(cardgroup, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByCardGroup>.GetPage(list, 1, pageSize, totalItem);

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(gridModel);
        }
        #endregion

        #region Tổng hợp thu tiền thẻ lượt theo làn đường

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyByLane(string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByLane");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportTotalMoneyByLaneExcel(lane, fromdate, todate, true);

                //Xuất file theo format
                ReportTotalMoneyByLaneFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyByCardGroup");
            }
            #endregion

            //Print
            if (chkExport == "2")
            {
                return RedirectToAction("PrintReportTotalMoneyByLane", new { lane = lane, fromdate = fromdate, todate = todate });
            }

            #region Giao diện
            var list = _ReportService.GetReportTotalMoneyByLane(lane, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByLane>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }
        private void ReportTotalMoneyByLaneFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByLane");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lane"], ItemValue = "Làn đường" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Số tiền" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }

        public ActionResult PrintReportTotalMoneyByLane(string lane = "", string fromdate = "", string todate = "")
        {
            var totalItem = 0;
            var pageSize = 50;

            var list = _ReportService.GetReportTotalMoneyByLane(lane, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByLane>.GetPage(list, 1, pageSize, totalItem);

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(gridModel);
        }
        #endregion

        #region Tổng hợp thu tiền thẻ lượt theo người dùng

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyByUser(string user = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {

            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByUser");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportTotalMoneyByUserExcel(user, fromdate, todate, true);

                //Xuất file theo format
                ReportTotalMoneyByUserFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyByCardGroup");
            }
            #endregion

            if (chkExport == "2")
            {
                return RedirectToAction("PrintReportTotalMoneyByUser", new { user = user, fromdate = fromdate, todate = todate });
            }

            #region Giao diện
            var list = _ReportService.GetReportTotalMoneyByUser(user, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByUser>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }
        private void ReportTotalMoneyByUserFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByUser");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["user"], ItemValue = "Người dùng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Số tiền" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            //Xuất file
            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }

        public ActionResult PrintReportTotalMoneyByUser(string user, string fromdate, string todate)
        {
            var totalItem = 0;
            var pageSize = 50;

            var list = _ReportService.GetReportTotalMoneyByUser(user, fromdate, todate);
            totalItem = list.Count;
            var gridModel = PageModelCustom<ReportTotalMoneyByUser>.GetPage(list, 1, pageSize, totalItem);

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(gridModel);
        }
        #endregion


        #region Báo cáo theo voucher

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportReduceMoneyByVoucher(string key = "", string fromdate = "", string todate = "" , string chkExport = "0")
        {
            var title = "Tổng số tiền giảm theo voucher";
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var strCG = new List<string>();
            var datefrompicker = "";
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
     
            long totalMoney = 0;

            #region Excel
            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportReduceMoneyByVochouer_Excel(key, fromdate, todate);

                //Xuất file theo format
                ReportReduceMoneyByVochouerFormatCell(listExcel, "", "Sheet1", "", title, datefrompicker);

                return RedirectToAction("ReportReduceMoneyByVochouer");
            }
            #endregion


            var list = _ReportService.GetReportReduceMoneyByVochouer(key, fromdate, todate);

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

       

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
        }

        #region Format cell lên excel
        private void ReportReduceMoneyByVochouerFormatCell(List<ReportReduceMoneyByVoucher_Excel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {

            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Tên voucher", ItemValue = "Tên voucher" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền", ItemValue = "Số tiền" });
   

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion
        #region Chi tiết thu tiền thẻ tháng

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailMoneyCardMonth(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", int page = 1, string column = "DateTimeOut", string chkExport = "0", string fromdate = "", string todate = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardMonthTRANSERCO");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            #region Excel
            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportDetailMoneyCardMonth_Excel(key, fromdate, todate, cardgroup, "", strCG, user, page, pageSize);

                long _totalmoneys = 0;

                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach (DataRow item in listExcel.Rows)
                    {
                        if (!string.IsNullOrEmpty(item["Phí(VNĐ)"].ToString()))
                        {
                            _totalmoneys = _totalmoneys + long.Parse(item["Phí(VNĐ)"].ToString());
                        }

                        //if(!string.IsNullOrEmpty(item["Phí(VNĐ)"].ToString()) && !item["Phí(VNĐ)"].ToString().Equals("0"))
                        //{
                        //    item["Phí(VNĐ)"] = Convert.ToDecimal(item["Phí(VNĐ)"].ToString()).ToString("###,###");
                        //}

                        if (!string.IsNullOrEmpty(item["Nhóm thẻ"].ToString()))
                        {
                            var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                            if (_cardgroup != null)
                            {
                                item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                            }
                            else
                            {
                                item["Nhóm thẻ"] = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(item["Nhóm khách hàng"].ToString()) && !item["Nhóm khách hàng"].ToString().Equals("0"))
                        {
                            var _customergroup = _tblCustomerGroupService.GetById(Guid.Parse(item["Nhóm khách hàng"].ToString()));
                            if (_customergroup != null)
                            {
                                item["Nhóm khách hàng"] = _customergroup.CustomerGroupName;
                            }
                            else
                            {
                                item["Nhóm khách hàng"] = "";
                            }
                        }
                        else
                        {
                            item["Nhóm khách hàng"] = "";
                        }

                        var _user = _UserService.GetById(item["NV thực hiện"].ToString());
                        if (_user != null)
                        {
                            item["NV thực hiện"] = _user.Username;
                        }
                        else
                        {
                            item["NV thực hiện"] = "";
                        }

                        //  item["Thời hạn cũ"] = Convert.ToDateTime(item["Thời hạn cũ"]).ToString("dd/MM/yyy HH:mm:ss");
                        //  item["Thời hạn mới"] = Convert.ToDateTime(item["Thời hạn mới"]).ToString("dd/MM/yyy HH:mm:ss");
                        //  item["Ngày thực hiện"] = Convert.ToDateTime(item["Ngày thực hiện"]).ToString("dd/MM/yyy HH:mm:ss");

                    }
                }


                listExcel.Rows.Add(null, "#", DictionarySearch["total"], "", "", "", "", "", "", "", _totalmoneys, "", "");

                //Xuất file theo format
                ReportDetailMoneyCardMonthFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportDetailMoneyCardMonth");
            }
            #endregion


            var list = _ReportService.GetReportDetailMoneyCardMonth(key, fromdate, todate, cardgroup, "", strCG, user, page, pageSize, ref totalItem, ref totalMoney);


            var gridModel = PageModelCustom<ReportDetailMoneyCardMonth>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListMonth().ToList();
            ViewBag.CardGroupDT = GetCardGroupListMonth().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupsC = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        public ActionResult PrintReportDetailMoneyCardMonth(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", string column = "DateTimeOut", string fromdate = "", string todate = "", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (!string.IsNullOrEmpty(customergroup))
            {
                listCustomerGroupId = _ReportService.GetChildCustomerGroupByparent(customergroup);
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            var list = _ReportService.PrintReportDetailMoneyCardMonth(key, fromdate, todate, cardgroup, customer, strCG, user, page, pageSize, ref totalItem, ref totalMoney);


            //   var gridModel = PageModelCustom<ReportDetailMoneyCardMonth>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(list);
        }


        #region Format cell lên excel
        private void ReportDetailMoneyCardMonthFormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardMonthTRANSERCO");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customerGroup"], ItemValue = "Nhóm khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["address"], ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["durationOld"], ItemValue = "Thời hạn cũ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["durationNew"], ItemValue = "Thời hạn mới" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Phí(VNĐ)" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["staffMade"], ItemValue = "NV thực hiện" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["executionDate"], ItemValue = "Ngày thực hiện" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Chi tiết thu tiền thẻ tháng TRANSERCO

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailMoneyCardMonthTRANSERCO(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", int page = 1, string column = "DateTimeOut", string chkExport = "0", string fromdate = "", string todate = "")
        {

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            #region Excel
            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportDetailMoneyCardMonth_ExcelTRANSERCO(key, fromdate, todate, cardgroup, "", strCG, user, page, pageSize);

                long _totalmoneys = 0;

                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach (DataRow item in listExcel.Rows)
                    {
                        if (!string.IsNullOrEmpty(item["Phí(VNĐ)"].ToString()))
                        {
                            _totalmoneys = _totalmoneys + long.Parse(item["Phí(VNĐ)"].ToString());
                        }

                        //if(!string.IsNullOrEmpty(item["Phí(VNĐ)"].ToString()) && !item["Phí(VNĐ)"].ToString().Equals("0"))
                        //{
                        //    item["Phí(VNĐ)"] = Convert.ToDecimal(item["Phí(VNĐ)"].ToString()).ToString("###,###");
                        //}

                        if (!string.IsNullOrEmpty(item["Nhóm thẻ"].ToString()))
                        {
                            var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                            if (_cardgroup != null)
                            {
                                item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                            }
                            else
                            {
                                item["Nhóm thẻ"] = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(item["Nhóm khách hàng"].ToString()) && !item["Nhóm khách hàng"].ToString().Equals("0"))
                        {
                            var _customergroup = _tblCustomerGroupService.GetById(Guid.Parse(item["Nhóm khách hàng"].ToString()));
                            if (_customergroup != null)
                            {
                                item["Nhóm khách hàng"] = _customergroup.CustomerGroupName;
                            }
                            else
                            {
                                item["Nhóm khách hàng"] = "";
                            }
                        }
                        else
                        {
                            item["Nhóm khách hàng"] = "";
                        }

                        var _user = _UserService.GetById(item["NV thực hiện"].ToString());
                        if (_user != null)
                        {
                            item["NV thực hiện"] = _user.Username;
                        }
                        else
                        {
                            item["NV thực hiện"] = "";
                        }

                        if (item["Thanh toán"].ToString().Contains("TM"))
                        {
                            item["Thanh toán"] = "Tiền mặt";
                        }
                        else
                        {
                            item["Thanh toán"] = "Chuyển khoản";
                        }
                        //  item["Thời hạn cũ"] = Convert.ToDateTime(item["Thời hạn cũ"]).ToString("dd/MM/yyy HH:mm:ss");
                        //  item["Thời hạn mới"] = Convert.ToDateTime(item["Thời hạn mới"]).ToString("dd/MM/yyy HH:mm:ss");
                        //  item["Ngày thực hiện"] = Convert.ToDateTime(item["Ngày thực hiện"]).ToString("dd/MM/yyy HH:mm:ss");

                    }
                }


                listExcel.Rows.Add(null, "#", "TỔNG SỐ", "", "", "", "", "", "", "", _totalmoneys, "", "", "");

                //Xuất file theo format
                ReportDetailMoneyCardMonthFormatCellTRANSERCO(listExcel, "Chi_tiết_thu_tiền_thẻ_tháng", "Sheet1", "", "Chi tiết thu tiền thẻ tháng", datefrompicker);

                return RedirectToAction("ReportDetailMoneyCardMonthTRANSERCO");
            }
            #endregion


            var list = _ReportService.GetReportDetailMoneyCardMonthTRANSERCO(key, fromdate, todate, cardgroup, "", strCG, user, page, pageSize, ref totalItem, ref totalMoney);


            var gridModel = PageModelCustom<ReportDetailMoneyCardMonthTRANSERCO>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListMonth().ToList();
            ViewBag.CardGroupDT = GetCardGroupListMonth().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupsC = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.selectedEventValue = GetSetFromSession(null);

            return View(gridModel);
        }

        public ActionResult PrintReportDetailMoneyCardMonthTRANSERCO(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", string column = "DateTimeOut", string fromdate = "", string todate = "", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (!string.IsNullOrEmpty(customergroup))
            {
                listCustomerGroupId = _ReportService.GetChildCustomerGroupByparent(customergroup);
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            var list = _ReportService.PrintReportDetailMoneyCardMonthTRANSERCO(key, fromdate, todate, cardgroup, customer, strCG, user, page, pageSize, ref totalItem, ref totalMoney);


            //   var gridModel = PageModelCustom<ReportDetailMoneyCardMonth>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalMoney, " đồng");

            return View(list);
        }

        public ActionResult PrintReportDetailMoneyCardMonthTRANSERCO_Company(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", string column = "DateTimeOut", string fromdate = "", string todate = "", int page = 1)
        {
            var listid = GetSetFromSession(null);

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (!string.IsNullOrEmpty(customergroup))
            {
                listCustomerGroupId = _ReportService.GetChildCustomerGroupByparent(customergroup);
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;
            var dtSumMonth = new DataTable();

            var dt = _ReportService.PrintReportDetailMoneyCardMonthTRANSERCO_Company(key, listid, fromdate, todate, cardgroup, customer, strCG, user, page, pageSize, ref totalItem, ref totalMoney, ref dtSumMonth);


            //   var gridModel = PageModelCustom<ReportDetailMoneyCardMonth>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;
            ViewBag.DTSumMonth = dtSumMonth;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";
            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalMoney, " đồng");
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(dt);
        }
        public ActionResult PrintReportDetailMoneyCardMonthTRANSERCO_Personal(string key = "", string user = "", string customer = "", string datefrompicker = "", string cardgroup = "", string customergroup = "", string listCustomerGroupId = "", string column = "DateTimeOut", string fromdate = "", string todate = "", int page = 1)
        {
            var listid = GetSetFromSession(null);

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (!string.IsNullOrEmpty(customergroup))
            {
                listCustomerGroupId = _ReportService.GetChildCustomerGroupByparent(customergroup);
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;
            var dtSumMonth = new DataTable();
            var dt = _ReportService.PrintReportDetailMoneyCardMonthTRANSERCO_Personal(key, listid, fromdate, todate, cardgroup, customer, strCG, user, page, pageSize, ref totalItem, ref totalMoney, ref dtSumMonth);


            //   var gridModel = PageModelCustom<ReportDetailMoneyCardMonth>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;
            ViewBag.DTSumMonth = dtSumMonth;
            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney > 0 ? Convert.ToDecimal(totalMoney).ToString("###,###") : "0";
            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalMoney, " đồng");
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(dt);
        }

        #region Format cell lên excel
        private void ReportDetailMoneyCardMonthFormatCellTRANSERCO(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Mã hợp đồng", ItemValue = "Mã hợp đồng" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "Nhóm khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Thời hạn cũ", ItemValue = "Thời hạn cũ" });
            listColumn.Add(new SelectListModel { ItemText = "Thời hạn mới", ItemValue = "Thời hạn mới" });
            listColumn.Add(new SelectListModel { ItemText = "Phí(VNĐ)", ItemValue = "Phí(VNĐ)" });
            listColumn.Add(new SelectListModel { ItemText = "Thanh toán", ItemValue = "Thanh toán" });
            listColumn.Add(new SelectListModel { ItemText = "NV thực hiện", ItemValue = "NV thực hiện" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày thực hiện", ItemValue = "Ngày thực hiện" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        public JsonResult GetTypeCustomerGroup(string id)
        {
            string action = "PrintReportDetailMoneyCardMonthTRANSERCO";

            if (!string.IsNullOrEmpty(id))
            {
                var customergroup = _tblCustomerGroupService.GetById(Guid.Parse(id));

                if (customergroup != null)
                {
                    action = customergroup.IsCompany ? "PrintReportDetailMoneyCardMonthTRANSERCO_Company" : "PrintReportDetailMoneyCardMonthTRANSERCO_Personal";
                }
            }

            return Json(action, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrRemoveOneAllSeleted(List<string> Id, bool isAdd)
        {
            GetSetFromSession(Id, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSession(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listEventId = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.EventIdActionParkingSession, host)];
            if (listEventId != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Add(item);
                            }
                        }
                        else
                        {
                            if (listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listEventId = list;
                }
                else
                {
                    listEventId = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.EventIdActionParkingSession, host)] = listEventId;

            return listEventId;
        }

        public PartialViewResult ModalSelected(int totalItem = 0, string url = "")
        {
            var listSelected = GetSetFromSession(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listSelected);
        }

        public JsonResult RemoveAllSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.EventIdActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tổng hợp thu tiền thẻ tháng theo nhóm thẻ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyCardMonthByCardGroup(string id = "", string cardgroup = "", string datefrompicker = "", string chkExport = "0", string fromdate = "", string todate = "")
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByCardGroup");

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportTotalMoneyCardMonthByCardGroup_Excel(fromdate, todate, cardgroup);

                //Xuất file theo format
                ReportTotalMoneyByCardGroupFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyCardMonthByCardGroup");
            }

            var list = _ReportService.GetReportTotalMoneyCardMonthByCardGroup(fromdate, todate, cardgroup);

            ViewBag.CardGroups = GetCardGroupListMonth().ToList();
            ViewBag.CardGroupDT = GetCardGroupListMonth().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.DateFromPickerValue = datefrompicker;

            //  ViewBag.totalMoney = list.Sum(n => n.Money);

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
        }

        public ActionResult PrintReportTotalMoneyCardMonthByCardGroup(string id = "", string cardgroup = "", string fromdate = "", string todate = "")
        {

            var list = _ReportService.GetReportTotalMoneyCardMonthByCardGroup(fromdate, todate, cardgroup);
            ViewBag.CardGroups = GetCardGroupList();

            ViewBag.CardGroupId = id;

            //  ViewBag.totalMoney = list.Sum(n => n.Money);

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(list);
        }

        #region Format cell lên excel
        private void ReportTotalMoneyByCardGroupFormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByCardGroup");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroupName"], ItemValue = "GroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Moneys" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Tổng hợp thu tiền thẻ tháng theo nhóm khách hàng
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyCardMonthByCustomerGroup(string customergroup = "", string datefrompicker = "", string chkExport = "0", string fromdate = "", string todate = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByGroupUser");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var strCG = new List<string>();
            GetListChildCustomer(strCG, customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportTotalMoneyCardMonthByCustomerGroup_Excel(fromdate, todate, strCG, customergroup);


                listExcel.Columns.Remove("ParentID");
                listExcel.Columns.Remove("CustomerGroupID");
                listExcel.Columns.Remove("Level");
                listExcel.Columns.Remove("ChkChild");
                DataColumn Col = listExcel.Columns.Add("STT", typeof(string));
                Col.SetOrdinal(0);

                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    int count = 0;
                    double total = 0;
                    foreach (DataRow dr in listExcel.Rows)
                    {
                        count++;
                        dr["STT"] = count;
                        if (!string.IsNullOrEmpty(dr["Moneys"].ToString()))
                        {
                            total += Convert.ToDouble(dr["Moneys"].ToString());
                        }
                        else
                        {
                            dr["Moneys"] = "0";
                        }
                    }

                    listExcel.Rows.Add("#", DictionarySearch["total"], total);
                }
                //Xuất file theo format
                ReportTotalMoneyCardMonthByCustomerGroupFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyCardMonthByCustomerGroup");
            }

            var list = _ReportService.GetReportTotalMoneyCardMonthByCustomerGroup(fromdate, todate, strCG, customergroup);

            ViewBag.CustomerGroups = GetMenuList();

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
        }

        public ActionResult PrintReportTotalMoneyCardMonthByCustomerGroup(string customergroup = "", string datefrompicker = "", string chkExport = "0", string fromdate = "", string todate = "")
        {
            var strCG = new List<string>();
            GetListChildCustomer(strCG, customergroup);

            var list = _ReportService.GetReportTotalMoneyCardMonthByCustomerGroup(fromdate, todate, strCG, customergroup);
            ViewBag.CustomerGroups = GetMenuList();

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.CustomerGroupId = customergroup;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(list);
        }

        #region Format cell lên excel
        private void ReportTotalMoneyCardMonthByCustomerGroupFormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByGroupUser");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cusGrp"], ItemValue = "GroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Moneys" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Tổng hợp thu tiền thẻ tháng theo người dùng
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyCardMonthByUser(string user = "", string datefrompicker = "", string chkExport = "0", string fromdate = "", string todate = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByUser");

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportTotalMoneyCardMonthByUser_Excel(fromdate, todate, user);

                //Xuất file theo format
                ReportTotalMoneyCardMonthByUserFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalMoneyCardMonthByUser");
            }

            var list = _ReportService.GetReportTotalMoneyCardMonthByUser(fromdate, todate, user);
            // ViewBag.CustomerGroups = GetCustomerGroupList();

            ViewBag.DateFromPickerValue = datefrompicker;
            //ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
        }

        public ActionResult PrintReportTotalMoneyCardMonthByUser(string user = "", string datefrompicker = "", string chkExport = "0", string fromdate = "", string todate = "")
        {

            var list = _ReportService.GetReportTotalMoneyCardMonthByUser(fromdate, todate, user);
            // ViewBag.CustomerGroups = GetCustomerGroupList();

            ViewBag.DateFromPickerValue = datefrompicker;
            //ViewBag.CustomerGroupId = customergroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            return View(list);
        }

        #region Format Excel
        private void ReportTotalMoneyCardMonthByUserFormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyCardMonthByUser");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();


            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["userName"], ItemValue = "UserName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Moneys" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo chi tiết xe phát sinh phí phụ trội
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleMoneyByCardMonth(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleMoneyByCardMonth");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";
            string totalmoney = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleMoneyByCardMonthExcel(key, user, cardgroup, fromdate, todate, lane, ref totalmoney);

                foreach (DataRow item in listExcel.Rows)
                {
                    if (!string.IsNullOrEmpty(item["Làn vào"].ToString()))
                    {
                        var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                        if (_laneIn != null)
                        {
                            item["Làn vào"] = _laneIn.LaneName;
                        }
                        else
                        {
                            item["Làn vào"] = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(item["Làn ra"].ToString()))
                    {
                        var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                        if (_laneOut != null)
                        {
                            item["Làn ra"] = _laneOut.LaneName;
                        }
                        else
                        {
                            item["Làn ra"] = "";
                        }
                    }


                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }
                    if (!string.IsNullOrEmpty(item["Giám sát vào"].ToString()))
                    {
                        var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                        if (_userIn != null)
                        {
                            item["Giám sát vào"] = _userIn.Username;
                        }
                        else
                        {
                            item["Giám sát vào"] = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(item["Giám sát ra"].ToString()))
                    {
                        var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                        if (_userOut != null)
                        {
                            item["Giám sát ra"] = _userOut.Username;
                        }
                        else
                        {
                            item["Giám sát ra"] = "";
                        }

                    }


                }

                listExcel.Rows.Add(0, DictionarySearch["total"], "", "", "", "", "", "", "", "", "", totalmoney);

                //Xuất file theo format
                ReportVehicleMoneyByCardMonthFormatCell(listExcel, "", Dictionary["title_EX"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportVehicleMoneyByCardMonth");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportVehicleMoneyByCardMonth(key, user, cardgroup, fromdate, todate, lane, page, pageSize, ref totalItem, ref totalmoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();

                var newobj = new ReportInOut
                {
                    CardNo = DictionarySearch["total"],
                    DateTimeIn = "",
                    Moneys = totalmoney
                };

                list.Add(newobj);
            }

            var gridModel = PageModelCustom<ReportInOut>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;


            return View(gridModel);
            #endregion
        }
        private void ReportVehicleMoneyByCardMonthFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleMoneyByCardMonth");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang

            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardId"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cost"], ItemValue = "Phí" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Báo cáo tổng hợp theo mã thẻ cho lượt vào ra phát sinh phụ trội
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalSubventionByCardNumber(string key = "", string customergroup = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalSubventionByCardNumber");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";
            string totalmoney = "";

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var listcustomer = _tblCustomerGroupService.GetAllActive();

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportTotalSubventionByCardNumberExcel(key, strCG, fromdate, todate, ref totalmoney);

                foreach (DataRow dr in listExcel.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["CustomerGroupID"].ToString()))
                    {
                        var cusgr = listcustomer.Where(n => n.CustomerGroupID == Guid.Parse(dr["CustomerGroupID"].ToString())).FirstOrDefault();
                        dr["CustomerGroupID"] = cusgr != null ? cusgr.CustomerGroupName : "";
                    }

                }

                listExcel.Rows.Add(0, DictionarySearch["total"], "", "", "", "", totalmoney);

                //Xuất file theo format
                ReportTotalSubventionByCardNumberFormatCell(listExcel, "", Dictionary["title_EX"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportVehicleMoneyByCardMonth");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportTotalSubventionByCardNumber(key, strCG, fromdate, todate, page, pageSize, ref totalItem, ref totalmoney);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.CustomerGroupID))
                    {
                        var cusgr = listcustomer.Where(n => n.CustomerGroupID == Guid.Parse(item.CustomerGroupID)).FirstOrDefault();
                        item.CustomerGroupName = cusgr != null ? cusgr.CustomerGroupName : "";
                    }

                }

                //ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();

                var newobj = new ReportTotalVehicleMoneyByCardMonth
                {
                    CardNo = DictionarySearch["total"],
                    Moneys = totalmoney
                };

                list.Add(newobj);
            }

            var gridModel = PageModelCustom<ReportTotalVehicleMoneyByCardMonth>.GetPage(list, page, pageSize, totalItem);
            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;
            //ViewBag.CardGroups = GetCardGroupListNew().ToList();
            //ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            //ViewBag.CardGroupId = cardgroup;

            //ViewBag.Lanes = GetLaneList().ToList();
            //ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            //ViewBag.LaneId = lane;

            //ViewBag.Users = GetUserList().ToList();
            //ViewBag.UserDT = GetUserList().ToDataTableNullable();
            //ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;


            return View(gridModel);
            #endregion
        }
        private void ReportTotalSubventionByCardNumberFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalSubventionByCardNumber");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang

            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardId"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customerGroup"], ItemValue = "Nhóm khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cost"], ItemValue = "Phí" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Thời hạn thẻ tháng
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportParkingCardExpire(string key = "", string fromdate = "", string todate = "", string cardgroup = "", string customer = "", int page = 1, string chkExport = "0", string IsAlmostExpired = "0")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardExpire");

            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportCardExpiredExcel(key, fromdate, todate, cardgroup, customer, page, pageSize, IsAlmostExpired);

                //Xuất file theo format
                ReportCardExpireFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportParkingCardExpire");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportCardExpired(key, fromdate, todate, cardgroup, customer, page, pageSize, ref totalItem, IsAlmostExpired);

            var gridModel = PageModelCustom<ReportCardExpire>.GetPage(list, page, pageSize, totalItem);

            ViewBag.KeyWord = key;
            ViewBag.IsAlmostExpiredValue = IsAlmostExpired;

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().Where(n => n.CardType == 0).ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            return View(gridModel);
            #endregion
        }
        private void ReportCardExpireFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardExpire");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"] + " 1", ItemValue = "Biển số 1" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"] + " 2", ItemValue = "Biển số 2" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"] + " 3", ItemValue = "Biển số 3" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["expirationDate"], ItemValue = "Ngày hết hạn" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customerCode"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Tên khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["phone"], ItemValue = "Điện thoại" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày sắp hết hạn", ItemValue = "Ngày sắp hết hạn" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }

        public JsonResult GetCustomerByAutoComplete(string name)
        {
            var lstCustomer = _tblCustomerService.GetAllActiveByKey(name);

            return Json(lstCustomer, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Miễn phí hoàn toàn

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleFreeAll(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalmoney = 0;
            var datefrompicker = "";
            var systemconfig = _tblSystemConfigService.GetDefault();
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleFreeAll");

            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleFreeAll_Excel(key, fromdate, todate, cardgroup, lane, user, systemconfig != null ? systemconfig.FeeName : "", page, pageSize);

                //foreach (DataRow item in listExcel.Rows)
                //{
                //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                //var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                //if (_laneIn != null)
                //{
                //    item["Làn vào"] = _laneIn.LaneName;
                //}
                //else
                //{
                //    item["Làn vào"] = "";
                //}

                //var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                //if (_laneOut != null)
                //{
                //    item["Làn ra"] = _laneOut.LaneName;
                //}
                //else
                //{
                //    item["Làn ra"] = "";
                //}

                //if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                //{
                //    var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                //    if (_cardgroup != null)
                //    {
                //        item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                //    }
                //    else
                //    {
                //        item["Nhóm thẻ"] = "";
                //    }
                //}
                //else
                //{
                //    if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                //    {
                //        item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                //    }
                //    else
                //    {
                //        item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                //    }
                //}

                //var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                //if (_userIn != null)
                //{
                //    item["Giám sát vào"] = _userIn.Username;
                //}
                //else
                //{
                //    item["Giám sát vào"] = "";
                //}

                //var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                //if (_userOut != null)
                //{
                //    item["Giám sát ra"] = _userOut.Username;
                //}
                //else
                //{
                //    item["Giám sát ra"] = "";
                //}
                //}

                //Xuất file theo format
                ReportVehicleFreeAllFormatCell(listExcel, Dictionary["titleExAll"], "Sheet1", "", Dictionary["titleAll"], datefrompicker);

                return RedirectToAction("ReportVehicleFreeAll");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.GetReportVehicleFreeAll(key, fromdate, todate, cardgroup, lane, user, page, pageSize, ref totalItem, ref totalmoney).ToList();

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportVehicleFreeAll>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            //     ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.money = totalmoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");


            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            ViewBag.IS110CAUGIAY = systemconfig != null ? (systemconfig.FeeName.Contains("TAK_110CAUGIAY") ? true : false) : false;

            return View(gridModel);
            #endregion
        }


        #region Format cell lên excel
        private void ReportVehicleFreeAllFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleFreeAll");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["voucher"], ItemValue = "Voucher" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Miễn phí hoàn toàn TRANSERCO

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleFreeAllTRANSERCO(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalmoney = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleFreeAllTRANSERCO_Excel(key, fromdate, todate, cardgroup, lane, user, page, pageSize);

                //foreach (DataRow item in listExcel.Rows)
                //{
                //    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                //    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                //    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                //    if (_laneIn != null)
                //    {
                //        item["Làn vào"] = _laneIn.LaneName;
                //    }
                //    else
                //    {
                //        item["Làn vào"] = "";
                //    }

                //    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                //    if (_laneOut != null)
                //    {
                //        item["Làn ra"] = _laneOut.LaneName;
                //    }
                //    else
                //    {
                //        item["Làn ra"] = "";
                //    }

                //    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                //    {
                //        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                //        if (_cardgroup != null)
                //        {
                //            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                //        }
                //        else
                //        {
                //            item["Nhóm thẻ"] = "";
                //        }
                //    }
                //    else
                //    {
                //        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                //        {
                //            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                //        }
                //        else
                //        {
                //            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                //        }
                //    }

                //    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                //    if (_userIn != null)
                //    {
                //        item["Giám sát vào"] = _userIn.Username;
                //    }
                //    else
                //    {
                //        item["Giám sát vào"] = "";
                //    }

                //    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                //    if (_userOut != null)
                //    {
                //        item["Giám sát ra"] = _userOut.Username;
                //    }
                //    else
                //    {
                //        item["Giám sát ra"] = "";
                //    }
                //}
                var titlereport = _tblLaneService.GetTitle(lane);
                //Xuất file theo format
                ReportVehicleFreeAllTRANSERCOFormatCell(listExcel, string.Format("Lượt_ra_vào_miễn_phí{0}", string.IsNullOrEmpty(lane) ? "" : "_" + titlereport.Replace(' ', '_').Replace('-', '_')), "Sheet1", "", string.Format("Lượt ra vào miễn phí {0}", string.IsNullOrEmpty(lane) ? "" : titlereport), datefrompicker);

                return RedirectToAction("ReportVehicleFreeAllTRANSERCO");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.GetReportVehicleFreeAllTRANSERCO(key, fromdate, todate, cardgroup, lane, user, page, pageSize, ref totalItem, ref totalmoney).ToList();

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportVehicleFreeAllTRANSERCO>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            //     ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.money = totalmoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
            ViewBag.selectedEventValue = GetSetFromSessionFreeAll(null);
            return View(gridModel);
            #endregion
        }

        public ActionResult PrintReportVehicleFreeAllTRANSERCO(string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "")
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalmoney = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }


            #region Giao diện


            var list = _ReportService.PrintReportVehicleFreeAllTRANSERCO(key, fromdate, todate, cardgroup, lane, user, ref totalItem, ref totalmoney).ToList();

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            //     ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.money = totalmoney;
            ViewBag.StringMoney = FunctionHelper.DocTienBangChu(totalmoney, " đồng");
            ViewBag.TitleReport = string.IsNullOrEmpty(lane) ? "" : _tblLaneService.GetTitle(lane);
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
            #endregion
        }

        #region Format cell lên excel
        private void ReportVehicleFreeAllTRANSERCOFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian ra", ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = "Làn ra", ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát ra", ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền(VNĐ)", ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Voucher", ItemValue = "Voucher" });
            listColumn.Add(new SelectListModel { ItemText = "Ghi chú", ItemValue = "Ghi chú" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        public JsonResult AddOrRemoveOneAllFreeAllSeleted(List<string> Id, bool isAdd)
        {
            GetSetFromSessionFreeAll(Id, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSessionFreeAll(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listEventId = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.EventIdFreeAllActionParkingSession, host)];
            if (listEventId != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Add(item);
                            }
                        }
                        else
                        {
                            if (listEventId.Any(n => n.Equals(item)))
                            {
                                listEventId.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listEventId = list;
                }
                else
                {
                    listEventId = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.EventIdFreeAllActionParkingSession, host)] = listEventId;

            return listEventId;
        }

        public PartialViewResult ModalSelectedFreeAll(int totalItem = 0, string url = "")
        {
            var listSelected = GetSetFromSessionFreeAll(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listSelected);
        }

        public JsonResult RemoveAllFreeAllSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.EventIdFreeAllActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFree()
        {
            var listSelected = GetSetFromSessionFreeAll(null);

            _ReportService.RemoveFreeMoneyEvent(listSelected);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult NoteFree(string Id, string Note)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            try
            {
                _ReportService.UpdateNoteFree(Id, Note);
                result = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception)
            {
                result = new MessageReport(false, "Có lỗi xảy ra");
                throw;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Miễn phí một phần

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleFreeApart(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalmoney = 0;
            long totalmoneyfree = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleFreeAll");
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleFreeApart_Excel(key, fromdate, todate, cardgroup, lane, user, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }

                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }
                }

                //Xuất file theo format
                ReportVehicleFreeApartFormatCell(listExcel, Dictionary["titleExPart"], "Sheet1", "", Dictionary["titleExPart"], datefrompicker);

                return RedirectToAction("ReportVehicleFreeApart");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.GetReportVehicleFreeApart(key, fromdate, todate, cardgroup, lane, user, page, pageSize, ref totalItem, ref totalmoney, ref totalmoneyfree).ToList();

            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportVehicleFreeAll>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            //     ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.money = totalmoney;
            ViewBag.moneyfree = totalmoneyfree;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportVehicleFreeApartFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleFreeAll");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "amountFree", ItemValue = "Tiền miễn phí" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["voucher"], ItemValue = "Voucher" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Chi tiết xử lý thẻ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCardProcessDetail(string customergroup = "", string key = "", string user = "", string cardgroup = "", string _action = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardProcessDetail");
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.ReportCardProcessDetail_Excel(key, strCG, fromdate, todate, cardgroup, _action, user, page, pageSize, ref totalItem);


                //Xuất file theo format
                ReportCardProcessDetailFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportCardProcessDetail");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.ReportCardProcessDetail(key, strCG, fromdate, todate, cardgroup, _action, user, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<ReportCardProcess>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroupCs = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;
            ViewBag.Action = FunctionHelper.Action();
            ViewBag.ActionValue = _action;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportCardProcessDetailFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardProcessDetail");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["time"], ItemValue = "Thời gian" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Thao tác", ItemValue = "Thao tác" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardHolder"], ItemValue = "Chủ thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cusGrp"], ItemValue = "Nhóm KH" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["staffMade"], ItemValue = "NV thực hiện" });

            listColumn.Add(new SelectListModel { ItemText = "Thông tin cũ", ItemValue = "Thông tin cũ" });


            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Tổng hợp xử lý thẻ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCardProcess(string cardgroup = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardProcess");
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.ReportCardProcess(fromdate, todate, cardgroup);

                //Xuất file theo format
                ReportCardProcessFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportCardProcess");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.ReportCardProcess(fromdate, todate, cardgroup).DataTableToList<ReportCardProcess>();

            var gridModel = PageModelCustom<ReportCardProcess>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportCardProcessFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardProcess");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "CardGroupID" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["addCard"], ItemValue = "ADD" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardIssuance"], ItemValue = "RELEASE" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["changeCard"], ItemValue = "CHANGE" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["returnCard"], ItemValue = "RETURN" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lockCard"], ItemValue = "LOCK" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["openCard"], ItemValue = "UNLOCK" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["delCard"], ItemValue = "DELETE" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["activeCard"], ItemValue = "ACTIVE" });
            listColumn.Add(new SelectListModel { ItemText = "Đổi biển số", ItemValue = "Đổi biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Đổi nhóm thẻ", ItemValue = "Đổi nhóm thẻ" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Sự kiện cảnh báo

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportAlarm(string key = "", string lane = "", string user = "", string alarmcode = "", string datefrompicker = "", int page = 1, string chkExport = "0", string fromdate = "", string todate = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportAlarm");
            var systemconfig = _tblSystemConfigService.GetDefault();
            var card = "";
            if (!string.IsNullOrEmpty(key))
            {
                var ob1 = _tblCardService.GetByCardNumber(key.Trim());
                if (ob1 != null)
                {
                    var card1 = _tblCardService.GetCardByCardNo(key);
                    if (card1 != null)
                    {
                        card = card1.CardNumber;
                    }
                }
               
            }
           
            
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport.Equals("1"))
            {
                var listExcel = new DataTable();

                if (systemconfig.FeeName.Contains("TRANSERCO"))
                {
                    listExcel = _tblAlarmService.ReportGetAllPagingByFirst_TRANSERCO(key, user, lane, alarmcode, datefrompicker);
                }
                else
                {
                    listExcel = _tblAlarmService.ReportGetAllPagingByFirst(key, user, lane, alarmcode, datefrompicker);
                }

                //Xuất file theo format
                ReportAlarmFormatCell(listExcel, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportAlarm");
            }

            var pageSize = 20;
            var total = 0;

            var list = new List<ReportAlarm>();

            if (systemconfig.FeeName.Contains("TRANSERCO"))
            {
                list = _tblAlarmService.GetAllPagingByFirst_TRANSERCO(key, user, lane, alarmcode, fromdate, todate, page, pageSize, ref total);
            }
            else
            {
                list = _tblAlarmService.GetAllPagingByFirst(card,key, user, lane, alarmcode, fromdate, todate, page, pageSize, ref total);
            }


            var gridModel = PageModelCustom<ReportAlarm>.GetPage(list, page, pageSize, total);

            ViewBag.keyValue = key;
            ViewBag.laneValue = lane;
            ViewBag.userValue = user;
            ViewBag.alarmcodeValue = alarmcode;
            ViewBag.datefrompickerValue = datefrompicker;

            ViewBag.Lanes = GetLaneList();
            ViewBag.Users = GetUserList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.Alarms = FunctionHelper.AlarmCodes();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        #region Format cell lên excel
        private void ReportAlarmFormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportAlarm");
            var Dictionary2 = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["time"], ItemValue = "Date" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary2["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardCode"], ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Plate" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["warning"], ItemValue = "AlarmName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lane"], ItemValue = "LaneName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["explanation"], ItemValue = "Description" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoring"], ItemValue = "Username" });
            listColumn.Add(new SelectListModel { ItemText = "Mô tả", ItemValue = "Mô tả" });

            var Lanes = GetLaneList();
            var Users = GetUserList();
            var Alarms = FunctionHelper.AlarmCodes();

            if (listData != null && listData.Rows.Count > 0)
            {
                foreach (DataRow item in listData.Rows)
                {
                    var aname = Alarms.FirstOrDefault(n => n.ItemValue.Equals(item["AlarmName"].ToString()));
                    var lname = Lanes.FirstOrDefault(n => n.LaneID.ToString() == item["LaneName"].ToString());
                    var uname = Users.FirstOrDefault(n => n.Id == item["Username"].ToString());

                    item["AlarmName"] = aname != null ? aname.ItemText : "";
                    item["LaneName"] = lname != null ? lname.LaneName : "";
                    item["Username"] = uname != null ? uname.Username : "";
                }
            }

            //Chuyển dữ liệu về datatable
            DataTable dt = listData;

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo thẻ theo căn hộ

        #region Chi tiết
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailCardCompartment(string compartmentid,string key, string cardgroup, string customerid, string customergroup, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1, string chkExport = "0", string selectedId = "", bool desc = false, string columnQuery = "CompartmentId", bool chkFindAutoCapture = false)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardCompartment");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetDetailCardCompartment_Excel(compartmentid,key, cardgroup, customerid, strCG, fromdate, todate, desc, columnQuery, isCheckByTime, "", active, chkFindAutoCapture);

                //Xuất file theo format
                ReportDetailCardCompartmentFormatCell(listExcel, Dictionary["titleDetail_EX"], "Sheet1", "", Dictionary["titleDetail"], fromdate + " - " + todate);

                return RedirectToAction("ReportDetailCardCompartment");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetReportDetailCardCompartment(compartmentid,key, cardgroup, customerid, strCG, fromdate, todate, desc, columnQuery, ref totalItem, isCheckByTime, page, pageSize, "", active, chkFindAutoCapture).ToList();

            var gridModel = PageModelCustom<tblCardCustomViewModel>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;


            ViewBag.Compartments = GetCompartmentListNew().ToList();
            ViewBag.CompartmentsDT = GetCompartmentListNew().ToDataTableNullable();
            ViewBag.CompartmentsId = compartmentid;


            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;
            ViewBag.LstCompartment = _tblCompartmentService.GetAll();
            ViewBag.KeyWord = key;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        private void ReportDetailCardCompartmentFormatCell(List<DetailCardDepartmentExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Create");

            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var DictionaryCom = FunctionHelper.GetLocalizeDictionary("report", "ReportCardCompartment");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionaryAction["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);
            var systemconfig = _tblSystemConfigService.GetDefault();
            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["Compartment"], ItemValue = "CompartmentId" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerCode"], ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerName"], ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerGroup"], ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNumber"], ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardGroup"], ItemValue = "CardGroupNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Plate"], ItemValue = "Plates" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["DateRegisted"], ItemValue = "DateRegister" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["DateReleased"], ItemValue = "DateRelease" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["DateExpired"], ItemValue = "DateExpire" });

            listColumn.Add(new SelectListModel { ItemText = Dictionary["DateCreated"], ItemValue = "DateCreated" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            //if (systemconfig != null && !systemconfig.FeeName.Contains("TRANSERCO"))
            //{
            //    dt.Columns.Remove("Description");
            //}
            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }

        #endregion

        #region Tổng hợp
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalCardCompartment(string compartmentid,string key, string cardgroup, string customerid, string customergroup, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1, string chkExport = "0", string selectedId = "", bool desc = false, string columnQuery = "CompartmentId", bool chkFindAutoCapture = false)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportCardCompartment");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetTotalCardCompartment_Excel(compartmentid,key, cardgroup, customerid, strCG, fromdate, todate, ref totalItem, isCheckByTime, page, pageSize, active);

                //Xuất file theo format
                ReportTotalCardCompartmentFormatCell(listExcel, Dictionary["titleTotal_EX"], "Sheet1", "", Dictionary["titleTotal"], fromdate + " - " + todate);

                return RedirectToAction("ReportTotalCardCompartment");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetReportTotalCardCompartment(compartmentid,key, cardgroup, customerid, strCG, fromdate, todate, ref totalItem, isCheckByTime, page, pageSize, active).ToList();
            var be = list.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            totalItem = list != null ? list.Count() : 0;
         
            var gridModel = PageModelCustom<TotalCardDepartment>.GetPage(be, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Compartments = GetCompartmentListNew().ToList();
            ViewBag.CompartmentsDT = GetCompartmentListNew().ToDataTableNullable();
            ViewBag.CompartmentsId = compartmentid;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;
            ViewBag.LstCompartment = _tblCompartmentService.GetAll();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        private void ReportTotalCardCompartmentFormatCell(List<TotalCardDepartment1> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Create");

            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            var DictionaryCom = FunctionHelper.GetLocalizeDictionary("report", "ReportCardCompartment");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionaryAction["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);
            var systemconfig = _tblSystemConfigService.GetDefault();
            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["Compartment"], ItemValue = "CompartmentId" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CAR_REG"], ItemValue = "CAR_REG" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CAR_NOTUSE"], ItemValue = "CAR_NOTUSE" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CAR_USE"], ItemValue = "CAR_USE" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CYCLE_REG"], ItemValue = "CYCLE_REG" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CYCLE_NOTUSE"], ItemValue = "CYCLE_NOTUSE" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["CYCLE_USE"], ItemValue = "CYCLE_USE" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["BIKE_REG"], ItemValue = "BIKE_REG" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["BIKE_NOTUSE"], ItemValue = "BIKE_NOTUSE" });
            listColumn.Add(new SelectListModel { ItemText = DictionaryCom["BIKE_USE"], ItemValue = "BIKE_USE" });


            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #endregion

        #region Lào Cai

        #region Chi tiết thu tiền thẻ lượt
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult LAOCAI_ReportDetailMoneyCardDay(string customergroup = "", string state = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");

            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportDetailMoneyCardDayExcel_LAOCAI(key, state, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    //if(item["Thời gian ra"] != null && !string.IsNullOrEmpty(item["Thời gian ra"].ToString()))
                    //{
                    //    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());
                    //}


                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    //var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    //if (_laneOut != null)
                    //{
                    //    item["Làn ra"] = _laneOut.LaneName;
                    //}
                    //else
                    //{
                    //    item["Làn ra"] = "";
                    //}

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    //var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    //if (_userOut != null)
                    //{
                    //    item["Giám sát ra"] = _userOut.Username;
                    //}
                    //else
                    //{
                    //    item["Giám sát ra"] = "";
                    //}

                    long _totalMoney = 0;

                    if (item["Trạng thái tiền vào"] != null && item["Trạng thái tiền vào"].ToString() == "Ðã thanh toán")
                    {
                        long.TryParse(item["Tiền vào"].ToString(), out _totalMoney);
                        totalMoney += _totalMoney;
                    }



                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát vào"] = Dictionary["total"];
                totalMoneyRow["Tiền vào"] = totalMoney.ToString();
                listExcel.Rows.Add(totalMoneyRow);


                //Xuất file theo format
                ReportDetailMoneyCardDayFormatCell_LAOCAI(listExcel, "", "Chi_tiết_thu_tiền_thẻ_lượt_khi_vào", "Sheet1", "", "Chi tiết thu tiền thẻ lượt khi vào", datefrompicker);

                return RedirectToAction("LAOCAI_ReportDetailMoneyCardDay");
            }
            #endregion


            #region Giao diện
            var list = _ReportService.GetReportDetailMoneyCardDay_LAOCAI(key, state, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay_LAOCAI>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;
            ViewBag.PayState = FunctionHelper.PayState();
            ViewBag.PayStateValue = state;
            ViewBag.TotalMoney = totalMoney;

            var systemconfig = _tblSystemConfigService.GetDefault();

            return View(gridModel);
            #endregion
        }

        public ActionResult LAOCAI_PrintReportDetailMoneyCardDay(string key = "", string state = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            long totalMoney = 0;

            var list = _ReportService.Print_GetReportDetailMoneyCardDay_LAOCAI(key, state, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);

            var gridModel = PageModelCustom<ReportDetailMoneyCardDay_LAOCAI>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;

            ViewBag.FilterLink = $"/Parking/Report/LAOCAI_PrintReportDetailMoneyCardDay?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";
            ViewBag.System = _tblSystemConfigService.GetDefault();
            ViewBag.PageSize = pageSize;

            return View(gridModel);
        }

        private void ReportDetailMoneyCardDayFormatCell_LAOCAI(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " :" + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["moneyout"], ItemValue = "Tiền ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["moneyin"], ItemValue = "Tiền vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["paystate"], ItemValue = "Trạng thái tiền vào" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["totalinout"], ItemValue = "Tổng đã thanh toán" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["timeAll"], ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file

            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username, 200);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }

        public JsonResult UpdatePayState(string payid)
        {
            var mess = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _tblCardEventService.UpdatePayState(payid);

                mess = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception)
            {
                mess = new MessageReport(false, "Có lỗi xảy ra");
                throw;
            }

            return Json(mess, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region #9

        #region Chi tiết thông tin thẻ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCustomerList(string customergroup = "", string key = "", string chkExport = "0", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            var datefrompicker = "";

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetCustomerList_Excel(key, strCG, page, pageSize, ref totalItem);

                //Xuất file theo format
                ReportCustomerListFormatCell(listExcel, "Chi_tiết_thông_tin_thẻ", "Sheet1", "", "Chi tiết thông tin thẻ", datefrompicker);

                return RedirectToAction("ReportCustomerList");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.GetCustomerList(key, strCG, page, pageSize, ref totalItem).ToList();


            var gridModel = PageModelCustom<ReportCustomerList>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportCustomerListFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";//"Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Mã KH", ItemValue = "Mã KH" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm KH", ItemValue = "Nhóm KH" });
            listColumn.Add(new SelectListModel { ItemText = "Tên KH", ItemValue = "Tên KH" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = "Điện thoại", ItemValue = "Điện thoại" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày hết hạn", ItemValue = "Ngày hết hạn" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày nhập thẻ", ItemValue = "Ngày nhập thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #endregion

        #region Báo cáo chi tiết thu tiền của xe đỗ vị trí Taxi Hà Nội

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleEvent(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleEvent_Excel(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney, chkCheckByTime);



                //Xuất file theo format
                ReportVehicleEventFormatCell(listExcel, "Báo_cáo_chi_tiết_thu_tiền_của_xe_đỗ_vị_trí_Taxi_Hà_Nội", "Sheet1", "", "Báo cáo chi tiết thu tiền của xe đỗ vị trí Taxi Hà Nội", datefrompicker);

                return RedirectToAction("ReportIn");
            }
            #endregion

            #region Giao diện


            var list = _ReportService.GetReportVehicleEvent(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney, chkCheckByTime).ToList();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportIn>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportVehicleEventFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Plate" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "DatetimeIn" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupID" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "LaneIDIn" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "UserIDIn" });
            listColumn.Add(new SelectListModel { ItemText = "Vị trí đỗ", ItemValue = "ViTriDo" });
            listColumn.Add(new SelectListModel { ItemText = "Phí(VNĐ)", ItemValue = "Moneys" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo tổng hợp doanh thu vé lượt theo vị trí đỗ xe
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyByVehicleEvent(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {

            var datefrompicker = "";

            long TX_totalmoneys = 0;
            long TX_totalIn = 0;
            long TX_totalOut = 0;

            long NotTX_totalmoneys = 0;
            long NotTX_totalIn = 0;
            long NotTX_totalOut = 0;

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //lấy danh sách nhóm thẻ
            var lstCardGroupId_TX = _ReportService.ReportTotalMoneyByVehicleEvent(key, user, fromdate, todate, cardgroup, lane, true).ToList();
            var lstCardGroupId_NotTX = _ReportService.ReportTotalMoneyByVehicleEvent(key, user, fromdate, todate, cardgroup, lane, false).ToList();

            //tỉnh số lượng và tiền
            var dtCardGroup_TX = _ReportService.GetTotalMoneysAndVehicleByCardGroup(fromdate, todate, lstCardGroupId_TX, "TX", ref TX_totalIn, ref NotTX_totalIn, ref TX_totalOut, ref NotTX_totalOut, ref TX_totalmoneys, ref NotTX_totalOut);

            var dtCardGroup_NotTX = _ReportService.GetTotalMoneysAndVehicleByCardGroup(fromdate, todate, lstCardGroupId_NotTX, "XN", ref TX_totalIn, ref NotTX_totalIn, ref TX_totalOut, ref NotTX_totalOut, ref TX_totalmoneys, ref NotTX_totalmoneys);


            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                count_TX = dtCardGroup_TX.Rows.Count;
                count_NotTX = dtCardGroup_NotTX.Rows.Count;

                var dt = new DataTable();
                dt.Columns.Add("STT", typeof(string));
                dt.Columns.Add("Vị trí đỗ", typeof(string));
                dt.Columns.Add("Lượt vào", typeof(string));
                dt.Columns.Add("Lượt ra", typeof(string));
                dt.Columns.Add("Số tiền(VNĐ)", typeof(string));

                dt.Rows.Add("1", "Bãi xe Taxi Hà Nội", TX_totalIn, TX_totalOut, string.Format(new System.Globalization.CultureInfo("en-US"), "{0:0,0}", TX_totalmoneys));

                //add nhóm thẻ bãi taxi
                if (dtCardGroup_TX != null && dtCardGroup_TX.Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow dtTX in dtCardGroup_TX.Rows)
                    {
                        count++;
                        dt.Rows.Add("", "1." + count + " " + dtTX["CardGroupName"].ToString(), dtTX["TotalVehicleIn"].ToString(), dtTX["TotalVehicleOut"].ToString(), dtTX["Moneys"].ToString());
                    }

                }

                dt.Rows.Add("2", "Bãi xe xí nghiệp", NotTX_totalIn, NotTX_totalOut, string.Format(new System.Globalization.CultureInfo("en-US"), "{0:0,0}", NotTX_totalmoneys));

                //add nhóm thẻ bãi xí nghiệp
                if (dtCardGroup_NotTX != null && dtCardGroup_NotTX.Rows.Count > 0)
                {
                    int _count = 0;
                    foreach (DataRow dtNotTX in dtCardGroup_NotTX.Rows)
                    {
                        _count++;
                        dt.Rows.Add("", "2." + _count + " " + dtNotTX["CardGroupName"].ToString(), dtNotTX["TotalVehicleIn"].ToString(), dtNotTX["TotalVehicleOut"].ToString(), dtNotTX["Moneys"].ToString());
                    }

                }
                //tổng
                dt.Rows.Add("#", "Tổng số", NotTX_totalIn + TX_totalIn, NotTX_totalOut + TX_totalOut, string.Format(new System.Globalization.CultureInfo("en-US"), "{0:0,0}", NotTX_totalmoneys + TX_totalmoneys));

                if (dt.Rows.Count > 0)
                {

                    string _title1 = "BÁO CÁO TỔNG HỢP DOANH THU VÉ LƯỢT THEO VỊ TRÍ ĐỖ XE";
                    string _title2 = string.Format("TỪ NGÀY {0} ĐẾN NGÀY {1}", fromdate, todate);

                    BindDataToExcelReportVehicleInOut(dt, _title1, _title2);

                }

                return RedirectToAction("ReportTotalMoneyByVehicleEvent");
            }
            #endregion

            #region Giao diện

            //Bãi xe taxi
            ViewBag.totalIn_TX = TX_totalIn;
            ViewBag.totalOut_TX = TX_totalOut;
            ViewBag.totalMoneys_TX = TX_totalmoneys.ToString().FormatMoney();

            //bãi xe xí nghiệp
            ViewBag.totalIn_NotTX = NotTX_totalIn;
            ViewBag.totalOut_NotTX = NotTX_totalOut;
            ViewBag.totalMoneys_NotTX = NotTX_totalmoneys.ToString().FormatMoney();

            //tổng số
            ViewBag.totalIn = NotTX_totalIn + TX_totalIn;
            ViewBag.totalOut = NotTX_totalOut + TX_totalOut;
            ViewBag.totalMoneys = (NotTX_totalmoneys + TX_totalmoneys).ToString().FormatMoney();

            //bảng nhóm thẻ
            ViewBag.dtCardGroup_TX = dtCardGroup_TX;
            ViewBag.dtCardGroup_NotTX = dtCardGroup_NotTX;

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View();
            #endregion
        }

        public ActionResult PrintReportTotalMoneyByVehicleEvent(string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {
            // var totalPage = 0;

            var datefrompicker = "";

            long TX_totalmoneys = 0;
            long TX_totalIn = 0;
            long TX_totalOut = 0;

            long NotTX_totalmoneys = 0;
            long NotTX_totalIn = 0;
            long NotTX_totalOut = 0;

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //lấy danh sách nhóm thẻ
            var lstCardGroupId_TX = _ReportService.ReportTotalMoneyByVehicleEvent(key, user, fromdate, todate, cardgroup, lane, true).ToList();
            var lstCardGroupId_NotTX = _ReportService.ReportTotalMoneyByVehicleEvent(key, user, fromdate, todate, cardgroup, lane, false).ToList();

            //tỉnh số lượng và tiền
            var dtCardGroup_TX = _ReportService.GetTotalMoneysAndVehicleByCardGroup(fromdate, todate, lstCardGroupId_TX, "TX", ref TX_totalIn, ref NotTX_totalIn, ref TX_totalOut, ref NotTX_totalOut, ref TX_totalmoneys, ref NotTX_totalOut);

            var dtCardGroup_NotTX = _ReportService.GetTotalMoneysAndVehicleByCardGroup(fromdate, todate, lstCardGroupId_NotTX, "XN", ref TX_totalIn, ref NotTX_totalIn, ref TX_totalOut, ref NotTX_totalOut, ref TX_totalmoneys, ref NotTX_totalmoneys);

            #region Giao diện

            //Bãi xe taxi
            ViewBag.totalIn_TX = TX_totalIn;
            ViewBag.totalOut_TX = TX_totalOut;
            ViewBag.totalMoneys_TX = TX_totalmoneys.ToString().FormatMoney();

            //bãi xe xí nghiệp
            ViewBag.totalIn_NotTX = NotTX_totalIn;
            ViewBag.totalOut_NotTX = NotTX_totalOut;
            ViewBag.totalMoneys_NotTX = NotTX_totalmoneys.ToString().FormatMoney();

            //tổng số
            ViewBag.totalIn = NotTX_totalIn + TX_totalIn;
            ViewBag.totalOut = NotTX_totalOut + TX_totalOut;
            ViewBag.totalMoneys = (NotTX_totalmoneys + TX_totalmoneys).ToString().FormatMoney();

            //bảng nhóm thẻ
            ViewBag.dtCardGroup_TX = dtCardGroup_TX;
            ViewBag.dtCardGroup_NotTX = dtCardGroup_NotTX;

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View();
            #endregion
        }

        #region Format cell lên excel

        public void BindDataToExcelReportVehicleInOut(DataTable _dt, string _title1, string _title2)
        {
            var dtHeader = getHeaderExcelReportVehicleInOut(_title1, _title2);
            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelFileReportVehicleInOut(new MemoryStream(), _dt, dtHeader);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={1}-{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmm"), _title1.Replace(" ", "-")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }

        public static DataTable getHeaderExcelReportVehicleInOut(string title1, string title2)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("header", typeof(string));

                dt.Rows.Add("XÍ NGHIỆP DỊCH VỤ VÀ CHO THUÊ VĂN PHÒNG ");
                dt.Rows.Add("ĐỊA CHỈ: 105 LÁNG HẠ, ĐỐNG ĐA, HÀ NỘI");
                dt.Rows.Add("MÃ SỐ THUẾ:");
                dt.Rows.Add("");
                dt.Rows.Add(title1);
                dt.Rows.Add(title2);

                return dt;
            }
            catch
            { }

            return null;
        }

        public static Stream CreateExcelFileReportVehicleInOut(Stream stream = null, DataTable dt = null, DataTable dtHeader = null, DataTable dtFooter = null)
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Futech";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Báo cáo";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "Báo cáo";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("Sheet1");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];
                //workSheet.Protection.IsProtected = false;
                // Đổ data vào Excel file
                //workSheet.Cells[1, 1].LoadFromDataTable(dt, true, TableStyles.Dark9);
                BindingFormatForExcelReportVehicleInOut(workSheet, dt, dtHeader);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        public static void BindingFormatForExcelReportVehicleInOut(ExcelWorksheet worksheet, DataTable dt, DataTable dtHeader)
        {
            worksheet.Cells[1, 1, 1, 5].Merge = true;
            worksheet.Cells[2, 1, 2, 5].Merge = true;
            worksheet.Cells[3, 1, 3, 5].Merge = true;
            worksheet.Cells[4, 1, 4, 5].Merge = true;
            worksheet.Cells[5, 1, 5, 5].Merge = true;
            worksheet.Cells[6, 1, 6, 5].Merge = true;

            if (count_TX > 0)
            {
                string merge_TX = "A9:" + "A" + (9 + count_TX);
                worksheet.Cells[merge_TX].Merge = true;
                worksheet.Cells[merge_TX].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            if (count_NotTX > 0)
            {
                string merge_NotTX = "";

                if (count_TX > 0)
                {
                    merge_NotTX = "A" + (10 + count_TX) + ":" + "A" + (10 + count_TX + count_NotTX);
                }
                else
                {
                    merge_NotTX = "A10:" + "A" + (10 + count_NotTX);
                }

                worksheet.Cells[merge_NotTX].Merge = true;
                worksheet.Cells[merge_NotTX].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }


            /*worksheet.Cells["A8:D8"].Merge = true;
            worksheet.Cells["E8:H8"].Merge = true;
            worksheet.Cells["A9:D9"].Merge = true;
            worksheet.Cells["E9:H9"].Merge = true;
            worksheet.Cells["A8:D8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["E8:H8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A9:D9"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["E9:H9"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A10:D10"].Merge = true;
            worksheet.Cells["E10:H10"].Merge = true;
            worksheet.Cells["A11:D11"].Merge = true;
            worksheet.Cells["E11:H11"].Merge = true;
            worksheet.Cells["A10:D10"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["E10:H10"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A11:D11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["E11:H11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);*/

            worksheet.Cells.AutoFitColumns();
            // Tạo header
            if (dtHeader != null && dtHeader.Rows.Count > 0)
            {
                for (int i = 1; i < dtHeader.Rows.Count + 1; i++)
                {
                    DataRow dr = dtHeader.Rows[i - 1];
                    worksheet.Cells[i, 1].Value = dr["header"].ToString();
                    if (i == 1 || i == 2)
                    {
                        worksheet.Cells[i, 1].Style.Font.Bold = true;
                    }

                    if (i == 5)
                    {
                        worksheet.Cells[i, 1].Style.Font.Size = 16;
                        worksheet.Cells[i, 1].Style.Font.Bold = true;
                        worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    if (i == 6)
                    {
                        worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
            }

            //
            if (dt != null)
            {
                // tạo header cho danh sách
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    DataColumn col = dt.Columns[i];
                    switch (i)
                    {
                        case 0:
                            worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Value = "STT";
                            break;
                        case 1:
                            worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Value = "Vị trí đỗ";
                            break;
                        case 2:
                            worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Value = "Lượt vào";
                            break;
                        case 3:
                            worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Value = "Lượt ra";
                            break;
                        case 4:
                            worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Value = "Số tiền(VNĐ)";
                            break;

                    }

                    worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[dtHeader.Rows.Count + 2, i + 1].AutoFitColumns();
                    // worksheet.Cells[dtHeader.Rows.Count + 2, i + 4].Style.Font.Bold = true;
                    //  worksheet.Cells[dtHeader.Rows.Count + 2, i + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //   worksheet.Cells[dtHeader.Rows.Count + 2, i + 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //   worksheet.Cells[dtHeader.Rows.Count + 2, i + 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    //   worksheet.Cells[dtHeader.Rows.Count + 2, i + 4].AutoFitColumns();
                }

                if (dt.Rows.Count > 0)
                {
                    var rowStart = dtHeader.Rows.Count + 3;
                    // Đỗ dữ liệu từ list vào 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow item = dt.Rows[i];
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            var _fromRow = rowStart + (i);
                            var _fromCol = j;

                            worksheet.Cells[_fromRow, _fromCol + 1].Style.Numberformat.Format = "@";
                            worksheet.Cells[_fromRow, _fromCol + 1].Value = item[j].ToString();

                            worksheet.Cells[_fromRow, _fromCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            //worksheet.Cells[_fromRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            //worksheet.Cells[_fromRow, 2].Style.Indent = 3;
                            if (_fromCol == 1)
                            {
                                if (worksheet.Cells[_fromRow, _fromCol + 1].Text.Equals("Bãi xe xí nghiệp") || worksheet.Cells[_fromRow, _fromCol + 1].Text.Equals("Bãi xe Taxi Hà Nội") || worksheet.Cells[_fromRow, _fromCol + 1].Text.Equals("Tổng số"))
                                {
                                    worksheet.Cells[_fromRow, _fromCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                }
                                else
                                {
                                    worksheet.Cells[_fromRow, _fromCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    worksheet.Cells[_fromRow, _fromCol + 1].Style.Indent = 3;
                                }

                            }

                            worksheet.Cells[_fromRow, _fromCol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[_fromRow, _fromCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            worksheet.Cells[rowStart + dt.Rows.Count - 1, _fromCol + 1].Style.Font.Bold = true;

                        }
                    }

                    worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Value = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                    worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Value = "Người lập báo cáo";
                    worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Value = GetCurrentUser.GetUser().Name;
                    worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            worksheet.Cells.AutoFitColumns();
        }
        #endregion
        #endregion

        #region Bảng tổng hợp doanh thu trông giữ xe
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalMoneyAndVehicleByCardGroup(string cardgroup = "", string key = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            long _totalmoneys = 0;
            long _totalIn = 0;
            long _totalOut = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportTotalMoneyAndVehicleByCardGroup(cardgroup, fromdate, todate, ref _totalIn, ref _totalOut, ref _totalmoneys);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                dt.Columns.Add("STT", typeof(string)).SetOrdinal(0);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        count++;

                        dr["STT"] = count;
                    }

                    dt.Rows.Add("#", "Tổng số", _totalIn, _totalOut, _totalmoneys.ToString().FormatMoney());
                }
                //Xuất file theo format
                ReportTotalMoneyAndVehicleByCardGroupFormatCell(dt, "Bảng_tổng_hợp_doanh_thu_trông_giữ_xe", "Sheet1", "", "Bảng tổng hợp doanh thu trông giữ xe", datefrompicker);

                return RedirectToAction("ReportTotalMoneyAndVehicleByCardGroup");
            }
            #endregion

            #region Giao diện

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.dt = dt;

            ViewBag.TotalIn = _totalIn;
            ViewBag.TotalOut = _totalOut;
            ViewBag.TotalMoneys = _totalmoneys.ToString().FormatMoney();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.KeyWord = key;

            return View();
            #endregion
        }

        public ActionResult PrintReportTotalMoneyAndVehicleByCardGroup(string cardgroup = "", string key = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            long _totalmoneys = 0;
            long _totalIn = 0;
            long _totalOut = 0;


            ViewBag.dt = _ReportService.GetReportTotalMoneyAndVehicleByCardGroup(cardgroup, fromdate, todate, ref _totalIn, ref _totalOut, ref _totalmoneys);
            #region Giao diện

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.TotalIn = _totalIn;
            ViewBag.TotalOut = _totalOut;
            ViewBag.TotalMoneys = _totalmoneys.ToString().FormatMoney();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.KeyWord = key;

            return View();
            #endregion
        }

        #region Format cell lên excel
        private void ReportTotalMoneyAndVehicleByCardGroupFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng số xe vào", ItemValue = "TotalVehicleIn" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng số xe ra", ItemValue = "TotalVehicleOut" });
            listColumn.Add(new SelectListModel { ItemText = "Doanh thu(VNĐ)", ItemValue = "Moneys" });



            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #endregion

        #region Tổng hợp theo nhóm thẻ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalByCardGroup(string cardgroup = "", string key = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportTotalByCardGroup(cardgroup, fromdate, todate);

            dt.Columns.Add("STT", typeof(string)).SetOrdinal(0);

            if (dt != null && dt.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count++;

                    dr["STT"] = count;
                }

            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Xuất file theo format
                ReportTotalByCardGroupFormatCell(dt, "Tổng_hợp_theo_nhóm_thẻ", "Sheet1", "", "Tổng hợp theo nhóm thẻ", datefrompicker);

                return RedirectToAction("ReportTotalByCardGroup");
            }
            #endregion

            #region Giao diện

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            // ViewBag.dt = dt;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");


            return View(dt);
            #endregion
        }

        #region Format cell lên excel
        private void ReportTotalByCardGroupFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng hợp", ItemValue = "Moneys" });



            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo chi tiết thẻ theo căn hộ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCardDetailByCompartment(string key = "", string user = "", string compartment = "", string chkExport = "0", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportCardDetailByCompartment(key, user, fromdate, todate, compartment, chkCheckByTime, page, pageSize, ref totalItem);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {

                //Xuất file theo format
                ReportCardDetailByCompartmentFormatCell(dt, "Báo_cáo_chi_tiết_thẻ_theo_căn_hộ", "Sheet1", "", "Báo cáo chi tiết thẻ theo căn hộ", datefrompicker);

                return RedirectToAction("ReportCardDetailByCompartment");
            }
            #endregion

            #region Giao diện

            var list = ExcuteSQL.ConvertTo<ReportCardDetailByCompartment>(dt).ToList();

            IPagedList<ReportCardDetailByCompartment> pagedList = list.ToPagedList(page, pageSize);

            var gridModel = PageModelCustom<ReportCardDetailByCompartment>.GetPage(pagedList, page, pageSize);

            ViewBag.Compartments = GetCompartmentList().ToList();
            ViewBag.CompartmentDT = GetCompartmentList().ToDataTableNullable();
            ViewBag.CompartmentId = compartment;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportCardDetailByCompartmentFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            //listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "LogID" });
            listColumn.Add(new SelectListModel { ItemText = "Căn hộ", ItemValue = "CompartmentName" });
            listColumn.Add(new SelectListModel { ItemText = "Mã KH", ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = "Tên KH", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm KH", ItemValue = "CustomerGroup" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroup" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Plate" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày đăng ký", ItemValue = "DateRegister" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày phát thẻ", ItemValue = "DateRelease" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày hủy thẻ", ItemValue = "DateCanceled" });
            listColumn.Add(new SelectListModel { ItemText = "Người thực hiện", ItemValue = "UserID" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo tổng hợp thẻ theo căn hộ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCardTotalByCompartment(string key = "", string user = "", string compartment = "", string chkExport = "0", bool chkCheckByTime = true, string fromdate = "", string todate = "", int page = 1)
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportCardTotalByCompartment(key, user, fromdate, todate, compartment, chkCheckByTime, page, pageSize, ref totalItem);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {

                //Xuất file theo format
                ReportCardTotalByCompartmentFormatCell(dt, "Báo_cáo_tổng_hợp_thẻ_theo_căn_hộ", "Sheet1", "", "Báo cáo tổng hợp thẻ theo căn hộ", datefrompicker);

                return RedirectToAction("ReportCardTotalByCompartment");
            }
            #endregion

            #region Giao diện

            var list = ExcuteSQL.ConvertTo<ReportCardTotalByCompartment>(dt).ToList();

            IPagedList<ReportCardTotalByCompartment> pagedList = list.ToPagedList(page, pageSize);

            var gridModel = PageModelCustom<ReportCardTotalByCompartment>.GetPage(pagedList, page, pageSize);

            ViewBag.Compartments = GetCompartmentList().ToList();
            ViewBag.CompartmentDT = GetCompartmentList().ToDataTableNullable();
            ViewBag.CompartmentId = compartment;

            //ViewBag.Users = GetUserList().ToList();
            //ViewBag.UserDT = GetUserList().ToDataTableNullable();
            //ViewBag.UserId = user;

            ViewBag.KeyWord = key;

            ViewBag.CheckByTime = chkCheckByTime;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportCardTotalByCompartmentFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            //listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Căn hộ", ItemValue = "CompartmentName" });
            listColumn.Add(new SelectListModel { ItemText = "Ôtô đăng ký", ItemValue = "CountRegistedCar" });
            listColumn.Add(new SelectListModel { ItemText = "Ôtô hủy", ItemValue = "CountLockCar" });
            listColumn.Add(new SelectListModel { ItemText = "Ôtô sử dụng", ItemValue = "CountUseCar" });
            listColumn.Add(new SelectListModel { ItemText = "Xe máy đăng ký", ItemValue = "CountRegistedMotorcycle" });
            listColumn.Add(new SelectListModel { ItemText = "Xe máy hủy", ItemValue = "CountLockMotorcycle" });
            listColumn.Add(new SelectListModel { ItemText = "Xe máy sử dụng", ItemValue = "CountUseMotorcycle" });
            listColumn.Add(new SelectListModel { ItemText = "Xe đạp đăng ký", ItemValue = "CountRegistedBicycle" });
            listColumn.Add(new SelectListModel { ItemText = "Xe đạp hủy", ItemValue = "CountLockBicycle" });
            listColumn.Add(new SelectListModel { ItemText = "Xe đạp sử dụng", ItemValue = "CountUseBicycle" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo in HĐ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportPrint(string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportPrint(key, fromdate, todate, page, pageSize, ref totalItem);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {

                //Xuất file theo format
                ReportPrintFormatCell(dt, "Báo_cáo_in_HĐ", "Sheet1", "", "Báo cáo in HĐ", datefrompicker);

                return RedirectToAction("ReportPrint");
            }
            #endregion

            #region Giao diện

            var list = ExcuteSQL.ConvertTo<ReportPrint>(dt).ToList();

            IPagedList<ReportPrint> pagedList = list.ToPagedList(page, pageSize);

            var gridModel = PageModelCustom<ReportPrint>.GetPage(pagedList, page, pageSize);

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportPrintFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();

            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Date" });
            listColumn.Add(new SelectListModel { ItemText = "Người dùng", ItemValue = "UserName" });
            listColumn.Add(new SelectListModel { ItemText = "Hóa đơn", ItemValue = "ObjectName" });
            listColumn.Add(new SelectListModel { ItemText = "Mô tả", ItemValue = "Description" });


            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo cho xe ra thủ công
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleOutByHand(string key = "", string user = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetReportVehicleOutByHand(key, user, fromdate, todate, page, pageSize, ref totalItem);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {

                //Xuất file theo format
                ReportVehicleOutByHandFormatCell(dt, "Báo_cáo_cho_xe_ra_thủ_công", "Sheet1", "", "Báo cáo cho xe ra thủ công", datefrompicker);

                return RedirectToAction("ReportVehicleOutByHand");
            }
            #endregion

            #region Giao diện

            var list = ExcuteSQL.ConvertTo<ReportPrint>(dt).ToList();

            IPagedList<ReportPrint> pagedList = list.ToPagedList(page, pageSize);

            var gridModel = PageModelCustom<ReportPrint>.GetPage(pagedList, page, pageSize);

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void ReportVehicleOutByHandFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();

            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Date" });
            listColumn.Add(new SelectListModel { ItemText = "Người dùng", ItemValue = "UserName" });
            listColumn.Add(new SelectListModel { ItemText = "Ứng dụng", ItemValue = "SubSystemCode" });
            listColumn.Add(new SelectListModel { ItemText = "Hóa đơn", ItemValue = "ObjectName" });
            listColumn.Add(new SelectListModel { ItemText = "Thao tác", ItemValue = "Actions" });
            listColumn.Add(new SelectListModel { ItemText = "Mô tả", ItemValue = "Description" });


            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Chi tiết thu tiền thẻ lượt

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportDetailMoneyCardDay3(string key = "", string user = "", string lane = "", string datefrompicker = "", string cardgroup = "", int page = 1, string column = "DateTimeOut", string chkExport = "0", string fromdate = "", string todate = "")
        {
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            #region Excel
            if (chkExport.Equals("1"))
            {
                var listExcel = _ReportService.GetReportDetailMoneyCardDay3_Excel(key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                //Xuất file theo format
                ReportDetailMoneyCardDay3FormatCell(listExcel, "Chi_tiết_thu_tiền_thẻ_lượt", "Sheet1", "", "Chi tiết thu tiền thẻ lượt", datefrompicker);

                return RedirectToAction("ReportDetailMoneyCardDay3");
            }
            #endregion


            var list = _ReportService.GetReportDetailMoneyCardDay3(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        public ActionResult PrintReportDetailMoneyCardDay3(string key = "", string user = "", string lane = "", string datefrompicker = "", string cardgroup = "", string column = "DateTimeOut", string fromdate = "", string todate = "", int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            var list = _ReportService.PrintReportDetailMoneyCardDay3(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);


            //  var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.columnSort = column;

            //ViewBag.eventCodeValue = eventcode;
            ViewBag.totalMoney = totalMoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(list);
        }


        #region Format cell lên excel
        private void ReportDetailMoneyCardDay3FormatCell(DataTable listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian ra", ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Làn vào", ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = "Làn ra", ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát vào", ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = "Giám sát ra", ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền(VNĐ)", ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng thời gian", ItemValue = "Tổng thời gian" });
            listColumn.Add(new SelectListModel { ItemText = "Voucher", ItemValue = "Voucher" });

            //Chuyển dữ liệu về datatable
            //  DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(listData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo doanh thu ca
        public ActionResult ReportTotalMoneyByCardGroupCAB2VCT(string cardgroup = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            var datefrompicker = "";
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = string.Format("TỪ {0} ĐẾN {1}", fromdate, todate);
            }

            var dt = _ReportService.GetReportTotalMoneyByCardGroupCAB2VCT(cardgroup, fromdate, todate);

            var table = _ReportService.GetReportTotalVehicleByCardGroup_CAB2VCT20003000(cardgroup, fromdate, todate);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {

                //Xuất file theo format
                BindDataToExcel(dt, table, "BÁO CÁO DOANH THU BÃI XE MÁY THEO CA", datefrompicker);

                return RedirectToAction("ReportTotalMoneyByCardGroupCAB2VCT");
            }
            #endregion
            ViewBag.dt = dt;
            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(table);
        }

        public ActionResult PrintReportTotalMoneyByCardGroupCAB2VCT(string cardgroup = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            var datefrompicker = "";


            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            ViewBag.dt = _ReportService.GetReportTotalMoneyByCardGroupCAB2VCT(cardgroup, fromdate, todate);

            var table = _ReportService.GetReportTotalVehicleByCardGroup_CAB2VCT20003000(cardgroup, fromdate, todate);

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(table);
        }

        public void BindDataToExcel(DataTable _dt1, DataTable _dt2, string _title1, string _title2)
        {
            var dtHeader = getHeaderExcel(_title1, _title2);
            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelFile(new MemoryStream(), _dt1, _dt2, dtHeader);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={1}-{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmm"), _title1.Replace(" ", "-")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }

        public static DataTable getHeaderExcel(string title1, string title2)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("header", typeof(string));

                dt.Rows.Add("XÍ NGHIỆP DỊCH VỤ VÀ CHO THUÊ VĂN PHÒNG ");
                dt.Rows.Add("ĐỊA CHỈ: 105 LÁNG HẠ, ĐỐNG ĐA, HÀ NỘI");
                dt.Rows.Add("MÃ SỐ THUẾ:");
                dt.Rows.Add("");
                dt.Rows.Add(title1);
                dt.Rows.Add(title2);

                return dt;
            }
            catch
            { }

            return null;
        }

        public static Stream CreateExcelFile(Stream stream = null, DataTable dt1 = null, DataTable dt2 = null, DataTable dtHeader = null, DataTable dtFooter = null)
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Futech";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Báo cáo";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "Báo cáo";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("Sheet1");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];
                //workSheet.Protection.IsProtected = false;
                // Đổ data vào Excel file
                //workSheet.Cells[1, 1].LoadFromDataTable(dt, true, TableStyles.Dark9);
                BindingFormatForExcel(workSheet, dt1, dt2, dtHeader);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        public static void BindingFormatForExcel(ExcelWorksheet worksheet, DataTable dt1, DataTable dt2, DataTable dtHeader)
        {
            double totalmoney = 0;
            worksheet.Cells[1, 1, 1, 5].Merge = true;
            worksheet.Cells[2, 1, 2, 5].Merge = true;
            worksheet.Cells[3, 1, 3, 5].Merge = true;
            worksheet.Cells[4, 1, 4, 5].Merge = true;
            worksheet.Cells[5, 1, 5, 5].Merge = true;
            worksheet.Cells[6, 1, 6, 5].Merge = true;
            worksheet.Cells["A8:E8"].Merge = true;
            worksheet.Cells["A8:E8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A11:E11"].Merge = true;
            worksheet.Cells["A11:E11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A14:C14"].Merge = true;
            worksheet.Cells["A14:C14"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A15:C15"].Merge = true;
            worksheet.Cells["A15:C15"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A16:C16"].Merge = true;
            worksheet.Cells["A16:C16"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A17:C17"].Merge = true;
            worksheet.Cells["A17:C17"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A18:C18"].Merge = true;
            worksheet.Cells["A18:C18"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A19:C19"].Merge = true;
            worksheet.Cells["A19:C19"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A20:C20"].Merge = true;
            worksheet.Cells["A20:C20"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["A21:E21"].Merge = true;
            worksheet.Cells["A21:E21"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            worksheet.Cells.AutoFitColumns();
            // Tạo header
            if (dtHeader != null && dtHeader.Rows.Count > 0)
            {
                for (int i = 1; i < dtHeader.Rows.Count + 1; i++)
                {
                    DataRow dr = dtHeader.Rows[i - 1];
                    worksheet.Cells[i, 1].Value = dr["header"].ToString();
                    if (i == 1 || i == 2)
                    {
                        worksheet.Cells[i, 1].Style.Font.Bold = true;
                    }

                    if (i == 5)
                    {
                        worksheet.Cells[i, 1].Style.Font.Size = 16;
                        worksheet.Cells[i, 1].Style.Font.Bold = true;
                        worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    if (i == 6)
                    {
                        worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
            }
            #region header xe vao
            worksheet.Cells[dtHeader.Rows.Count + 2, 1].Value = "Số lượt xe vào";
            worksheet.Cells[dtHeader.Rows.Count + 2, 1].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 2, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 3, 1].Value = "Xe máy nhân viên";
            worksheet.Cells[dtHeader.Rows.Count + 3, 1].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 3, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 3, 2].Value = "Xe máy khách";
            worksheet.Cells[dtHeader.Rows.Count + 3, 2].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 3, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 3, 3].Value = "Xe đạp nhân viên";
            worksheet.Cells[dtHeader.Rows.Count + 3, 3].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 3, 4].Value = "Xe đạp khách";
            worksheet.Cells[dtHeader.Rows.Count + 3, 4].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 3, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 3, 5].Value = "Tổng số";
            worksheet.Cells[dtHeader.Rows.Count + 3, 5].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 3, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 3, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            #endregion

            #region header xe ra
            worksheet.Cells[dtHeader.Rows.Count + 5, 1].Value = "Số lượt xe ra";
            worksheet.Cells[dtHeader.Rows.Count + 5, 1].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 5, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 6, 1].Value = "Xe máy nhân viên";
            worksheet.Cells[dtHeader.Rows.Count + 6, 1].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 6, 2].Value = "Xe máy khách";
            worksheet.Cells[dtHeader.Rows.Count + 6, 2].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 6, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 6, 3].Value = "Xe đạp nhân viên";
            worksheet.Cells[dtHeader.Rows.Count + 6, 3].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 6, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 6, 4].Value = "Xe đạp khách";
            worksheet.Cells[dtHeader.Rows.Count + 6, 4].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 6, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 6, 5].Value = "Tổng số";
            worksheet.Cells[dtHeader.Rows.Count + 6, 5].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 6, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 5, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 6, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 6, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 6, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 6, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 6, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            #endregion

            #region header chi tiết
            worksheet.Cells[dtHeader.Rows.Count + 8, 1].Value = "Chi tiết";
            worksheet.Cells[dtHeader.Rows.Count + 8, 1].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 8, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 8, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 8, 4].Value = "Số lượt";
            worksheet.Cells[dtHeader.Rows.Count + 8, 4].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 8, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[dtHeader.Rows.Count + 8, 5].Value = "Thành tiền";
            worksheet.Cells[dtHeader.Rows.Count + 8, 5].Style.Font.Bold = true;
            worksheet.Cells[dtHeader.Rows.Count + 8, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dtHeader.Rows.Count + 8, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            #endregion


            if (dt1 != null && dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    worksheet.Cells[dtHeader.Rows.Count + 4, j + 1].Value = dt1.Rows[j]["TotalVehicleIn"].ToString();
                    worksheet.Cells[dtHeader.Rows.Count + 4, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 4, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[dtHeader.Rows.Count + 7, j + 1].Value = dt1.Rows[j]["TotalVehicleOut"].ToString();
                    worksheet.Cells[dtHeader.Rows.Count + 7, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 7, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

            }

            if (dt2 != null && dt2.Rows.Count > 0)
            {

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    totalmoney += Convert.ToDouble(dt2.Rows[i]["Moneys"].ToString());
                    switch (i)
                    {
                        case 0:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe máy khách thu 2.000đ (7h01 - 19h)";
                            break;
                        case 1:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe máy khách thu 3.000đ (19h01 - 7h)";
                            break;
                        case 2:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe đạp khách thu 1.000đ (7h01 - 19h)";
                            break;
                        case 3:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe đạp khách thu 2.000đ (19h01 - 7h)";
                            break;
                        case 4:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe máy quá giờ";
                            break;
                        case 5:
                            worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Value = "Số xe đạp quá giờ";
                            break;
                    }

                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 4].Value = dt2.Rows[i]["VehicleName"].ToString();

                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 5].Value = dt2.Rows[i]["Moneys"].ToString().FormatMoney() + " VNĐ";

                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[dtHeader.Rows.Count + 9 + i, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                }
            }

            worksheet.Cells[dtHeader.Rows.Count + 15, 1].Value = "Tổng doanh thu:                              " + totalmoney.ToString().FormatMoney() + " VNĐ";

            //    }
            //}

            //worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Value = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            //worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //worksheet.Cells[dt.Rows.Count + rowStart + 3, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Value = "Người lập báo cáo";
            //worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //worksheet.Cells[dt.Rows.Count + rowStart + 5, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Value = GetFullName(userid);
            //worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            //worksheet.Cells[dt.Rows.Count + rowStart + 7, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            worksheet.Cells.AutoFitColumns();
        }
        #endregion

        #region Báo cáo xe đỗ quá ngày
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportVehicleTooDay(string key = "", string datefrompicker = "", int page = 1, string chkExport = "0", string fromdate = "", string todate = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportVehicleTooDay_Excel(key, fromdate, todate);
                //if (listExcel != null && listExcel.Rows.Count > 0)
                //{
                //    listExcel.Columns.Remove("CustomerGroupID");
                //}
                //Xuất file theo format
                ReportVehicleTooDayFormatCell(listExcel, "", "Báo_cáo_xe_đỗ_quá_ngày", "Sheet1", "", "Báo cáo xe đỗ quá ngày", datefrompicker);

                return RedirectToAction("ReportVehicleTooDay");
            }
            #endregion

            var list = _ReportService.GetReportVehicleTooDay(key, fromdate, todate, page, pageSize, ref totalItem);

            var gridModel = PageModelCustom<ReportVehicleTooDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;

            ViewBag.totalMoney = totalMoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        #region Format cell lên excel
        private void ReportVehicleTooDayFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Plate" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian vào", ItemValue = "DateTimeIn" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian ra", ItemValue = "DatetimeOut" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Số ngày quá hạn", ItemValue = "TooDay" });


            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Báo cáo nhóm thẻ nhân viên
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportInOutByCustomer(string customergroup = "", string key = "", string datefrompicker = "", int page = 1, string chkExport = "0", string fromdate = "", string todate = "")
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            long totalMoney = 0;

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportInOutByCustomer_Excel(key, strCG, fromdate, todate);
                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    listExcel.Columns.Remove("CustomerGroupID");
                }
                //Xuất file theo format
                ReportInOutByCustomerFormatCell(listExcel, "", "Báo_cáo_nhóm_thẻ_của_nhân_viên", "Sheet1", "", "Báo cáo nhóm thẻ của nhân viên", datefrompicker);

                return RedirectToAction("ReportInOutByCustomer");
            }
            #endregion

            var list = _ReportService.GetReportInOutByCustomer(key, strCG, fromdate, todate, page, pageSize, ref totalItem);

            var gridModel = PageModelCustom<ReportInOutByCustomer>.GetPage(list, page, pageSize, totalItem);


            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.totalMoney = totalMoney;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        #region Format cell lên excel
        private void ReportInOutByCustomerFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "RowNumber" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Nhân viên", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Số ngày sử dụng", ItemValue = "Day" });


            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #endregion

        #region 108

        #region Báo cáo nội bộ
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportInternal(string voucher ,string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            string number = "";
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportDetailMoneyCardDayExcel(voucher,number ,key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }


                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }


                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                }

                //Xuất file theo format
                ReportDetailMoneyCardDayFormatCell(listExcel, "", "Báo_cáo_nội_bộ", "Sheet1", "", "Báo cáo nội bộ", datefrompicker);

                return RedirectToAction("ReportInternal");
            }
            #endregion

            if (chkExport == "2")
            {
                return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            }

            #region Giao diện
            var list = _ReportService.GetReportInternal(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;
            ViewBag.LstPublicEvent = _PublicEventService.GetId();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            return View(gridModel);
            #endregion
        }

        public JsonResult PublicEvent(string id, bool IsChk)
        {
            var result = new MessageReport();
            if (IsChk)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var publicEvent = _PublicEventService.GetByEventId(id);
                    if (publicEvent == null)
                    {
                        var newobj = new PublicEvent
                        {
                            Id = Guid.NewGuid(),
                            EventID = id
                        };

                        result = _PublicEventService.Create(newobj);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var publicEvent = _PublicEventService.GetByEventId(id);
                    if (publicEvent != null)
                    {
                        result = _PublicEventService.DeleteById(publicEvent.Id.ToString());
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PublicAll(string arrAll, bool IsChk)
        {
            var result = new MessageReport();
            var lstall = new List<string>();
            if (!string.IsNullOrEmpty(arrAll))
            {
                var t = arrAll.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    foreach (var item in t)
                    {
                        lstall.Add(item.Trim());
                    }
                }
            }

            if (IsChk)
            {
                if (lstall != null && lstall.Count > 0)
                {
                    foreach (var id in lstall)
                    {
                        var publicEvent = _PublicEventService.GetByEventId(id);
                        if (publicEvent == null)
                        {
                            var newobj = new PublicEvent
                            {
                                Id = Guid.NewGuid(),
                                EventID = id
                            };

                            result = _PublicEventService.Create(newobj);
                        }
                    }

                }
            }
            else
            {
                if (lstall != null && lstall.Count > 0)
                {
                    result = _PublicEventService.DeleteMulti(lstall);
                }

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Báo cáo công khai
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportPublic(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var lstpublicevent = _PublicEventService.GetId();

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportPublic_Excel(lstpublicevent, key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                }

                //Xuất file theo format
                ReportDetailMoneyCardDayFormatCell(listExcel, "", "Báo_cáo_công_khai", "Sheet1", "", "Báo cáo công khai", datefrompicker);

                return RedirectToAction("ReportPublic");
            }
            #endregion

            if (chkExport == "2")
            {
                return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            }

            #region Giao diện
            var list = _ReportService.GetReportPublic(lstpublicevent, key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;
            ViewBag.LstPublicEvent = _PublicEventService.GetId();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            return View(gridModel);
            #endregion
        }
        #endregion

        #region Biểu đồ vào ra theo nhóm thẻ
        public ActionResult ReportChartInOutByCardGroup(string fromdate = "", string todate = "", string cardgroup = "", string vehiclegroupid = "")
        {
            long _totalIn = 0;
            long _totalOut = 0;
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartInOutByCardGroup(cardgroup, vehiclegroupid, fromdate, todate, ref _totalIn, ref _totalOut);

            var list = ExcuteSQL.DataTableToList<ReportChartInOutByCardGroup>(dt);

            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.TotalIn = _totalIn;
            ViewBag.TotalOut = _totalOut;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }

        #endregion

        #region Biểu đồ vào ra theo làn
        public ActionResult ReportChartInOutByLane(string fromdate = "", string todate = "", string lane = "")
        {
            long _totalIn = 0;
            long _totalOut = 0;
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var list = _ReportService.GetReportChartInOutByLane(lane, fromdate, todate, ref _totalIn, ref _totalOut);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.TotalIn = _totalIn;
            ViewBag.TotalOut = _totalOut;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }

        #endregion

        #region Biểu đồ doanh thu theo làn
        public ActionResult ReportChartMoneyByLane(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {
            long _totalIn = 0;
            long _totalOut = 0;
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var list = _ReportService.GetReportChartMoneyByLane(lane, vehiclegroupid, fromdate, todate, ref _totalIn, ref _totalOut);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;


            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;


            ViewBag.TotalIn = _totalIn;
            ViewBag.TotalOut = _totalOut;


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }
        #endregion

        #region  Biểu đồ lượt xe ra theo làn
        public ActionResult ReportChartOutByLane(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "", string cardgroup = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartOutByLane(vehiclegroupid, cardgroup, lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(dt);
        }

        public JsonResult GetReportChartOutByLane(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "", string cardgroup = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartOutByLane(vehiclegroupid, cardgroup, lane, fromdate, todate);

            var listcolumn = new List<string>(0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (!dc.ColumnName.Equals("CardGroupID"))
                    {
                        listcolumn.Add(dc.ColumnName);
                    }

                }
            }
            var JsonResult = JsonConvert.SerializeObject(dt);

            var obj = new { JsonResult = JsonResult, listcolumn = listcolumn };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Biểu đồ doanh thu theo mức
        public ActionResult ReportChartMoneyByLevel(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var list = _ReportService.GetReportMoneyByLevel(vehiclegroupid, lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;


            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }
        #endregion

        #region Biểu đồ lượt xe ra theo mức thu
        public ActionResult ReportChartOutByLevel(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartOutByLevel(vehiclegroupid, lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(dt);
        }

        public JsonResult GetReportChartOutByLevel(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartOutByLevel(vehiclegroupid, lane, fromdate, todate);

            var listcolumn = new List<string>(0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (!dc.ColumnName.Equals("LaneIDOut"))
                    {
                        listcolumn.Add(dc.ColumnName);
                    }

                }
            }
            var JsonResult = JsonConvert.SerializeObject(dt);

            var obj = new { JsonResult = JsonResult, listcolumn = listcolumn };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Biểu đồ lượt xe ra theo thời gian
        public ActionResult ReportChartOutByTime(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "", string cardgroup = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartOutByTime(vehiclegroupid, cardgroup, lane, fromdate, todate);

            var listcolumn = new List<string>(0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (!dc.ColumnName.Equals("LaneIDOut"))
                    {
                        listcolumn.Add(dc.ColumnName);
                    }

                }
            }
            ViewBag.JsonResult = JsonConvert.SerializeObject(dt);
            ViewBag.ListColumn = listcolumn;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;


            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(dt);
        }

        public JsonResult GetReportChartOutByTime(string jsondt = "")
        {
            jsondt = jsondt.Replace("&quot;", @"""");

            return Json(jsondt, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Biểu đồ doanh thu theo thời gian
        public ActionResult ReportChartMoneyByTime(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartMoneyByTime(vehiclegroupid, lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;


            ViewBag.VehicleGroupDT = GetVehicleGroupList().ToDataTableNullable();
            ViewBag.VehicleGroupId = vehiclegroupid;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(dt);
        }

        public JsonResult GetReportChartMoneyByTime(string fromdate = "", string todate = "", string lane = "", string vehiclegroupid = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var dt = _ReportService.GetReportChartMoneyByTime(vehiclegroupid, lane, fromdate, todate);

            var listcolumn = new List<string>(0);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (!dc.ColumnName.Equals("VehicleGroupID"))
                    {
                        listcolumn.Add(dc.ColumnName);
                    }

                }
            }
            var JsonResult = JsonConvert.SerializeObject(dt);

            var obj = new { JsonResult = JsonResult, listcolumn = listcolumn };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Biểu đồ vào ra theo thời gian
        public ActionResult ReportChartInOutByTime(string fromdate = "", string todate = "", string lane = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var list = _ReportService.GetReportChartInOutByTime(lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            //ViewBag.Type = FunctionHelper.Action

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }
        #endregion

        #endregion

        #region BVDK_THANHPHO_VINH

        #region Báo cáo doanh thu theo nhóm thẻ lượt
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult BVDK_ReportTotalMoneyByCardGroup(string cardgroup = "", string key = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            long _totalmoneys = 0;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            var dt = _ReportService.GetBVDK_ReportTotalMoneyByCardGroup(cardgroup, fromdate, todate, ref _totalmoneys);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                dt.Columns.Add("STT", typeof(string)).SetOrdinal(0);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        count++;

                        dr["STT"] = count;
                    }

                    dt.Rows.Add("#", "Tổng số", _totalmoneys > 0 ? _totalmoneys.ToString("###,###") : "0");
                }
                //Xuất file theo format
                BVDK_ReportTotalMoneyByCardGroupFormatCell(dt, "Báo_cáo_thu_tiền_theo_nhóm_thẻ_lượt", "Sheet1", "", "BÁO CÁO THU TIỀN THEO NHÓM THẺ LƯỢT", datefrompicker);

                return RedirectToAction("BVDK_ReportTotalMoneyByCardGroup");
            }
            #endregion

            #region Giao diện

            ViewBag.CardGroups = GetCardGroupList().Where(n => n.CardType == 1).ToList();
            ViewBag.CardGroupDT = GetCardGroupList().Where(n => n.CardType == 1).ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.dt = dt;

            ViewBag.TotalMoneys = _totalmoneys.ToString().FormatMoney();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.KeyWord = key;

            return View();
            #endregion
        }

        public ActionResult PrintBVDK_ReportTotalMoneyByCardGroup(string cardgroup = "", string key = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
        {
            long _totalmoneys = 0;


            ViewBag.dt = _ReportService.GetBVDK_ReportTotalMoneyByCardGroup(cardgroup, fromdate, todate, ref _totalmoneys);
            #region Giao diện


            ViewBag.TotalMoneys = _totalmoneys.ToString().FormatMoney();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.KeyWord = key;
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View();
            #endregion
        }

        #region Format cell lên excel
        private void BVDK_ReportTotalMoneyByCardGroupFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });

            listColumn.Add(new SelectListModel { ItemText = "Doanh thu(VNĐ)", ItemValue = "Moneys" });



            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFileBVDK_ReportTotalMoneyByCardGroup(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #endregion

        #endregion

        #region FPT

        #region Cảnh báo quá lượt sử dụng
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult FPT_AlarmExceededTurn(string turn = "0", string key = "", string cardgroup = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            int n;
            bool isNumeric = int.TryParse(turn, out n);

            if (!isNumeric)
            {
                turn = "0";
            }

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetAlarmExceededTurn_FPTExcel(key, fromdate, todate, cardgroup, turn, ref totalItem);

                //Xuất file theo format
                FPT_AlarmExceededTurnFormatCell(listExcel, "", "Cảnh_báo_quá_lượt_sử_dụng", "Sheet1", "", "Cảnh báo quá lượt sử dụng", datefrompicker);

                return RedirectToAction("FPT_AlarmExceededTurn");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetAlarmExceededTurn_FPT(key, fromdate, todate, cardgroup, turn, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<AlarmTurnFPT>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.KeyWord = key;
            ViewBag.Turn = turn;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void FPT_AlarmExceededTurnFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Số lượt", ItemValue = "Số lượt" });



            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Cảnh báo không sử dụng thẻ tháng
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult FPT_AlarmNotUse(string number = "0", string customergroup = "", string active = "", string key = "", string cardgroup = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            var strCG = new List<string>();

            GetListChild(strCG, customergroup);

            int n;
            bool isNumeric = int.TryParse(number, out n);

            if (!isNumeric)
            {
                number = "0";
            }

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetAlarmNotUse_FPTExcel(key, strCG, active, fromdate, todate, cardgroup, number, ref totalItem);

                //Xuất file theo format
                FPT_AlarmNotUseFormatCell(listExcel, "", "Cảnh_báo_không_sử_dụng_thẻ_tháng", "Sheet1", "", "Cảnh báo không sử dụng thẻ tháng", datefrompicker);

                return RedirectToAction("FPT_AlarmNotUse");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.GetAlarmNotUse_FPT(key, strCG, active, fromdate, todate, cardgroup, number, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<AlarmNotUseFPT>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.CustomerGroupId = customergroup;
            ViewBag.lcustomergroups = GetMenuList();

            ViewBag.lactives = FunctionHelper.CardStatus();
            ViewBag.activeValue = active;

            ViewBag.KeyWord = key;
            ViewBag.Number = number;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        #region Format cell lên excel
        private void FPT_AlarmNotUseFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm KH", ItemValue = "Nhóm KH" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái thẻ", ItemValue = "Trạng thái thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Thời điểm cuối", ItemValue = "Thời điểm cuối" });
            listColumn.Add(new SelectListModel { ItemText = "Số ngày không sử dụng", ItemValue = "Số ngày không sử dụng" });



            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region Biểu đồ vào ra theo thời gian
        public ActionResult FPT_ChartInOutByTime(string fromdate = "", string todate = "", string lane = "")
        {

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }
            var list = _ReportService.FPT_GetChartInOutByTime(lane, fromdate, todate);

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            //ViewBag.Type = FunctionHelper.Action

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            return View(list);
        }
        #endregion
        #endregion

        #region Danh sách sử dụng

        public List<SelectListModel> GetCompartmentListNew()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listResidentGroup = _tblCompartmentService.GetAll().ToList();
            if (listResidentGroup.Any())
            {
                foreach (var item in listResidentGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.CompartmentID.ToString(), ItemText = item.CompartmentName });
                }
            }
            

            return list;
        }
        public List<SelectListModel> GetVouchers()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listResidentGroup = _ReduceParkingFeesService.GetAll().ToList();
            if (listResidentGroup.Any())
            {
                foreach (var item in listResidentGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.DCTypeCode.ToString(), ItemText = item.DCTypeName });
                }
            }
           

            return list;
        }

        public List<SelectListModel2> GetCardGroupListNew()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel2>();
            var listResidentGroup = _tblCardGroupService.GetAll().ToList();
            if (listResidentGroup.Any())
            {
                foreach (var item in listResidentGroup)
                {
                    list.Add(new SelectListModel2 { ItemValue = item.CardGroupID.ToString(), ItemOtherValue = item.CardType.ToString(), ItemText = item.CardGroupName });
                }
            }
            if (_ReportService.SystemUsingLoop())
            {
                list.Add(new SelectListModel2 { ItemValue = "LOOP_D", ItemOtherValue = "loop", ItemText = "Vòng từ - Xe lượt(Loop)" });
                list.Add(new SelectListModel2 { ItemValue = "LOOP_M", ItemOtherValue = "loop", ItemText = "Vòng từ-Xe tháng(Loop)" });
            }

            return list;
        }

        public List<SelectListModel> GetSendTypes()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listResidentGroup = FunctionHelper.SendTypes();
            return listResidentGroup;
        }
        public List<SelectListModel> GetLaneListNew()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listLaneGr = _tblLaneService.GetAll().ToList();
            if (listLaneGr.Any())
            {
                foreach (var item in listLaneGr)
                {
                    list.Add(new SelectListModel { ItemValue = item.LaneID.ToString(),  ItemText = item.LaneName });
                }
            }
          

            return list;
        }
        private IEnumerable<tblCardGroup> GetCardGroupList()
        {
            return _tblCardGroupService.GetAll();
        }
        private IEnumerable<tblCardGroup> GetCardGroupListMonth()
        {
            return _tblCardGroupService.GetAllActiveMonth();
        }

        private IEnumerable<tblCompartment> GetCompartmentList()
        {
            return _tblCompartmentService.GetAll();
        }

        private IEnumerable<tblLane> GetLaneList()
        {
            return _tblLaneService.GetAll();
        }

        private IEnumerable<User> GetUserList()
        {
            return _UserService.GetAll();
        }

        private IEnumerable<tblVehicleGroup> GetVehicleGroupList()
        {
            return _tblVehicleGroupService.GetAll();
        }

        private IEnumerable<tblCustomerGroup> GetCustomerGroupList()
        {
            return _tblCustomerGroupService.GetAll();
        }

        private void GetListChild(List<string> str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                // if (str.Any())
                //  {
                str.Add(id);
                // }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str.Add(item.CustomerGroupID.ToString());
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }
        }
        private void GetListChildCustomer(List<string> str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                // if (str.Any())
                //  {
                str.Add(id);
                // }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str.Add(item.CustomerGroupID.ToString());
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }
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



        #endregion

        #region Excel
        private void ExportFile(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments);
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            //ExcelExport exp = new ExcelExport();
            Response.End();
        }

        private void ExportFile_ByCG_LOTTE(List<TotalByCardGroup> list = null, IEnumerable<tblVehicleGroup> vehicleGroups = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.WriteToExcel_ByCardGroup_LOTTE(null, vehicleGroups, list, listTitle, dtHeader, sheetname, comments);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }

        private void ExportFileBVDK(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.BVDK_WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }

        private void ExportFileBVDK_ReportTotalMoneyByCardGroup(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.WriteToExcelBVDK_ReportTotalMoneyByCardGroup(null, list, listTitle, dtHeader, sheetname, comments);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }
        private List<SelectListModel> GetExcelColumnByAction(string action)
        {
            var list = new List<SelectListModel>();
            list.Add(new SelectListModel { ItemValue = "0", ItemText = "Tất cả" });
            var _menu = _MenuFunctionService.GetByControllerAction("Report", action);

            if (_menu != null)
            {
                var obj = _ExcelColumnService.GetByMenuFunctionId(_menu.Id);

                if (obj != null)
                {
                    var arr = obj.ColName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (arr.Length > 0)
                    {
                        foreach (var m in arr)
                        {
                            list.Add(new SelectListModel { ItemValue = m.Trim(), ItemText = m.Trim() });
                        }
                    }
                }
            }

            return list;
        }

        #endregion

        #region Bạch Mai

        #region Báo cáo sổ 2 (Báo cáo thuế)
        /// <summary>
        /// Báo cáo sổ 2 (Báo cáo thuế)
        /// </summary>
        /// <param name="customergroup"></param>
        /// <param name="key"></param>
        /// <param name="user"></param>
        /// <param name="cardgroup"></param>
        /// <param name="lane"></param>
        /// <param name="chkExport"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult BachMai_ReportS2(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {

            var title = ConfigurationManager.AppSettings["ReportS2"];
            var systemconfig = _tblSystemConfigService.GetDefault();
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");

            var totalItem = 0;
            double totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport == "0")
            {
                _ReportService.InsertEventBachMai(systemconfig.CustomInfo, key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportSoHaiNEWExcel();

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                    double _totalMoney = 0;

                    _totalMoney = Convert.ToDouble(item["Tiền"].ToString());

                    totalMoney += _totalMoney;
                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát ra"] = Dictionary["total"];
                totalMoneyRow["Tiền"] = totalMoney.ToString();
                listExcel.Rows.Add(totalMoneyRow);


                //Xuất file theo format
                ReportSoHaiFormatCell(listExcel, "", !string.IsNullOrEmpty(title) ? title.Replace(' ', '_') : "", "Sheet1", "", title, datefrompicker);
                return RedirectToAction("BachMai_ReportS2");
            }
            #endregion

            //if (chkExport == "2")
            //{
            //    return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            //}

            #region Giao diện
            var list = _ReportService.GetReportSoHaiNEW(page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.KeyWord = key;
            ViewBag.Title = title;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;


            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            return View(gridModel);
            #endregion
        }

        public ActionResult PrintBachMai_ReportS2(string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            var title = ConfigurationManager.AppSettings["ReportS2"];
            var objsystem = _tblSystemConfigService.GetDefault();
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            double totalMoney = 0;

            var list = _ReportService.GetReportSoHaiNEW(page, pageSize, ref totalItem, ref totalMoney);

            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;
            ViewBag.Title = title;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;

            ViewBag.FilterLink = $"/Parking/Report/PrintReportDetailMoneyCardDay?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";
            ViewBag.System = objsystem;
            ViewBag.PageSize = pageSize;

            return View(gridModel);
        }

        private void ReportSoHaiFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " :" + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeAll"], ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file

            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username, 200);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }
        #endregion

        #region Báo cáo chỉ có nhóm xe đạp
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult BachMai_ReportS3(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            var title = ConfigurationManager.AppSettings["ReportS3"];
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");

            var totalItem = 0;
            long totalMoney = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportS3Excel(key, user, fromdate, todate, cardgroup, lane, page, pageSize);

                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());
                    item["Tổng thời gian"] = StringUtil.CalculateTimeDiff(item["Thời gian vào"].ToString(), item["Thời gian ra"].ToString());

                    var _laneIn = !string.IsNullOrEmpty(item["Làn vào"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString())) : new tblLane();
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = !string.IsNullOrEmpty(item["Làn ra"].ToString()) ? _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString())) : new tblLane();
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                    long _totalMoney = 0;

                    long.TryParse(item["Tiền"].ToString(), out _totalMoney);

                    totalMoney += _totalMoney;
                }

                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["Giám sát ra"] = Dictionary["total"];
                totalMoneyRow["Tiền"] = totalMoney.ToString();
                listExcel.Rows.Add(totalMoneyRow);


                //Xuất file theo format
                ReportS3FormatCell(listExcel, "", !string.IsNullOrEmpty(title) ? title.Replace(' ', '_') : "", "Sheet1", "", title, datefrompicker);

                return RedirectToAction("BachMai_ReportS3");
            }
            #endregion

            //if (chkExport == "2")
            //{
            //    return RedirectToAction("PrintReportDetailMoneyCardDay", new { key = key, user = user, cardgroup = cardgroup, lane = lane, fromdate = fromdate, todate = todate, totalItem = totalItem });
            //}

            #region Giao diện
            var list = _ReportService.GetReportS3(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;
            ViewBag.Title = title;
            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.TotalMoney = totalMoney;

            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            return View(gridModel);
            #endregion
        }

        public ActionResult PrintBachMai_ReportS3(string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", int totalItem = 0, int page = 1, int pageSize = 500)
        {
            var title = ConfigurationManager.AppSettings["ReportS3"];
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            long totalMoney = 0;

            var list = _ReportService.GetReportS3(key, user, fromdate, todate, cardgroup, lane, page, pageSize, ref totalItem, ref totalMoney);

            var gridModel = PageModelCustom<ReportDetailMoneyCardDay>.GetPage(list, page, pageSize, totalItem);

            ViewBag.Key = key;

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserId = user;
            ViewBag.Title = title;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TotalMoney = totalMoney;

            ViewBag.FilterLink = $"/Parking/Report/PrintReportDetailMoneyCardDay?key={key}&user={user}&fromdate={fromdate}&todate={todate}&cardgroup={cardgroup}&lane={lane}&totalItem={totalItem}&page=1";
            ViewBag.System = _tblSystemConfigService.GetDefault();
            ViewBag.PageSize = pageSize;

            return View(gridModel);
        }

        private void ReportS3FormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " :" + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Tiền" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeAll"], ItemValue = "Tổng thời gian" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file

            var objsystem = _tblSystemConfigService.GetDefault();
            if (objsystem != null && objsystem.FeeName == "BVDK_THANHPHO_VINH")
            {
                dtHeader = _tblSystemConfigService.getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(titleSheet, timeSearch, user.Username, 200);
                ExportFileBVDK(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
            else
            {
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }

        }
        #endregion

        #endregion

        #region Ba Vì
        public ActionResult ReportInvoiceBAVI(string key = "", string fromdate = "", string todate = "", bool IsFilterByTimeIn = false, string IsSync = "", int page = 1)
        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var list = _ReportService.GetReportInvoiceBavi(key, fromdate, todate, IsFilterByTimeIn, IsSync, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<ReportInvoiceBAVI>.GetPage(list, page, pageSize, totalItem);


            //     ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = key;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            ViewBag.IsSync = IsSync;
            ViewBag.isFilterByTimeIn = IsFilterByTimeIn;

            return View(gridModel);
        }

        public async Task<JsonResult> SyncInvoiceBAVI(string eventId, string invoiceCreatedDate = "")
        {
            var report = new MessageReport();

            try
            {
                var dateCreated = DateTime.ParseExact(invoiceCreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                var currentEv = _tblCardEventService.GetById(eventId);
                if (currentEv == null)
                {
                    report.Message = "Bản ghi không tồn tại";
                }
                else
                {
                    report = await SaveInvoiceAPIBavi(currentEv.PlateOut, currentEv.CardNumber, (DateTime)currentEv.DatetimeIn, (DateTime)currentEv.DateTimeOut, currentEv.CardNo, eventId, (long)currentEv.Moneys, dateCreated);

                    if (report.isSuccess)
                    {
                        _ReportService.UpdateInvoiceStatus(eventId, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                report.Message = ex.Message;
            }

            return await Task.FromResult(Json(report));
        }

        public static async Task<string> LoginAPIBavi()
        {
            var result = "";
            try
            {
                var url = "http://0104019005.itcinvoice.vn/api/Account/Login";
                //var url = "http://testapi.minvoice.com.vn/api/Account/Login";

                var obj = new
                {
                    username = "FUTECH",
                    password = "bavi@123",
                    ma_dvcs = "VP"
                };

                var response = await ApiHelper.HttpPost(url, obj);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var objToken = await ApiHelper.ConvertResponse<TokenResult>(response);

                    result = objToken.token;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //
            }

            return result;
        }

        public static async Task<MessageReport> SaveInvoiceAPIBavi(string plate, string cardnumber, DateTime dtimein, DateTime dtimeout, string cardno, string eventid, long money, DateTime dateCreated)
        {
            var token = await LoginAPIBavi();
            var result = new MessageReport() { isSuccess = false, Message = "" };

            if (string.IsNullOrWhiteSpace(token))
            {
                //save log to text file
                result.Message = $"Không lấy được token đăng nhập";
                return result;
            }
            else
            {

                var _timeIn = (int)(dtimeout - dtimein).TotalMinutes;
                var _totalTime = GetTimeDetailFromMinutes(_timeIn);

                var url = "http://0104019005.itcinvoice.vn/api/InvoiceAPI/Save";
                //var url = "http://testapi.minvoice.com.vn/api/InvoiceAPI/Save";

                var objDataDetail = new[]
                {
                    new
                    {
                        stt_rec0 = "",
                        inv_itemCode = "",
                        inv_itemName = "Dịch vụ trông giữ xe",
                        inv_unitCode = "",
                        inv_unitName = "Lượt",
                        inv_unitPrice = 0,
                        inv_quantity = 1,
                        inv_TotalAmountWithoutVat = money,
                        inv_vatAmount = 0,
                        inv_TotalAmount = money,
                        inv_promotion = false,
                        inv_discountPercentage = 0,
                        inv_discountAmount = 0,
                        ma_thue = "0",
                    }
                };

                var objDetail = new[]
                {
                    new
                    {
                        tab_id = "TAB00192",
                        data = objDataDetail
                    },
                };

                var objData = new[]
                {
                    new
                    {
                        inv_invoiceSeries = "AB/20E",
                        inv_invoiceIssuedDate = dateCreated.ToString("yyyy-MM-dd"), //DateTime.Now.ToString("yyyy-MM-dd"), //"2020-02-28",
                        inv_currencyCode = "VND",
                        inv_exchangeRate = 1,
                        inv_buyerDisplayName = "Khách hàng không lấy hóa đơn",
                        ma_dt = "VP",
                        inv_buyerLegalName = "",
                        inv_buyerTaxCode = "",
                        inv_buyerAddressLine = "",
                        inv_buyerEmail = "",
                        inv_buyerBankAccount = "",
                        inv_buyerBankName = "",
                        inv_paymentMethodName = "Tiền mặt/Chuyển khoản",
                        inv_sellerBankAccount = "",
                        inv_sellerBankName = "",
                        mau_hd = "02GTTT0/002",
                        bien_so_xe = plate, //"30H44843",
                        ma_the = cardnumber, //"AB456KHDF",
                        gio_vao = dtimein.ToString("dd/MM/yyyy HH:mm"), //"29/02/2020 17:30",
                        gio_ra = dtimeout.ToString("dd/MM/yyyy HH:mm"), //"29/02/2020 19:30",
                        thoi_gian_do = _totalTime, //"2h00",
                        so_the = cardno, //"AH000091",
                        Fkey = eventid,

                        details = objDetail,
                    }
                };

                var obj = new
                {
                    windowid = "WIN00189",
                    editmode = 1,
                    data = objData
                };

                var response = await ApiHelper.HttpPost(url, obj, "Bear", token + ";VP;vi");

                if (!response.IsSuccessStatusCode)
                {
                    result.Message = $"Lỗi API : ({(int)response.StatusCode}){response.StatusCode.ToString()}";
                    return result;
                }
                else
                {
                    var _responseData = await ApiHelper.ConvertResponse<APIResult>(response);

                    if (_responseData.ok)
                    {
                        result.Message = $"Cập nhật hóa đơn thành công. Mã hóa đơn : {_responseData.data.inv_invoiceNumber1}";
                        result.isSuccess = true;
                        return result;
                    }
                    else
                    {
                        result.Message = $"Lỗi dữ liệu : {_responseData.error}";
                        return result;
                    }
                }
            }
        }

        public static string GetTimeDetailFromMinutes(int minutes)
        {
            string s = "";
            s = minutes / 60 + "h" + (minutes % 60) + "m";
            return s;
        }

        public class TokenResult
        {
            public string token { get; set; }
        }

        public class APIResult
        {
            public string error { get; set; }
            public bool ok { get; set; }
            public APIResult_Data data { get; set; }
        }

        public class APIResult_Data
        {
            public string inv_invoiceNumber1 { get; set; }
        }
        #endregion

        #region Hoành Bồ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult HOANHBO_ReportInOut(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string excelcol = "", bool IsFilterByTimeIn = false, string fromdate = "", string todate = "", int page = 1)
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportInOut");
            var DictionaryTotal = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var strCG = new List<string>();
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;
            long totalDVT = 0;
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.HOANHBO_GetReportInOut_Excel(keyReplace, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize);

                double total = 0;
                foreach (DataRow item in listExcel.Rows)
                {
                    //item["Ngày hết hạn"] = GetExpireDate(item["Mã thẻ"].ToString());
                    //item["Số ngày còn lại"] = GetDays(item["Mã thẻ"].ToString());

                    var metkhoi = _tblCardService.GetByCardNumber(item["Mã thẻ"].ToString());
                    if (metkhoi != null)
                    {
                        item["Mét khối đăng ký"] = metkhoi.DVT;
                        total += metkhoi.DVT;
                    }
                    else
                    {
                        item["Mét khối đăng ký"] = "";
                    }

                    var _laneIn = _tblLaneService.GetById(Guid.Parse(item["Làn vào"].ToString()));
                    if (_laneIn != null)
                    {
                        item["Làn vào"] = _laneIn.LaneName;
                    }
                    else
                    {
                        item["Làn vào"] = "";
                    }

                    var _laneOut = _tblLaneService.GetById(Guid.Parse(item["Làn ra"].ToString()));
                    if (_laneOut != null)
                    {
                        item["Làn ra"] = _laneOut.LaneName;
                    }
                    else
                    {
                        item["Làn ra"] = "";
                    }

                    if (!item["Nhóm thẻ"].ToString().Equals("LOOP_D") && !item["Nhóm thẻ"].ToString().Equals("LOOP_M"))
                    {
                        var _cardgroup = _tblCardGroupService.GetById(Guid.Parse(item["Nhóm thẻ"].ToString()));
                        if (_cardgroup != null)
                        {
                            item["Nhóm thẻ"] = _cardgroup.CardGroupName;
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "";
                        }
                    }
                    else
                    {
                        if (item["Nhóm thẻ"].ToString().Equals("LOOP_D"))
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe lượt(Loop)";
                        }
                        else
                        {
                            item["Nhóm thẻ"] = "Vòng từ - Xe tháng(Loop)";
                        }
                    }

                    var _userIn = _UserService.GetById(item["Giám sát vào"].ToString());
                    if (_userIn != null)
                    {
                        item["Giám sát vào"] = _userIn.Username;
                    }
                    else
                    {
                        item["Giám sát vào"] = "";
                    }
                    var _userOut = _UserService.GetById(item["Giám sát ra"].ToString());
                    if (_userOut != null)
                    {
                        item["Giám sát ra"] = _userOut.Username;
                    }
                    else
                    {
                        item["Giám sát ra"] = "";
                    }

                }
                var totalMoneyRow = listExcel.NewRow();
                totalMoneyRow["CardNo"] = "Tổng số";
                totalMoneyRow["Mã thẻ"] = listExcel.Rows.Count;
                totalMoneyRow["Giám sát ra"] = "Tổng số mét khối(m3)";
                totalMoneyRow["Mét khối đăng ký"] = total;
                listExcel.Rows.Add(totalMoneyRow);

                //Xuất file theo format
                HOANHBO_ReportInOutFormatCell(listExcel, excelcol, "Tổng_hợp_khối_lượng_xe_chở_vật_liệu", "Sheet1", "", "Tổng hợp khối lượng xe chở vật liệu", datefrompicker);

                return RedirectToAction("HOANHBO_ReportInOut");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.HOANHBO_GetReportInOut(keyReplace, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize, ref totalItem, ref totalDVT).ToList();
            if (list.Any())
            {
                var str = "";
                var strCardNumber = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                    strCardNumber += item.CardNumber + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();

                ViewBag.DVTs = _tblCardService.GetCardByCardNumbers(strCardNumber);
            }

            var gridModel = PageModelCustom<HOANHBO_ReportInOut>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();
            ViewBag.LaneId = lane;

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.KeyWord = key;
            ViewBag.TotalDVT = totalDVT;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            ViewBag.isFilterByTimeIn = IsFilterByTimeIn;

            return View(gridModel);
            #endregion
        }
        private void HOANHBO_ReportInOutFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportInOut");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + " : " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " : " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeIn"], ItemValue = "Thời gian vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["timeOut"], ItemValue = "Thời gian ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["customer"], ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceIn"], ItemValue = "Làn vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["lanceOut"], ItemValue = "Làn ra" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringIn"], ItemValue = "Giám sát vào" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["monitoringOut"], ItemValue = "Giám sát ra" });
            listColumn.Add(new SelectListModel { ItemText = "Mét khối đăng ký", ItemValue = "Mét khối đăng ký" });

            if (!string.IsNullOrEmpty(excelcol))
            {
                string[] columnNames = dtData.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName)
                               .ToArray();
                if (columnNames.Length > 0)
                {
                    foreach (var dcol in columnNames)
                    {
                        if (!excelcol.Contains(dcol))
                        {
                            dtData.Columns.Remove(dcol);

                            var item = listColumn.FirstOrDefault(n => n.ItemValue.Equals(dcol));

                            if (item != null)
                            {
                                listColumn.Remove(item);
                            }
                        }

                    }
                }
            }

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }

        public JsonResult EditPlate(string Id, string Plate)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            try
            {
                _ReportService.UpdatePlate(Id, Plate);
                result = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception)
            {
                result = new MessageReport(false, "Có lỗi xảy ra");
                throw;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lotte

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportTotalByCardGroup_LOTTE(string cardgroup = "", string chkExport = "0", string fromdate = "", string todate = "")
        {

            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalByCardGroup_LOTTE");

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Giao diện

            var list = _ReportService.TotalByCardGroup(cardgroup, fromdate, todate);

            if (list != null && list.Count > 0)
            {
                var cardGroups = GetCardGroupListNew();

                foreach (var item in list)
                {
                    var objCG = cardGroups.FirstOrDefault(n => n.ItemValue == item.CardGroupID);

                    item.CardGroupID = objCG != null ? objCG.ItemText : "";
                }

                list = list.OrderBy(n => n.CardGroupID).ToList();
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Xuất file theo format
                TotalByCardGroup_LOTTEFormatCell(list, Dictionary["titleEx"], "Sheet1", "", Dictionary["title"], datefrompicker);

                return RedirectToAction("ReportTotalByCardGroup_LOTTE");
            }
            #endregion

            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.VehicleGroup = GetVehicleGroupList();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            return View(list);
            #endregion
        }

        private void TotalByCardGroup_LOTTEFormatCell(List<TotalByCardGroup> dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportTotalMoneyByCardGroup");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardGroup"], ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["quantity"], ItemValue = "Số lượng" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["amount"], ItemValue = "Số tiền" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            var objsystem = _tblSystemConfigService.GetDefault();

            var vehicleGroups = GetVehicleGroupList();

            ExportFile_ByCG_LOTTE(dtData, vehicleGroups, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion


        #region Báo cáo HDĐT
        #region     
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportSuccesApiHDĐT( string sendType = "", string key = "",  string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            //var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            //var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
           
            //var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":","") : String.Empty;
            //GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportSuccesApiHDĐTExcel( key, sendType, fromdate, todate,  page, pageSize);


                //Xuất file theo format
                ReportGetReportSuccesApiHDĐTCell(listExcel, "Danh sách gửi thành công HDDT", "Sheet1", "", "Danh sách gửi thành công HDDT", datefrompicker);

                return RedirectToAction("ReportSuccesApiHDĐT");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportSuccesApiHDĐT( key, sendType, fromdate, todate, page, pageSize, ref totalItem).ToList();
           
            var gridModel = PageModelCustom<tblThaiSonSuccessApi_custom>.GetPage(list, page, pageSize, totalItem);
        
            ViewBag.SendTypeDT = GetSendTypes();
            ViewBag.SendTypeId = sendType;
            ViewBag.KeyWord = key;

            ViewBag.Users = _UserService.GetAllActive().ToList();
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }

        private void ReportGetReportSuccesApiHDĐTCell(List<tblThaiSonSuccessApi_Excel> dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian gửi", ItemValue = "Thời gian gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Mã hóa đơn", ItemValue = "Mã hóa đơn" });
            listColumn.Add(new SelectListModel { ItemText = "Người gửi", ItemValue = "Người gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Hình thức gửi", ItemValue = "Hình thức gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Lần gửi", ItemValue = "Lần gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số xe", ItemValue = "Biển số xe" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian thành công", ItemValue = "Thời gian thành công" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Thành tiền", ItemValue = "Thành tiền" });
            //Chuyển dữ liệu về datatable
            DataTable dt = dtData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region  Danh sách gửi thất bại HĐ ĐT
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportErrorApiHDĐT(string sendType = "", string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            //var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            //var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":","") : String.Empty;
            //GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportErrorApiHDĐTExcel(key, sendType, fromdate, todate, page, pageSize);


                //Xuất file theo format
                ReportGetReportErrorApiHDĐTCell(listExcel, "Danh sách gửi thất bại HDDT", "Sheet1", "", "Danh sách gửi thất bại HDDT", datefrompicker);

                return RedirectToAction("ReportSuccesApiHDĐT");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportErrorApiHDĐT(key, sendType, fromdate, todate, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<tblThaiSonErrorApiTemp_Custom>.GetPage(list, page, pageSize, totalItem);

            ViewBag.SendStatusDT = FunctionHelper.SendStatus();
            ViewBag.SendTypeId = sendType;
            ViewBag.KeyWord = key;

            ViewBag.Users = _UserService.GetAllActive().ToList();
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }

        private void ReportGetReportErrorApiHDĐTCell(List<tblThaiSonErrorApiTemp_Excel> dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Thời gian" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã số thẻ", ItemValue = "Mã số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số xe", ItemValue = "Biển số xe" });
            listColumn.Add(new SelectListModel { ItemText = "Người gửi", ItemValue = "Người gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Thành tiền", ItemValue = "Thành tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });
         
            //Chuyển dữ liệu về datatable
            DataTable dt = dtData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region  Danh sách gửi thủ công HĐ ĐT
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportErrorManualApiHDĐT(string sendType = "", string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1)
        {
            //var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            //var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":","") : String.Empty;
            //GetListChild(strCG, customergroup);

            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportManualApiHDĐTExcel(key, sendType, fromdate, todate, page, pageSize);


                //Xuất file theo format
                ReportGetReportManualApiHDĐTCell(listExcel, "Danh sách gửi thất bại HDDT", "Sheet1", "", "Danh sách gửi thất bại HDDT", datefrompicker);

                return RedirectToAction("ReportSuccesApiHDĐT");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportManualApiHDĐT(key, sendType, fromdate, todate, page, pageSize, ref totalItem).ToList();

            var gridModel = PageModelCustom<tblThaiSonErrorApiTemp_Custom>.GetPage(list, page, pageSize, totalItem);

            ViewBag.SendStatusDT = FunctionHelper.SendStatus();
            ViewBag.SendTypeId = sendType;
            ViewBag.KeyWord = key;

            ViewBag.Users = _UserService.GetAllActive().ToList();
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View(gridModel);
            #endregion
        }

        private void ReportGetReportManualApiHDĐTCell(List<tblThaiSonErrorApiTemp_Excel> dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportVehicleComeIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Thời gian" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã số thẻ", ItemValue = "Mã số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số xe", ItemValue = "Biển số xe" });
            listColumn.Add(new SelectListModel { ItemText = "Người gửi", ItemValue = "Người gửi" });
            listColumn.Add(new SelectListModel { ItemText = "Thành tiền", ItemValue = "Thành tiền" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });

            //Chuyển dữ liệu về datatable
            DataTable dt = dtData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #endregion
        #region DemoFilter

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult DemoFilter(string fromdate = "05/01/2021 00:00:00", string todate = "05/01/2022 23:59:59", int page = 1)
        {

            ViewBag.CardGroups = GetCardGroupListNew().ToList();
            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();

            ViewBag.Lanes = GetLaneList().ToList();
            ViewBag.LaneDT = GetLaneList().ToDataTableNullable();

            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();

            ViewBag.CustomerGroups = GetMenuList();

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;

            return View();
        }

        public PartialViewResult Partial_DemoFilter(string key = "", int page = 1)
        {
            var strCG = new List<string>();

            var totalItem = 0;
            var pageSize = 20;

            var list = _ReportService.DemoFilter(key, page, pageSize, ref totalItem).ToList();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }

            var gridModel = PageModelCustom<ReportInOut>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListNew().ToList();

            ViewBag.Users = GetUserList().ToList();

            return PartialView(gridModel);
        }

        public PartialViewResult Partial_PagingAjax(PageModel model)
        {
            return PartialView(model);
        }
        public JsonResult GetIdByKey(string key, string action)
        {
            string id = _ReportService.GetIdByKey(key, action);

            return Json(id, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public async Task<PartialViewResult> ImageFTP(string filename, string description, string type = "")
        {
            ViewBag.descriptions = description;
            ViewBag.TypeValue = type;

            if (filename.Contains("bienso"))
            {
                ViewBag.TypeValue = "HOAPHAT";
            }

            if (_tblSystemConfigService.GetDefault().FeeName == "BVDK_THANHPHO_VINH")
                ViewBag.L = await FunctionHelper.FtpImage_V5(filename);
            else
                ViewBag.L = await FunctionHelper.FtpImage(filename);

            return PartialView();
        }

        public JsonResult DeleteEventIn(string id)
        {
            var cardnumber = "";
            var user = GetCurrentUser.GetUser();

            var result = new MessageReport();

            if (id.Contains("_CARD"))
            {
                var _id = id.Replace("_CARD", "");

                result = _tblCardEventService.DeleteById(_id, ref cardnumber);

                if (result.isSuccess)
                {
                    WriteLog.Write(result, user, _id, cardnumber, "tblCardEvent", ConstField.ParkingCode, ActionConfigO.Delete);

                    _tblCardEventService.DeleteTempById(_id);
                }
            }
            else
            {
                var _id = id.Replace("_LOOP", "");

                result = _tblLoopEventService.DeleteById(_id, ref cardnumber);

                if (result.isSuccess)
                {
                    WriteLog.Write(result, user, _id, cardnumber, "tblLoopEvent", ConstField.ParkingCode, ActionConfigO.Delete);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEventInOut(string id)
        {
            var cardnumber = "";
            var user = GetCurrentUser.GetUser();

            var result = new MessageReport();

            if (id.Contains("_CARD"))
            {
                var _id = id.Replace("_CARD", "");

                result = _tblCardEventService.DeleteById(_id, ref cardnumber);

                if (result.isSuccess)
                {
                    WriteLog.Write(result, user, _id, cardnumber, "tblCardEvent", ConstField.ParkingCode, ActionConfigO.Delete);
                }
            }
            else
            {
                var _id = id.Replace("_LOOP", "");

                result = _tblLoopEventService.DeleteById(_id, ref cardnumber);

                if (result.isSuccess)
                {
                    WriteLog.Write(result, user, _id, cardnumber, "tblLoopEvent", ConstField.ParkingCode, ActionConfigO.Delete);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PartialExcelColumn(string action)
        {
            var lstValue = new List<string>();
            var list = GetExcelColumnByAction(action);

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    lstValue.Add(item.ItemValue);
                }
            }

            var dt = list.ToDataTableNullable();
            ViewBag.ListValue = lstValue;
            return PartialView(dt);
        }
    }
}