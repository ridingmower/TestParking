using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using Kztek.Service;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblCardEventDeleteController : Controller
    {

        private ItblCardEventDeleteService _tblCardEventDeleteService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private IUserService _UserService;
        private ItblLaneService _tblLaneService;
        private IReportService _ReportService;
        private ItblSystemConfigService _tblSystemConfigService;

        public tblCardEventDeleteController(ItblCardEventDeleteService _tblCardEventDeleteService, IReportService _ReportService, ItblCardGroupService _tblCardGroupService, ItblCustomerGroupService _tblCustomerGroupService, IUserService _UserService, ItblLaneService _tblLaneService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblCardEventDeleteService = _tblCardEventDeleteService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._UserService = _UserService;
            this._tblLaneService = _tblLaneService;
            this._ReportService = _ReportService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        #region danh sách sử dụng
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

        private IEnumerable<tblLane> GetLaneList()
        {
            return _tblLaneService.GetAll();
        }

        private IEnumerable<User> GetUserList()
        {
            return _UserService.GetAll();
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
        #endregion

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", string fromdate = "", string todate = "", int page = 1, string chkExport = "0")
        {
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

            var list = _tblCardEventDeleteService.GetAllSql(key, fromdate, todate, page, pageSize, ref totalItem);

            var gridModel = PageModelCustom<tblCardEventDeleteCustom>.GetPage(list, page, pageSize, totalItem);

            ViewBag.keyValue = key;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            return View(gridModel);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult DeleteEvent(string customergroup = "", string number = "0", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string excelcol = "", bool IsFilterByTimeIn = false, string fromdate = "", string todate = "", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);
            var host = Request.Url.Host;
            var totalItem = 0;
            decimal totalMoney = 0;
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


            #region Giao diện
            var list = _tblCardEventDeleteService.GetListEventInOut(key, number, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize, ref totalItem, ref totalMoney).ToList();
            var listid = new List<string>();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                    listid.Add(item.Id);
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportInOut108>.GetPage(list, page, pageSize, totalItem);

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
            ViewBag.Number = number;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            ViewBag.isFilterByTimeIn = IsFilterByTimeIn;

            Session[string.Format("{0}_{1}", SessionConfig.EventIdDelete108ParkingSession, host)] = new List<string>();
            ViewBag.selectedEventValue = GetSetFromSession(listid);
            ViewBag.TotalEventDelete = _tblCardEventDeleteService.GetTotalMoney(key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup);
            return View(gridModel);
            #endregion
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult HistoryDeleteEvent(string customergroup = "", string number = "0", string key = "", string user = "", string cardgroup = "", string lane = "", string chkExport = "0", string excelcol = "", bool IsFilterByTimeIn = false, string fromdate = "", string todate = "", int page = 1, int pageSize = 20)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);
            var host = Request.Url.Host;
            var totalItem = 0;
            decimal totalMoney = 0;


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
                var listExcel = _tblCardEventDeleteService.GetHistoryDeleteEvent_Excel(key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, ref totalItem);

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
                    var _user = _UserService.GetById(item["Người xóa"].ToString());
                    if (_user != null)
                    {
                        item["Người xóa"] = _user.Username;
                    }
                    else
                    {
                        item["Người xóa"] = "";
                    }
                }

                //Xuất file theo format
                HistoryDeleteEventFormatCell(listExcel, excelcol, "Lịch_sử_xóa_sự_kiện", "Sheet1", "", "Lịch sử xóa sự kiện", datefrompicker);

                return RedirectToAction("HistoryDeleteEvent");
            }
            #endregion

            #region Giao diện
            var list = _tblCardEventDeleteService.GetHistoryDeleteEvent(key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup, page, pageSize, ref totalItem).ToList();
            var listid = new List<string>();
            if (list.Any())
            {
                var str = "";
                foreach (var item in list)
                {
                    str += item.LaneIDIn + "," + item.LaneIDOut + ",";
                    listid.Add(item.Id);
                }

                ViewBag.LaneList = _tblLaneService.GetAllActiveByListId(str).ToList();
            }


            var gridModel = PageModelCustom<ReportInOut108>.GetPage(list, page, pageSize, totalItem);

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
            ViewBag.Number = number;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("dd/MM/yyyy 23:59") : todate;

            ViewBag.isFilterByTimeIn = IsFilterByTimeIn;

            //Session[string.Format("{0}_{1}", SessionConfig.EventIdDelete108ParkingSession, host)] = new List<string>();
            //ViewBag.selectedEventValue = GetSetFromSession(listid);
            ViewBag.TotalMoney = _tblCardEventDeleteService.GetTotalMoney(key, strCG, IsFilterByTimeIn, fromdate, todate, cardgroup, lane, user, customergroup);
            return View(gridModel);
            #endregion
        }

        private void HistoryDeleteEventFormatCell(DataTable dtData, string excelcol, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày xóa", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Người xóa", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Số tiền", ItemValue = "Biển số" });
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
            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }

        public JsonResult AddOrRemoveOneAllSeleted(List<string> Id, bool isAdd)
        {
            GetSetFromSession(Id, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSession(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listEventId = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.EventIdDelete108ParkingSession, host)];
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

            Session[string.Format("{0}_{1}", SessionConfig.EventIdDelete108ParkingSession, host)] = listEventId;

            return listEventId;
        }

        public PartialViewResult ModalSelectedEvent(int totalItem = 0, string url = "")
        {
            var listSelected = GetSetFromSession(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listSelected);
        }
        public async Task<PartialViewResult> ImageFTP(string filename, string description, string type = "")
        {
            ViewBag.descriptions = description;
            ViewBag.TypeValue = type;

            ViewBag.L = await FunctionHelper.FtpImage(filename);

            return PartialView();
        }

        public JsonResult SaveDeleteEvent()
        {
            var host = Request.Url.Host;

            var result = new MessageReport(false, "Có lỗi xảy ra");

            var list = GetSetFromSession(null);

            if (list != null && list.Count > 0)
            {
                var userid = GetCurrentUser.GetUser().Id;
                var check = _tblCardEventDeleteService.DeleteEvent(list, userid);
                if (check)
                {
                    result = new MessageReport(true, "Xóa thành công");

                    Session[string.Format("{0}_{1}", SessionConfig.EventIdDelete108ParkingSession, host)] = new List<string>();
                }
                else
                {
                    result = new MessageReport(false, "Có lỗi xảy ra");
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RestoreDeleteEvent(string customergroup = "", string key = "", string user = "", string cardgroup = "", string lane = "", string fromdate = "", string todate = "", string rowid = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            var check = _tblCardEventDeleteService.RestoreDeleteEvent(key, strCG, fromdate, todate, cardgroup, lane, user, rowid);

            if (check)
            {
                result = new MessageReport(true, "Khôi phục thành công");
            }
            else
            {
                result = new MessageReport(false, "Có lỗi xảy ra");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialDelete(string number)
        {

            return PartialView();
        }

        public JsonResult GetCountEvent()
        {
            var listSelected = GetSetFromSession(null);
            var a = listSelected.Count();
            return Json(a, JsonRequestBehavior.AllowGet);
        }

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
    }
}