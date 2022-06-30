using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ActiveCardListTRANSERCOController : Controller
    {
        private ItblActiveCardService _tblActiveCardService;
        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCardService _tblCardService;
        private IUserService _UserService;
        private ItblSystemConfigService _tblSystemConfigService;

        public ActiveCardListTRANSERCOController(ItblActiveCardService _tblActiveCardService, ItblCustomerService _tblCustomerService, ItblCustomerGroupService _tblCustomerGroupService, ItblCardGroupService _tblCardGroupService, IUserService _UserService, ItblSystemConfigService _tblSystemConfigService, ItblCardService _tblCardService)
        {
            this._tblActiveCardService = _tblActiveCardService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCardGroupService = _tblCardGroupService;
            this._UserService = _UserService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCardService = _tblCardService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string cardgroupids, string customergroupids, string userids, string fromdate, string todate,string typepay, int page = 1, string chkExport = "0")
        {
            if (string.IsNullOrWhiteSpace(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrWhiteSpace(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (chkExport == "1")
            {
                var listExcel = _tblActiveCardService.ReportAllByFirstTRANSERCO(key,typepay, "", fromdate, todate, cardgroupids, customergroupids, userids);

                //Xuất file theo format
                PK_ActiveCardListFormatCell(listExcel, "Danh_sách_thẻ_đã_gia_hạn", "Sheet1", "", "Danh sách thẻ đã gia hạn", fromdate + " - " + todate);

                return RedirectToAction("Index");
            }

            int pageSize = 20;

            var list = _tblActiveCardService.GetAllPagingByFirstTRANSERCO(key,typepay, "", fromdate, todate, cardgroupids, customergroupids, userids, page, pageSize);
            if (list.Any())
            {
                var strCustomer = new List<string>();
                foreach (var item in list)
                {
                    strCustomer.Add(item.CustomerID);
                }

                ViewBag.CustomerList = _tblCustomerService.GetAllByListId(strCustomer).ToList();
            }

            var gridModel = PageModelCustom<tblActiveCardCustomViewModel>.GetPage(list, page, pageSize);

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupidsValue = cardgroupids;
            ViewBag.customergroupidsValue = customergroupids;
            ViewBag.useridsValue = userids;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.TypePay = FunctionHelper.TypePay();
            ViewBag.TypePayValue = typepay;
            //
            ViewBag.cardgroups = GetListCardGroup().ToDataTableNullable();
            ViewBag.users = GetListUser().ToDataTableNullable();
            ViewBag.customergroups = GetMenuList();

            return View(gridModel);
        }

        private void PK_ActiveCardListFormatCell(List<tblActiveCard_ExcelTRANSERCO> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });           
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Plate" });
            listColumn.Add(new SelectListModel { ItemText = "Mã hợp đồng", ItemValue = "ContractCode" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = "Mã số thuế", ItemValue = "Tax" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày cũ", ItemValue = "OldDate" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày mới", ItemValue = "NewDate" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày chêch lệch", ItemValue = "Days" });
            listColumn.Add(new SelectListModel { ItemText = "Phí", ItemValue = "Money" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày tạo", ItemValue = "DateCreated" });
            listColumn.Add(new SelectListModel { ItemText = "NV thực hiện", ItemValue = "UserName" });
            listColumn.Add(new SelectListModel { ItemText = "Thanh toán", ItemValue = "TransferPaymentValue" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();
            dt.Columns.Remove("IsTransferPayment");
            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
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

        public IEnumerable<tblCustomer> GetListCustomer(string id)
        {
            return null;
        }

        public IEnumerable<tblCardGroup> GetListCardGroup()
        {
            var query = _tblCardGroupService.GetAllActive();
            return query;
        }

        public IEnumerable<User> GetListUser()
        {
            var query = _UserService.GetAllActive();
            return query;
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

        public JsonResult DeleteEvent(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            if (!string.IsNullOrEmpty(id))
            {
                var obj = new tblActiveCard();

                var activecard = _tblActiveCardService.GetById(id);

                if (activecard != null)
                {
                    var card = _tblCardService.GetByCardNumber(activecard.CardNumber);
                    if (card != null)
                    {
                        if (Convert.ToDateTime(card.ExpireDate).Date == Convert.ToDateTime(activecard.NewExpireDate).Date)
                        {
                            card.ExpireDate = activecard.OldExpireDate;
                            _tblCardService.Update(card);
                            result = _tblActiveCardService.DeleteById(id);
                        }
                        else
                        {
                            result = new MessageReport(false, "Không thể xóa");
                        }
                    }
                }

                if (result.isSuccess)
                {
                    WriteLog.Write(result, GetCurrentUser.GetUser(), id, "", "tblActiveCard", ConstField.ParkingCode, ActionConfigO.Delete);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}