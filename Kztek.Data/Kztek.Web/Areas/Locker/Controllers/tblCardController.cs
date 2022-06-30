using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class tblCardController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblLockerService _tblLockerService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;

        private ItblSystemConfigService _tblSystemConfigService;

        private ItblCardEventService _tblCardEventService;

        private ItblAccessLevelService _tblAccessLevelService;
        private ItblAccessLevelDetailService _tblAccessLevelDetailService;
        private ItblAccessUploadDetailService _tblAccessUploadDetailService;

        private ItblAccessControllerService _tblAccessControllerService;

        public tblCardController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerService _tblCustomerService, ItblCustomerGroupService _tblCustomerGroupService, ItblSystemConfigService _tblSystemConfigService, ItblCardEventService _tblCardEventService, ItblAccessLevelService _tblAccessLevelService, ItblAccessLevelDetailService _tblAccessLevelDetailService, ItblAccessUploadDetailService _tblAccessUploadDetailService, ItblAccessControllerService _tblAccessControllerService, ItblLockerService _tblLockerService)
        {
            this._tblCardService = _tblCardService;
            this._tblLockerService = _tblLockerService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCustomerGroupService = _tblCustomerGroupService;

            this._tblSystemConfigService = _tblSystemConfigService;

            this._tblCardEventService = _tblCardEventService;

            this._tblAccessLevelService = _tblAccessLevelService;
            this._tblAccessLevelDetailService = _tblAccessLevelDetailService;
            this._tblAccessUploadDetailService = _tblAccessUploadDetailService;

            this._tblAccessControllerService = _tblAccessControllerService;
        }

        private static string keyV = "";
        private static string cardgroupsV = "";
        private static string customeridV = "";
        private static string strCGV = "";
        private static string activeV = "";
        private static string fromdateV = "";
        private static string todateV = "";
        private static string isCheckByTimeV = "";
        private static string levelsV = "";

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string cardgroups, string customerid, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1, string chkExport = "0", string selectedId = "", string levels = "", bool desc = true, string columnQuery = "ImportDate")
        {
            var str = GetListChild("", customergroups);

            if (chkExport == "1")
            {
                var listExcel = _tblCardService.GetLockerExcelCardByFirstTSQL(key, cardgroups, customerid, str, fromdate, todate, desc, columnQuery, isCheckByTime);

                //Xuất file theo format
                PK_CardCustomerFormatCell(listExcel, "Danh_sách_thẻ", "Sheet1", "", "Danh sách thẻ", fromdate + " - " + todate);

                return RedirectToAction("Index");
            }

            int pageSize = 20;
            int total = 0;

            var list = _tblCardService.GetAllPagingByFirstTSQL_Locker(key, cardgroups, customerid, str, fromdate, todate,desc, columnQuery, ref total, isCheckByTime, page, pageSize, levels);

            var gridModel = PageModelCustom<tblCardCustomViewModel>.GetPage(list, page, pageSize, total);

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.levelsValue = levels;
            ViewBag.customeridValue = customerid;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;
            ViewBag.columnQueryValue = columnQuery;
            ViewBag.DescValue = desc;

            //
            ViewBag.selectedIdValue = selectedId;
            ViewBag.selectedCardValue = GetSetFromSession(null);

            //
            ViewBag.lcardgroups = GetListCardGroup().ToDataTableNullable();
            ViewBag.llevelDTs = GetListAccessLevel().ToDataTableNullable();
            ViewBag.llevels = GetListAccessLevel();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.lactives = FunctionHelper.CardStatus();

            //Dùng cho phân quyền nhiều
            keyV = key;
            cardgroupsV = cardgroups;
            customeridV = customerid;
            strCGV = str;
            activeV = active;
            fromdateV = fromdate;
            todateV = todate;
            isCheckByTimeV = isCheckByTime;
            levelsV = levels;

            return View(gridModel);
        }

        private void PK_CardCustomerFormatCell(List<tblLockerCardExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
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
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày hết hạn", ItemValue = "DateExpire" });
            listColumn.Add(new SelectListModel { ItemText = "Mã khách hàng", ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = "CMT", ItemValue = "CMT" });
            listColumn.Add(new SelectListModel { ItemText = "Số điện thoại", ItemValue = "SĐT" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = "Tủ đồ", ItemValue = "LockerName" });
            listColumn.Add(new SelectListModel { ItemText = "Sử dụng", ItemValue = "Inactive" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày tạo", ItemValue = "DateCreated" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

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

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0")
        {
            //
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.llevels = GetListAccessLevel();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            return View();
        }
        [HttpPost]
        public ActionResult Create(tblCardSubmit obj, HttpPostedFileBase FileUpload, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", bool SaveAndCountinue = false)
        {
            //
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.llevels = GetListAccessLevel();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            var map = new tblCard()
            {
                CardID = Guid.NewGuid(),
                CardNo = obj.CardNo,
                CardNumber = obj.CardNumber,
                CardGroupID = obj.CardGroupID,
                CustomerID = GetOrSetCustomer(obj, FileUpload),
                AccessLevelID = "",
                ChkRelease = false,
                ImportDate = DateTime.Now,
                DateRegister = Convert.ToDateTime(obj.DtpDateRegisted),
                DateRelease = Convert.ToDateTime(obj.DtpDateReleased),
                ExpireDate = Convert.ToDateTime(obj.DtpDateExpired),
                DateActive = Convert.ToDateTime(obj.DtpDateActive),
                Description = !string.IsNullOrWhiteSpace(obj.CardDescription) ? obj.CardDescription : "",
                IsDelete = false,
                IsLock = obj.CardInActive,
                //Plate1 = obj.Plate1,
                //Plate2 = obj.Plate2,
                //Plate3 = obj.Plate3,
                //VehicleName1 = obj.VehicleName1,
                //VehicleName2 = obj.VehicleName2,
                //VehicleName3 = obj.VehicleName3,

                DateCancel = DateTime.Now,
                AccessExpireDate = Convert.ToDateTime("2099/12/31")
            };

            //Thực hiện thêm mới
            var result = _tblCardService.Create(map);
            if (result.isSuccess)
            {
                //Log for hệ thống
                WriteLog.Write(result, GetCurrentUser.GetUser(), map.CardID.ToString(), obj.CardNumber, "tblCard", ConstField.LockerCode, ActionConfigO.Create);

                //Upload file vào folder chứa ảnh /upload/avatar
                UploadFile(FileUpload);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID });
                }

                return RedirectToAction("Index", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1)
        {
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.llevels = GetListAccessLevel();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            ViewBag.PN = page;

            var obj = _tblCardService.GetCustomById(Guid.Parse(id));
            if (obj != null)
            {
                var t = _tblCardService.GetById(Guid.Parse(id));
                if (t != null)
                {
                    obj.DtpDateExpired = Convert.ToDateTime(t.ExpireDate).ToString("dd/MM/yyyy");

                    obj.DtpDateRegisted = Convert.ToDateTime(t.DateRegister != null ? t.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.DtpDateReleased = Convert.ToDateTime(t.DateRelease != null ? t.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");

                    obj.DtpDateActive = Convert.ToDateTime(t.DateActive != null ? t.DateActive : DateTime.Now).ToString("dd/MM/yyyy");

                    obj.OldDtpDateRegisted = Convert.ToDateTime(t.DateRegister != null ? t.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.OldDtpDateReleased = Convert.ToDateTime(t.DateRelease != null ? t.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.OldDtpDateActive = Convert.ToDateTime(t.DateActive != null ? t.DateActive : DateTime.Now).ToString("dd/MM/yyyy");
                }
            }

            return View(obj);
        }
        [HttpPost]
        public ActionResult Update(tblCardSubmit obj, HttpPostedFileBase FileUpload, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1)
        {
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.llevels = GetListAccessLevel();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            ViewBag.PN = page;

            var oldObj = _tblCardService.GetCustomById(Guid.Parse(obj.CardID));
            if (oldObj == null)
            {
                return View(obj);
            }

            //Kiểm tra trùng mã thẻ
            var existedCard = _tblCardService.GetByCardNumber_Id(obj.CardNumber, Guid.Parse(obj.CardID));
            if (existedCard != null)
            {
                ModelState.AddModelError("CardNumber", "Mã thẻ đã tồn tại");
                return View(obj);
            }

            //Check action
            #region Check action in card

            #endregion

            //Upload File
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                oldObj.CustomerAvatar = string.Format("{0}{1}", url, fileName);
            }

            //Gán giá trị
            var result = new MessageReport();

            var map = _tblCardService.GetById(Guid.Parse(obj.CardID));
            if (map != null)
            {
                //Thẻ
                map.CardNo = obj.CardNo;
                map.CardGroupID = obj.CardGroupID;
                map.Description = !string.IsNullOrWhiteSpace(obj.CardDescription) ? obj.CardDescription : "";
                map.IsLock = obj.CardInActive;
                //map.Plate1 = obj.Plate1;
                //map.Plate2 = obj.Plate2;
                //map.Plate3 = obj.Plate3;
                //map.VehicleName1 = obj.VehicleName1;
                //map.VehicleName2 = obj.VehicleName2;
                //map.VehicleName3 = obj.VehicleName3;

                //Khách hàng
                obj.CustomerAvatar = oldObj.CustomerAvatar;
                map.CustomerID = GetOrSetCustomer(obj, FileUpload);

                //Ngày giờ
                map.DateRegister = Convert.ToDateTime(obj.DtpDateRegisted);
                map.DateRelease = Convert.ToDateTime(obj.DtpDateReleased);

                //map.AccessLevelID = obj.AccessLevelID;

                result = _tblCardService.Update(map);
            }

            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                //Log for hệ thống
                WriteLog.Write(result, cuuser, oldObj.CardID.ToString(), oldObj.CardNumber, "tblCard", ConstField.LockerCode, ActionConfigO.Update);

                //Upload file vào folder chứa ảnh /upload/avatar
                UploadFile(FileUpload);

                //Process
                var list = GetListActionType(obj);
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        SaveCardProcess(map, item, cuuser.Id);
                    }
                }

                return RedirectToAction("Index", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID, page = page });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = _tblCardService.GetById(Guid.Parse(id));
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ không tồn tại trong hệ thống";
                result1.isSuccess = false;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            var existedInEvent = _tblCardEventService.GetAllByCardNumber(obj.CardNumber);
            if (existedInEvent.Any())
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ đang tồn tại trong sự kiện. Không thể xóa";
                result1.isSuccess = false;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            var result = _tblCardService.DeleteById(id);
            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                WriteLog.Write(result, cuuser, obj.CardID.ToString(), obj.CardNumber, "tblCard", ConstField.LockerCode, ActionConfigO.Delete);

                SaveCardProcess(obj, "DELETE", cuuser.Id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<tblCardGroup> GetListCardGroup()
        {
            var query = _tblCardGroupService.GetAll();
            return query;
        }

        public IEnumerable<tblAccessLevel> GetListAccessLevel()
        {
            return _tblAccessLevelService.GetAllActive();
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
            var MenuList = _tblCustomerGroupService.GetAll().ToList();
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
                    str += "'" + id + "'" + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += "'" + item.CustomerGroupID.ToString() + "'" + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }

        public JsonResult PreviewImageUpload()
        {
            var str = "";

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    str = FunctionHelper.ConvertImgFileUploadToBase64(file);
                }
            }

            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListCardByKey(string key)
        {
            var listString = new List<string>();

            var list = _tblCardService.GetAllActiveByKey(key);

            foreach (var item in list)
            {
                listString.Add(string.Format("{0}", item.CardNumber));
            }

            return Json(listString.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCard(string code)
        {
            var obj = _tblCardService.GetByCardNumber(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListCustomerByKey(string key)
        {
            var cus = new List<SelectListModelAutocomplete>();

            var list = _tblCustomerService.GetAllActiveByKeyMaxTake(key, 10);

            var str = "";
            foreach (var item in list)
            {
                str += item.CustomerGroupID + ",";
            }

            var lCuGr = _tblCustomerGroupService.GetAllActiveByListId(str).ToList();

            foreach (var item in list)
            {
                var cuG = lCuGr.FirstOrDefault(n => n.CustomerGroupID.ToString() == item.CustomerGroupID);

                var t = new SelectListModelAutocomplete();

                t.id = item.CustomerCode;
                t.name = string.Format("{0} - {1} - {2} - {3}", item.CustomerName, item.CustomerCode, item.Mobile, cuG != null ? cuG.CustomerGroupName : "");
                t.value = string.Format("{0} - {1} - {2} - {3}", item.CustomerName, item.CustomerCode, item.Mobile, cuG != null ? cuG.CustomerGroupName : "");

                cus.Add(t);
            }
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        private void UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                string error = "";

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];

                Common.UploadFile(out error, Server.MapPath(url), fileUpload);
            }
        }

        private string GetOrSetCustomer(tblCardSubmit obj, HttpPostedFileBase FileUpload)
        {
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                obj.CustomerAvatar = string.Format("{0}{1}", url, fileName);
            }

            var id = "";

            if (!string.IsNullOrWhiteSpace(obj.CustomerID))
            {
                var tCus = _tblCustomerService.GetById(Guid.Parse(obj.CustomerID));
                if (tCus != null)
                {
                    id = tCus.CustomerID.ToString();

                    tCus.CustomerCode = obj.CustomerCode;
                    tCus.Address = obj.CustomerAddress;
                    tCus.Avatar = obj.CustomerAvatar;
                    tCus.CustomerGroupID = obj.CustomerGroupID;
                    tCus.CustomerName = obj.CustomerName;
                    tCus.Mobile = obj.CustomerMobile;
                    tCus.IDNumber = obj.CustomerIdentify;

                    var result = _tblCustomerService.Update(tCus);
                }
                else
                {
                    tCus = new tblCustomer()
                    {
                        CustomerID = Guid.NewGuid(),
                        Address = obj.CustomerAddress,
                        Avatar = obj.CustomerAvatar,
                        AccessLevelID = "",
                        CompartmentId = "",
                        CustomerCode = obj.CustomerCode,
                        CustomerGroupID = obj.CustomerGroupID,
                        CustomerName = obj.CustomerName,
                        Description = obj.CustomerAddress,
                        IDNumber = obj.CustomerIdentify,
                        Inactive = false,
                        Mobile = obj.CustomerMobile,
                        SortOrder = 0,
                    };

                    var result = _tblCustomerService.Create(tCus);
                    if (result.isSuccess)
                    {
                        id = tCus.CustomerID.ToString();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(obj.CustomerCode))
                {
                    var objC = _tblCustomerService.GetByCode(obj.CustomerCode);

                    if (objC != null)
                    {
                        id = objC.CustomerID.ToString();
                    }
                    else
                    {
                        var tCus = new tblCustomer()
                        {
                            CustomerID = Guid.NewGuid(),
                            Address = obj.CustomerAddress,
                            Avatar = obj.CustomerAvatar,
                            AccessLevelID = "",
                            CompartmentId = "",
                            CustomerCode = obj.CustomerCode,
                            CustomerGroupID = obj.CustomerGroupID,
                            CustomerName = obj.CustomerName,
                            Description = obj.CustomerAddress,
                            IDNumber = obj.CustomerIdentify,
                            Inactive = false,
                            Mobile = obj.CustomerMobile,
                            SortOrder = 0,
                            Finger1 = "",
                            Finger2 = "",
                            DevPass = "",
                            AccessExpireDate = Convert.ToDateTime("2099/12/31")
                        };

                        var result = _tblCustomerService.Create(tCus);
                        if (result.isSuccess)
                        {
                            id = tCus.CustomerID.ToString();
                        }
                    }
                }
            }

            return id;
        }

        private List<string> GetListActionType(tblCardSubmit obj)
        {
            //Đổi thẻ
            if (obj.CardNumber != obj.OldCardNumber)
            {
                obj.isChangeCard = true;
            }

            //Khóa thẻ, mở thẻ


            //Phát thẻ
            if (string.IsNullOrWhiteSpace(obj.OldCustomerCode) && !string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                obj.isReleaseCard = true;
                obj.isChangeCustomer = false;
            }

            //Đổi khách hàng
            if (!string.IsNullOrWhiteSpace(obj.OldCustomerCode) && (!obj.OldCustomerCode.Equals(obj.CustomerCode) || !string.IsNullOrWhiteSpace(obj.CustomerCode)) && (obj.CustomerID != obj.OldCustomerID))
            {
                obj.isChangeCustomer = true;
                obj.isReturnCard = false;
            }

            //Trả thẻ
            if (!string.IsNullOrWhiteSpace(obj.OldCustomerCode) && string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                obj.isReturnCard = true;
            }

            if (obj.OldDtpDateActive != obj.DtpDateActive)
            {
                obj.isChangeActiveCard = true;
            }

            ////
            var str = new List<string>();

            //Xử lý với thẻ
            if (obj.isChangeCard)
            {
                //Cấp mới
                str.Add("ADD");//1
            }
            else
            {
                //if (objMap.isModifiedCard)
                //{
                //    //Sửa thông tin thẻ
                //    str += 8 + ",";
                //}
            }

            //Xử lý với khách hàng
            if (obj.isChangeCustomer)
            {
                //Cấp lại
                str.Add("CHANGE");
            }
            else
            {
                //if (objMap.isModifiedCustomer)
                //{
                //    //Sửa thông tin khách hàng
                //    str += 7 + ",";
                //}
            }

            //Xử lý với thông tin cơ bản
            //if (objMap.isModifiedBaseInfo)
            //{
            //    //Sửa thông tin cơ bản
            //    str += 6 + ",";
            //}

            //Xử lý với ngày gia hạn
            //if (objMap.isModifiedExtendCard)
            //{
            //    //Gia hạn thẻ
            //    str += 3 + ",";
            //}

            //Xử lý với phương tiện xe
            //if (objMap.isModifiedVehicle)
            //{
            //    //Thay đổi thông tin xe
            //    str += 5 + ",";
            //}

            if (obj.isReturnCard)
            {
                //Trả thẻ
                str.Add("RETURN");//10
            }

            //Phát thẻ
            if (obj.isReleaseCard)
            {
                str.Add("RELEASE");//11
            }

            //Hoạt động thẻ
            if (obj.isChangeActiveCard)
            {
                str.Add("ACTIVE");
            }

            if (obj.OldCardInActive != obj.CardInActive)
            {
                if (obj.CardInActive)
                {
                    str.Add("LOCK");
                }
                else
                {
                    str.Add("UNLOCK");
                }
            }

            return str;
        }

        private void SaveCardProcess(tblCard obj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

        private void SaveCardProcess(tblCardSubmit obj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

        private void SaveCardExtendProcess(tblCardSubmit obj, string _newexpire, string userid)
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("VALUES (");

            sb.AppendLine(string.Format("'{0}'", obj.CustomerCode));
            sb.AppendLine(", GETDATE()");
            sb.AppendLine(string.Format(", '{0}'", obj.CardNumber));
            sb.AppendLine(string.Format(", '{0}'", obj.CardNo));
            sb.AppendLine(string.Format(", '{0}'", obj.Plate1 + ";" + obj.Plate2 + ";" + obj.Plate3));
            sb.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(obj.DtpDateExpired).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", DATEDIFF(DAY, '{0}', '{1}')", Convert.ToDateTime(obj.DtpDateExpired).ToString("yyyy/MM/dd"), Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", '{0}'", obj.CardGroupID));
            sb.AppendLine(string.Format(", '{0}'", obj.CustomerGroupID));
            sb.AppendLine(string.Format(", '{0}'", userid));
            sb.AppendLine(string.Format(", '{0}'", "0"));
            sb.AppendLine(string.Format(", '{0}'", obj.CustomerID));
            sb.AppendLine(", 0");

            sb.AppendLine(")");

            //Update card
            sb.AppendLine("UPDATE tblCard");
            sb.AppendLine(string.Format("SET ExpireDate = '{0}'", Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format("WHERE CardNumber = '{0}'", obj.CardNumber));

            ExcuteSQL.Execute(sb.ToString());
        }

        public JsonResult GetCustomer(string code)
        {
            var obj = _tblCustomerService.GetByCode(code);
            if (obj != null)
            {
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var obj1 = new tblCustomerSubmit();
                obj1.CustomerID = "";
                obj1.CustomerCode = "";
                obj1.CustomerName = "";
                obj1.IDNumber = "";
                obj1.Mobile = "";
                obj1.CustomerGroupID = "";
                obj1.Address = "";
                obj1.Avatar = "";

                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }

        #region Import
        public PartialViewResult ModalImportCard()
        {
            return PartialView();
        }

        public void DownloadFile()
        {
            Common.DownloadFile(Server.MapPath("~/Templates/addNewCard.xlsx"), "addNewCard.xlsx");
        }

        public JsonResult ImportFile()
        {
            //
            var userCard = GetCurrentUser.GetUser();
            var fileUpload = "";

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var txtError = "";
                        var name = Common.UploadFileWithDatetime(out txtError, Server.MapPath("~/upload/files/import/"), file);

                        var path = Path.Combine(Server.MapPath("~/upload/files/import/"), name);

                        fileUpload = name;

                        if (System.IO.File.Exists(path))
                        {
                            var dt = FunctionHelper.ReadFromExcelCardCustomer(path, ref txtError);
                            if (!string.IsNullOrWhiteSpace(txtError))
                            {
                                var result = new MessageReport();
                                result.Message = txtError;
                                result.isSuccess = false;

                                return Json(result, JsonRequestBehavior.AllowGet);
                            }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    //////Khai báo
                                    bool isNewCard = false;
                                    bool isNewCustomer = false;
                                    var customerIdmap = "";

                                    //////Lấy dữ liệu từ file
                                    var stt = item["STT"].ToString().Trim();

                                    //Kiểm tra chỉ khi có STT thì mới thực hiện
                                    if (!string.IsNullOrWhiteSpace(stt))
                                    {
                                        //Thẻ
                                        var cardno = item["CardNo"].ToString().Trim();
                                        var cardnumber = item["Mã thẻ"].ToString().Trim();
                                        var cardgroupname = item["Nhóm thẻ"].ToString().Trim();
                                        var dateexpire = !string.IsNullOrWhiteSpace(item["Ngày hết hạn"].ToString().Trim()) ? item["Ngày hết hạn"].ToString().Trim() : DateTime.Now.ToString("dd/MM/yyyy 23:59");
                                        var lockers = item["Tủ đồ"].ToString().Trim();
                                       // var vehiclenames = item["Tên xe"].ToString().Trim();

                                        //Khách hàng
                                        var customercode = item["Mã khách hàng"].ToString().Trim();
                                        var customername = item["Khách hàng"].ToString().Trim();
                                        var customergroupname = item["Nhóm khách hàng"].ToString().Trim();
                                        var customeridnumber = item["CMT"].ToString().Trim();
                                        var customermobile = item["Số điện thoại"].ToString().Trim();
                                        var customeraddress = item["Địa chỉ"].ToString().Trim();

                                        //Hoạt động
                                        var isLock = string.IsNullOrWhiteSpace(item["Sử dụng"].ToString().Trim()) ? false : (item["Sử dụng"].ToString().Trim().Equals("Hoạt động") ? false : true);

                                        //Id cardgroup
                                        var cardgroupid = GetCardGroupId(cardgroupname);

                                        //Id customergroup
                                        var customergroupid = GetCustomerGroupIdNew(customergroupname);

                                        //Lấy card custom
                                        var cardcustom = GetOrSetCardCustom(cardnumber);

                                        //Lấy khách hàng
                                        var customercustom = _tblCustomerService.GetCustomByCode(customercode);

                                        //////Gán giá trị mới từ form với các trường

                                        //Thẻ
                                        if (cardcustom != null)
                                        {
                                            cardcustom.CardNo = cardno;
                                            cardcustom.CardInActive = isLock;
                                            cardcustom.CardGroupID = cardgroupid;
                                        }
                                        else
                                        {
                                            //
                                            isNewCard = true;

                                            //
                                            cardcustom = new tblCardSubmit();
                                            cardcustom.CardID = Guid.NewGuid().ToString();
                                            cardcustom.CardNo = cardno;
                                            cardcustom.CardNumber = cardnumber;
                                            cardcustom.CardGroupID = cardgroupid;
                                            cardcustom.CardInActive = isLock;
                                            cardcustom.DtpDateExpired = dateexpire;
                                        }

                                        //var accesslever = GetListAccessLevel().FirstOrDefault(n => n.AccessLevelName.Equals(role));

                                        cardcustom.AccessLevelID = "";
                                       
                                        //Khách hàng
                                        if (!string.IsNullOrWhiteSpace(customercode))
                                        {
                                            if (customercustom != null)
                                            {
                                                customercustom.Address = customeraddress;
                                                customercustom.IDNumber = customeridnumber;
                                                customercustom.Mobile = customermobile;
                                                customercustom.CustomerName = customername;
                                                customercustom.CustomerGroupID = customergroupid;
                                            }
                                            else
                                            {
                                                //
                                                isNewCustomer = true;

                                                //
                                                customercustom = new tblCustomerSubmit();
                                                customercustom.CustomerID = Guid.NewGuid().ToString();
                                                customercustom.CustomerCode = customercode;
                                                customercustom.Address = customeraddress;
                                                customercustom.IDNumber = customeridnumber;
                                                customercustom.Mobile = customermobile;
                                                customercustom.CustomerName = customername;
                                                customercustom.CustomerGroupID = customergroupid;
                                            }

                                            customerIdmap = customercustom.CustomerID.ToString();
                                        }

                                        //Gán khách hàng vào thẻ
                                        cardcustom.CustomerID = customerIdmap;

                                        //////Nạp cở sở dữ liệu

                                        //Thẻ + Khách hàng
                                        SetCard_Customer(cardcustom, customercustom, isNewCard, isNewCustomer);

                                        //gán thẻ cho tủ đồ
                                        SetCardNumberToLocker(lockers, cardnumber, cardno);

                                        //////Lưu sự kiện thao tác

                                        //Gia hạn
                                        var oldexpire = cardcustom.DtpDateExpired;
                                        var newexpire = Convert.ToDateTime(dateexpire).ToString("dd/MM/yyyy");

                                        if (oldexpire != newexpire)
                                        {
                                            SaveCardExtendProcess(cardcustom, dateexpire, userCard.Id);
                                        }

                                        //Còn lại
                                        var listAction = GetListActionType(cardcustom);
                                        if (listAction.Any())
                                        {
                                            foreach (var itemAc in listAction)
                                            {
                                                SaveCardProcess(cardcustom, itemAc, userCard.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var resultUp1 = new MessageReport();
                resultUp1.isSuccess = true;
                resultUp1.Message = "Upload excel thành công";

                //////Lưu log sự kiện
                WriteLog.Write(resultUp1, userCard, fileUpload, fileUpload, "tblCard", ConstField.ParkingCode, ActionConfigO.ImportExcel);

                return Json(resultUp1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var resultUp3 = new MessageReport();
                resultUp3.isSuccess = true;
                resultUp3.Message = ex.Message;

                return Json(resultUp3, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        private string GetCardGroupId(string name)
        {
            var obj = _tblCardGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CardGroupID.ToString();
            }

            return "";
        }

        private string GetCustomerGroupId(string name)
        {
            var obj = _tblCustomerGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CustomerGroupID.ToString();
            }

            return "";
        }

        private string GetCustomerGroupIdNew(string name)
        {
            var obj = _tblCustomerGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CustomerGroupID.ToString();
            }
            else
            {
                var newobj = new tblCustomerGroup
                {
                    CustomerGroupName = name,
                    CustomerGroupID = Guid.NewGuid(),
                    Inactive = false,
                    ParentID = "0",
                    Ordering = _tblCustomerGroupService.GetAll().Count() + 1
                };

               var result = _tblCustomerGroupService.Create(newobj);
                if (result.isSuccess)
                {
                    return newobj.CustomerGroupID.ToString();
                }            
            }

            return "";
        }

        private tblCardSubmit GetOrSetCardCustom(string cardnumber)
        {
            var card = _tblCardService.GetByCardNumber(cardnumber);
            if (card != null)
            {
                var obj = _tblCardService.GetCustomById(card.CardID);

                obj.DtpDateExpired = Convert.ToDateTime(card.ExpireDate).ToString("dd/MM/yyyy");

                obj.DtpDateRegisted = Convert.ToDateTime(card.DateRegister != null ? card.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                obj.DtpDateReleased = Convert.ToDateTime(card.DateRelease != null ? card.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");

                obj.OldDtpDateRegisted = Convert.ToDateTime(card.DateRegister != null ? card.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                obj.OldDtpDateReleased = Convert.ToDateTime(card.DateRelease != null ? card.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");

                return obj;
            }


            return null;
        }

        private void SetCard_Customer(tblCardSubmit cardsubmit, tblCustomerSubmit customersubmit, bool isNewCard, bool isNewCustomer)
        {
            var str = new StringBuilder();

            //Thẻ
            if (isNewCard)
            {
                str.AppendLine("INSERT INTO [dbo].[tblCard]([CardNo], [CardNumber], [CustomerID], [CardGroupID], [ImportDate], [ExpireDate], [IsLock], [IsDelete], [Plate1], [VehicleName1], [Plate2], [VehicleName2], [Plate3], [VehicleName3],[AccessLevelID])");

                str.AppendLine("VALUES (");

                str.AppendLine(string.Format("'{0}'", cardsubmit.CardNo));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardNumber));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CustomerID));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardGroupID));
                str.AppendLine(",GETDATE()");
                str.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(cardsubmit.DtpDateExpired).ToString("yyyy/MM/dd")));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardInActive ? 1 : 0));
                str.AppendLine(", 0");

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate1));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName1));

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate2));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName2));

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate3));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName3));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.AccessLevelID));

                str.AppendLine(")");
            }
            else
            {
                str.AppendLine("UPDATE [dbo].[tblCard] SET");
                str.AppendLine(string.Format(" [CustomerID] = '{0}'", cardsubmit.CustomerID));
                str.AppendLine(string.Format(",[IsLock] = '{0}'", cardsubmit.CardInActive ? 1 : 0));

                if (!string.IsNullOrWhiteSpace(cardsubmit.CardNo))
                    str.AppendLine(string.Format(",[CardNo] = '{0}'", cardsubmit.CardNo));

                if (!string.IsNullOrWhiteSpace(cardsubmit.CardGroupID))
                    str.AppendLine(string.Format(",[CardGroupID] = '{0}'", cardsubmit.CardGroupID));

                if (!string.IsNullOrWhiteSpace(cardsubmit.DtpDateExpired))
                    str.AppendLine(string.Format(",[ExpireDate] = '{0}'", Convert.ToDateTime(cardsubmit.DtpDateExpired).ToString("yyyy/MM/dd")));

                if (!string.IsNullOrWhiteSpace(cardsubmit.Plate1))
                    str.AppendLine(string.Format(",[Plate1] = '{0}'", cardsubmit.Plate1));

                if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName1))
                    str.AppendLine(string.Format(",[VehicleName1] = N'{0}'", cardsubmit.VehicleName1));

                if (!string.IsNullOrWhiteSpace(cardsubmit.Plate2))
                    str.AppendLine(string.Format(",[Plate2] = '{0}'", cardsubmit.Plate2));

                if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName2))
                    str.AppendLine(string.Format(",[VehicleName2] = N'{0}'", cardsubmit.VehicleName2));

                if (!string.IsNullOrWhiteSpace(cardsubmit.Plate3))
                    str.AppendLine(string.Format(",[Plate3] = '{0}'", cardsubmit.Plate3));

                if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName3))
                    str.AppendLine(string.Format(",[VehicleName3] = N'{0}'", cardsubmit.VehicleName3));
                
                str.AppendLine(string.Format(",[AccessLevelID] = '{0}'", cardsubmit.AccessLevelID));

                str.AppendLine(string.Format("WHERE CardNumber = '{0}'", cardsubmit.CardNumber));
            }

            //Khách hàng
            if (customersubmit != null)
            {
                if (!string.IsNullOrWhiteSpace(customersubmit.CustomerCode))
                {
                    if (isNewCustomer)
                    {
                        //var k = _tblCustomerService.GetAll().Count();

                        str.AppendLine("INSERT INTO [dbo].[tblCustomer]");
                        str.AppendLine("([CustomerID]");
                        str.AppendLine(", [CustomerName]");
                        str.AppendLine(", [CustomerCode]");
                        str.AppendLine(", [Address]");
                        str.AppendLine(", [Mobile]");
                        str.AppendLine(", [IDNumber]");
                        str.AppendLine(", [CustomerGroupID]");
                        str.AppendLine(", [EnableAccount]");
                        str.AppendLine(", [Inactive]");
                        str.AppendLine(", [UserIDofFinger], [Finger1], [Finger2], [DevPass], [AccessExpireDate])");
                        str.AppendLine(string.Format("VALUES('{0}', N'{1}','{2}', N'{3}', '{4}', '{5}', '{6}', 1 , 0, 0, '', '', '', '2099-12-31')", customersubmit.CustomerID, customersubmit.CustomerName, customersubmit.CustomerCode, customersubmit.Address, customersubmit.Mobile, customersubmit.IDNumber, customersubmit.CustomerGroupID));
                    }
                    else
                    {
                        str.AppendLine("UPDATE [dbo].[tblCustomer]");
                        str.AppendLine(string.Format("SET [CustomerName] = N'{0}'", customersubmit.CustomerName));
                        str.AppendLine(string.Format(",[Address] = N'{0}'", customersubmit.Address));
                        str.AppendLine(string.Format(",[Mobile] = N'{0}'", customersubmit.Mobile));
                        str.AppendLine(string.Format(",[IDNumber] = N'{0}'", customersubmit.IDNumber));
                        str.AppendLine(string.Format(",[CustomerGroupID] = '{0}'", customersubmit.CustomerGroupID));
                        str.AppendLine(string.Format("WHERE CONVERT(varchar(50),[CustomerID]) = '{0}'", customersubmit.CustomerID));
                    }
                }
            }

            //
            ExcuteSQL.Execute(str.ToString());
        }

        public PartialViewResult ModalButtonControl(int totalItem = 0, string url = "")
        {
            var listCardChoice = GetSetFromSession(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;
            ViewBag.llevels = GetListAccessLevel();

            return PartialView(listCardChoice);
        }

        public PartialViewResult ShowCardSelected(string cardnumber)
        {
            var listCardChoice = GetSetFromSession(null);

            return PartialView(listCardChoice);
        }

        public JsonResult RemoveAllCardSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionAccessSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrRemoveOneAllCardSeleted(List<string> CardNumbers, bool isAdd)
        {
            GetSetFromSession(CardNumbers, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSession(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listCardChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionAccessSession, host)];
            if (listCardChoice != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listCardChoice.Any(n => n.Equals(item)))
                            {
                                listCardChoice.Add(item);
                            }
                        }
                        else
                        {
                            if (listCardChoice.Any(n => n.Equals(item)))
                            {
                                listCardChoice.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listCardChoice = list;
                }
                else
                {
                    listCardChoice = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionAccessSession, host)] = listCardChoice;

            return listCardChoice;
        }

        public JsonResult ActionToCards(string type, string mess, string value = "")
        {
            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = "Có lỗi xảy ra";

            var data = GetSetFromSession(null);
            if (data != null && data.Any())
            {
                result = ActionProcess(type, data, value, mess);
            }
            else
            {
                if (type != "AUTHORIZEALL")
                {
                    result.isSuccess = false;
                    result.Message = "Vui lòng chọn thẻ.";
                }
                else
                {
                    result = ActionProcess(type, data, value, mess);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private MessageReport ActionProcess(string type, List<string> data, string value = "", string mess = "")
        {
            var listCard = "";
            var userCard = GetCurrentUser.GetUser();

            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = "Có lỗi xảy ra";

            try
            {
                var str = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(mess))
                {
                    str.AppendLine("UPDATE tblCard SET {0}, Description = N'" + mess + "'");
                }
                else
                {
                    str.AppendLine("UPDATE tblCard SET {0}");
                }

                str.AppendLine("WHERE CardNumber IN (");
                var count = 0;
                foreach (var item in data)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item, count == data.Count ? "" : ","));

                    listCard += string.Format("{0}{1}", item, count == data.Count ? "" : ",");
                }

                str.AppendLine(")");

                switch (type)
                {
                    case "LOCK":
                        var tLOCK = string.Format(str.ToString(), "IsLock = 1");
                        result.isSuccess = ExcuteSQL.Execute(tLOCK);
                        break;
                    case "UNLOCK":
                        var tUNLOCK = string.Format(str.ToString(), "IsLock = 0");
                        result.isSuccess = ExcuteSQL.Execute(tUNLOCK);
                        break;
                    case "DELETE":
                        var tDELETE = string.Format(str.ToString(), "IsDelete = 1");
                        result.isSuccess = ExcuteSQL.Execute(tDELETE);
                        break;
                    case "AUTHORIZE":
                        var tAUTHORIZE = string.Format(str.ToString(), "AccessLevelID = '" + value + "'");
                        result.isSuccess = ExcuteSQL.Execute(tAUTHORIZE);
                        break;
                    case "AUTHORIZEALL":
                        result.isSuccess = UpdateMultiCard(value).isSuccess;
                        break;
                    default:
                        break;
                }

                if (result.isSuccess)
                {
                    result.Message = "Thực hiện thành công";

                    switch (type)
                    {
                        case "LOCK":
                            WriteLog.Write(result, userCard, listCard, listCard, "tblCard", ConstField.LockerCode, ActionConfigO.Lock);
                            break;
                        case "UNLOCK":
                            WriteLog.Write(result, userCard, listCard, listCard, "tblCard", ConstField.LockerCode, ActionConfigO.Unlock);
                            break;
                        case "DELETE":
                            WriteLog.Write(result, userCard, listCard, listCard, "tblCard", ConstField.LockerCode, ActionConfigO.Delete);
                            break;
                        case "AUTHORIZE":
                            WriteLog.Write(result, userCard, listCard, listCard, "tblCard", ConstField.LockerCode, ActionConfigO.Authorize);
                            break;
                        default:
                            break;
                    }
                }

                if (type == "DELETE")
                {
                    var host = Request.Url.Host;
                    Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionAccessSession, host)] = new List<string>();
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        private MessageReport UpdateMultiCard(string newvalue)
        {
            var result = _tblCardService.UpdateMultiCardLevel(keyV, cardgroupsV, customeridV, strCGV, fromdateV, todateV, isCheckByTimeV, levelsV, newvalue);

            return result;
        }


        public PartialViewResult CardUploadStatus(string cardnumber, string accesslevel)
        {
            var success = 0;
            var total = 0;

            var strController = new List<string>();

            var listAccessDetail = _tblAccessLevelDetailService.GetAllByLevelId(accesslevel);
            if (listAccessDetail.Any())
            {
                foreach (var item in listAccessDetail)
                {
                    strController.Add(item.ControllerID);
                }
            }

            //var listAccessControl = _tblAccessControllerService.GetAllByListId(strController).ToList();

            var cardController = _tblAccessUploadDetailService.GetAllByCardNumber(cardnumber).ToList();

            foreach (var item in cardController)
            {
                if (strController.Contains(item.ControllerID))
                {
                    total++;

                    if (item.Action == "UPLOAD" && item.Status == "SUCCESS")
                    {
                        success++;
                    }
                }
            }

            var model = new SelectListModelCardUploadStatus()
            {
                ListController = null,
                ListUploadDetail = null,
                Status = string.Format("<span class='label label-sm label-info'> {0} / {1} </span>", success, total)
            };

            return PartialView(model);
        }

        /// <summary>
        /// gán thẻ cho tủ đồ
        /// </summary>
        /// <param name="lockers"></param>
        /// <param name="cardnumber"></param>
        /// <param name="cardno"></param>
        void SetCardNumberToLocker(string lockers,string cardnumber,string cardno)
        {
            if(!string.IsNullOrEmpty(lockers) && !string.IsNullOrEmpty(cardnumber))
            {
                var arr = lockers.Split(';');
                if(arr.Length > 0)
                {
                    foreach(var item in arr)
                    {
                        var objLocker = _tblLockerService.GetByName(item.Trim());

                        if(objLocker != null && string.IsNullOrEmpty(objLocker.CardNumber) && objLocker.LockerType == "0")
                        {
                            objLocker.CardNumber = cardnumber;
                            objLocker.CardNo = cardno;
                            objLocker.LockerType = "1";
                            _tblLockerService.UpdateSql(objLocker);
                        }
                    }
                }
            }
        }
    }
}