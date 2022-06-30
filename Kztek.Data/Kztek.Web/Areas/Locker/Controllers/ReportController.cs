using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.LockerEvent;
using Kztek.Service;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class ReportController : Controller
    {
        #region Service
        private IReportService _ReportService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCustomerService _tblCustomerService;
        private ItblCardService _tblCardService;
        private ItblLockerControllerService _tblLockerControllerService;


        public ReportController(IReportService _ReportService, ItblCardGroupService _tblCardGroupService, ItblSystemConfigService _tblSystemConfigService, ItblCustomerGroupService _tblCustomerGroupService, ItblCustomerService _tblCustomerService, ItblCardService _tblCardService, ItblLockerControllerService _tblLockerControllerService)
        {
            this._ReportService = _ReportService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCardService = _tblCardService;
            this._tblLockerControllerService = _tblLockerControllerService;
        }

        // GET: Locker/Report
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Báo cáo sự kiện tủ đồ

        public ActionResult ReportLockerEvent(string key = "", string lockercontrol = "", string type = "", string cardgroup = "", string fromdate = "", string todate = "", string chkExport = "0", int page = 1)
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
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportLockerEvent_Excel(key, lockercontrol, type, cardgroup, fromdate, todate, ref totalItem);

                //Xuất file theo format
                ReportLockerEventFormatCell(listExcel, "", "Báo_cáo_sự_kiện_tủ_đồ", "Sheet1", "", "Báo cáo sự kiện tủ đồ", datefrompicker);

                return RedirectToAction("ReportLockerEvent");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportLockerEvent(key, lockercontrol, type, cardgroup, fromdate, todate, page, pageSize, ref totalItem);

            var gridModel = PageModelCustom<tblLockerEvent_Report>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroupDT = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.ControllerDT = GetControllerList().ToDataTableNullable();
            ViewBag.ControllerId = lockercontrol;

            ViewBag.TypeDT = FunctionHelper.TypeEventLocker().ToDataTableNullable();
            ViewBag.Type = type;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            #endregion

            return View(gridModel);
        }

        #region Format cell lên excel
        private void ReportLockerEventFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ ngày" + ": " + titleTime.Split(new[] { '-' })[0] + " - " + "Đến ngày" + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Ngăn", ItemValue = "LockerIndex" });
            listColumn.Add(new SelectListModel { ItemText = "Bộ điều khiển", ItemValue = "Bộ điều khiển" });
            listColumn.Add(new SelectListModel { ItemText = "Loại sự kiện", ItemValue = "Loại sự kiện" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày tạo", ItemValue = "Ngày tạo" });

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion
        #endregion

        #region báo cáo thao tác tủ đồ
        /// <summary>
        /// View báo cáo thao tác tủ đồ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardgroup"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="chkExport"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ReportActionHistoryLoker(string key = "", string objcontrol = "", string fromdate = "", string todate = "", string actionLooker = "", string type = "", string chkExport = "0", int page = 1)

        {
                // var totalPage = 0;
                var totalItem = 0;
                var pageSize = 20;

                var datefrompicker = "";

                if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                {
                    datefrompicker = fromdate + "-" + todate;
                }
                #region Excel
                //Excel
                if (chkExport.Equals("1"))
                {
                    //Query lấy dữ liệu
                    var listExcel = _ReportService.GetReportActionHistoryLoker_Excel(key, objcontrol, fromdate, todate, actionLooker, type ,ref totalItem);

                    //Xuất file theo format
                    ReportActionHistoryLokerFormatCell(listExcel, "", "Báo_cáo_lịch_sử_thao_tác_tủ_đồ", "Sheet1", "", "Báo cáo lịch sử thao tác tủ đồ", datefrompicker);

                    return RedirectToAction("ReportLockerEvent");
                }
                #endregion

                #region Giao diện

                var list = _ReportService.ReportActionHistoryLoker(key, objcontrol, fromdate, todate, actionLooker, type, page, pageSize, ref totalItem);

                var gridModel = PageModelCustom<ReportLockerProcess>.GetPage(list, page, pageSize, totalItem);

                ViewBag.KeyWord = key;
                ViewBag.fromdateValue = fromdate;
                ViewBag.todateValue = todate;
                ViewBag.ControllerDT = GetControllerList().ToDataTableNullable();
                ViewBag.ControllerId = objcontrol;
                ViewBag.ActionLst = FunctionHelper.ActionLockerProcess();
                ViewBag.ActionValue = actionLooker;
                ViewBag.TypeLockerProcessLst = FunctionHelper.TypeLockerProcess();
                ViewBag.typeValue = type;
                #endregion

            return View(gridModel);
            }

        /// <summary>
        /// Format xuất excel báo cáo thao tác tủ đồ
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="excelcol"></param>
        /// <param name="filename"></param>
        /// <param name="sheetname"></param>
        /// <param name="comments"></param>
        /// <param name="titleSheet"></param>
        /// <param name="titleTime"></param>
        private void ReportActionHistoryLokerFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
            {

                //Nội dung đầu trang
                var user = GetCurrentUser.GetUser();

            var timeSearch = "";
            if (!String.IsNullOrEmpty(titleTime))
                timeSearch = "Từ ngày" + ": " + titleTime.Split(new[] { '-' })[0] + " - " + "Đến ngày" + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

                //Header
                var listColumn = new List<SelectListModel>();
                listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
                listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "CardNo" });
                listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
                listColumn.Add(new SelectListModel { ItemText = "Tên tủ đồ", ItemValue = "Tên tủ đô" });
                listColumn.Add(new SelectListModel { ItemText = "Người dùng", ItemValue = "Người dùng" });
                listColumn.Add(new SelectListModel { ItemText = "Bộ điều khiển", ItemValue = "Bộ điều khiển" });
                listColumn.Add(new SelectListModel { ItemText = "Thao tác", ItemValue = "Thao tác" });
                listColumn.Add(new SelectListModel { ItemText = "Ngày thao tác", ItemValue = "Ngày thao tác" });

                //Xuất file
                ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
            }
        #endregion

        #region báo cáo Cảnh báo tủ đồ 
        public ActionResult ReportLokerAlarm(string key = "", string objcontrol = "",string CardGroupID ="", string fromdate = "", string todate = "",string lockerAlarmCode = "", string lockerEventCode = "",string lockerEventType = "", string chkExport = "0", int page = 1)

        {
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var datefrompicker = "";

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportLokerAlarm_Excel(key, objcontrol, lockerEventType, CardGroupID, lockerAlarmCode, lockerEventCode, fromdate, todate, page, pageSize, ref totalItem);

                //Xuất file theo format
                ReportLokerAlarm_FormatCell(listExcel, "", "Danh_sách_sự_kiện_cảnh_báo", "Sheet1", "", "Danh sách sự kiện cảnh báo", datefrompicker);

                return RedirectToAction("ReportLokerAlarm");
            }
            #endregion

            #region Giao diện

            var list = _ReportService.ReportLokerAlarm(key, objcontrol, lockerEventType,  CardGroupID, lockerAlarmCode, lockerEventCode, fromdate, todate,  page, pageSize, ref totalItem);
 
            var gridModel = PageModelCustom<tblLockerAlarmReport>.GetPage(list, page, pageSize, totalItem);

            ViewBag.KeyWord = key;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.controllerDT = GetControllerList().ToDataTableNullable();
            ViewBag.controllerId = objcontrol;
            ViewBag.lockerEventCodeLst = FunctionHelper.LockerEventCode();
            ViewBag.lockerEventCode = lockerEventCode;
            ViewBag.lockerEventTypeLst = FunctionHelper.LockerEventType();
            ViewBag.lockerEventType = lockerEventType;
            ViewBag.lockerAlarmCodeLst = FunctionHelper.LockerAlarmCode();
            ViewBag.lockerAlarmCode = lockerAlarmCode;
            ViewBag.CardGroupLst = GetCardGroupListNew().ToDataTableNullable();
            ViewBag.CardGroupId = CardGroupID;
            #endregion

            return View(gridModel);
        }

        /// <summary>
        /// Format xuất excel báo cáo cảnh báo tủ đồ
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="excelcol"></param>
        /// <param name="filename"></param>
        /// <param name="sheetname"></param>
        /// <param name="comments"></param>
        /// <param name="titleSheet"></param>
        /// <param name="titleTime"></param>
        private void ReportLokerAlarm_FormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";
            if (!String.IsNullOrEmpty(titleTime))
             timeSearch = "Từ ngày" + ": " + titleTime.Split(new[] { '-' })[0] + " - " + "Đến ngày" + ": " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Tên tủ đô" });
            listColumn.Add(new SelectListModel { ItemText = "Bộ điều khiển", ItemValue = "Bộ điều khiển" });
            listColumn.Add(new SelectListModel { ItemText = "Ngăn", ItemValue = "Vị trí tủ" });
            listColumn.Add(new SelectListModel { ItemText = "Sự kiện", ItemValue = "Sự kiện" });
            listColumn.Add(new SelectListModel { ItemText = "Loại sự kiện", ItemValue = "Kiểu sự kiện" });
            listColumn.Add(new SelectListModel { ItemText = "Cảnh báo", ItemValue = "Alarm" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày thao tác", ItemValue = "Ngày thao tác" });

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Danh sách sử dụng

        public async Task<PartialViewResult> ImageFTP(string filename, string description, string type = "")
        {
            ViewBag.descriptions = description;
            ViewBag.TypeValue = type;

            ViewBag.L = await FunctionHelper.FtpImage(filename);

            return PartialView();
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

        public List<SelectListModel2> GetControllerList()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel2>();
            var listcontroller = _tblLockerControllerService.GetAll().ToList();
            if (listcontroller.Any())
            {
                foreach (var item in listcontroller)
                {
                    list.Add(new SelectListModel2 { ItemValue = item.Id.ToString(), ItemText = item.ControllerName });
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

        #endregion
    }
}