using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
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
using System.Web;
using System.Web.Mvc;
using Kztek.Web.Core.Functions;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblCustomerController : Controller
    {
        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCardService _tblCardService;
        private ItblCompartmentService _tblCompartmentService;

        public tblCustomerController(ItblCompartmentService _tblCompartmentService,ItblCustomerService _tblCustomerService, ItblCustomerGroupService _tblCustomerGroupService, ItblSystemConfigService _tblSystemConfigService, ItblCardService _tblCardService)
        {
            this._tblCustomerService = _tblCustomerService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCardService = _tblCardService;
            this._tblCompartmentService = _tblCompartmentService;
        }

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", string customergroup = "", int page = 1, string chkExport = "0", string customerstatus = "", string selectedId = "")
        {
            var str = GetListChild("", customergroup);

            if (chkExport.Equals("1"))
            {
                var listExcel = _tblCustomerService.ExcelAllByFirst(key, str, customerstatus);
                var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCustomer", "Index");
                //Xuất file theo format
                PK_CustomerMapFormatCell(listExcel, Dictionary["TitleEx"], "Sheet1", "", Dictionary["Title"], "");

                return RedirectToAction("Index", new { key = key, customergroup = customergroup, customerstatus = customerstatus, page = page });
            }

            var pageSize = 20;

            var list = _tblCustomerService.GetAllPagingByFirst(key, str, page, pageSize, customerstatus);
            if (list.Any())
            {
                var lstId = "";
                var lstCustomerId = "";
                foreach (var item in list)
                {
                    lstId += item.CustomerGroupID + ";";
                    lstCustomerId += item.CustomerID.ToString() + ",";
                }

                ViewBag.listCustomerGroup = _tblCustomerGroupService.GetAllActiveByListId(lstId).ToList();
                //ViewBag.listCardCustomer = _PK_CardCustomerService.GetAllCardByListCustomerId(lstCustomerId).ToList();
            }

            var gridModel = PageModelCustom<tblCustomer>.GetPage(list, page, pageSize);
            ViewBag.Compartments = _tblCompartmentService.GetAll();
            ViewBag.keyValue = key;
            ViewBag.customergroupValue = customergroup;
            ViewBag.customerstatusValue = customerstatus;
            ViewBag.selectedIdValue = selectedId;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerStatusList = GetCustomerStatusList();


            return View(gridModel);
        }

        private void PK_CustomerMapFormatCell(List<tblCustomer_Excel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCustomer", "Index");
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] +": "+ titleTime.Split(new[] { '-' })[0] +" - "+ DictionaryAction["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["NumberRow"], ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerCode"] , ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerName"] , ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["IdentityCard"]  , ItemValue = "CustomerIdentify" });     
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "CustomerAddress" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["phone"], ItemValue = "CustomerMobile" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerGroup"], ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Compartment"] , ItemValue = "CompartmentId" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNumber"], ItemValue = "Cards" });
            //listColumn.Add(new SelectListModel { ItemText = Dictionary["licensePlate"], ItemValue = "Plates" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Status"], ItemValue = "Active" });
           
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
        #endregion

        #region Thêm mới
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key = "", string customergroup = "", string customerstatus = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.CustomerGroupValue = customergroup;
            ViewBag.customerstatusValue = customerstatus;
            ViewBag.Compartments = GetCompartments();
            ViewBag.CustomerGroups = GetMenuList();
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            return View();
        }

        [HttpPost]
        public ActionResult Create(tblCustomer obj, HttpPostedFileBase FileUpload, bool SaveAndCountinue = false, string key = "", string customergroup = "", string customerstatus = "", string RePassword = "")
        {
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            ViewBag.keyValue = key;
            ViewBag.CustomerGroupValue = customergroup;
            ViewBag.customerstatusValue = customerstatus;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.Compartments = GetCompartments();
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerName))
            {
                ModelState.AddModelError("CustomerName", DictionaryAction["enter_customer_name"]);
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                ModelState.AddModelError("CustomerCode", DictionaryAction["enter_customer_code"]);
                return View(obj);
            }

            var existed = _tblCustomerService.GetByCode(obj.CustomerCode);
            if (existed != null)
            {
                ModelState.AddModelError("CustomerCode",DictionaryAction["Customer_code_already_exists"]);
                return View(obj);
            }

            if (!string.IsNullOrWhiteSpace(obj.Password))
            {
                if (obj.Password != RePassword)
                {
                    ModelState.AddModelError("Password", DictionaryAction["correct_password"]);
                    return View(obj);
                }

                obj.Password = CryptorEngine.Encrypt(obj.Password, true);
            }

            //Gán giá trị
            obj.CustomerID = Guid.NewGuid();
            obj.AccessLevelID = "";
            obj.Finger1 = "";
            obj.Finger2 = "";
            obj.DevPass = "";
            obj.AccessExpireDate = Convert.ToDateTime("2099/12/31");
            obj.CompartmentId = !string.IsNullOrEmpty(obj.CompartmentId) ? obj.CompartmentId.Trim() : "";
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                obj.Avatar = string.Format("{0}{1}", url, fileName);
            }

            if (!string.IsNullOrEmpty(obj.Address))
            {
                obj.AddressUnsign = StringUtil.RemoveSpecialCharactersVn(obj.Address.ToLower()).Replace("-"," ");
            }

            //Thực hiện thêm mới
            var result = _tblCustomerService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Create);

                UploadFile(FileUpload);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key, customergroup = customergroup, customerstatus = customerstatus, selectedId = obj.CustomerID });
                }

                return RedirectToAction("Index", new { key = key, customergroup = customergroup, customerstatus = customerstatus, selectedId = obj.CustomerID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
        #endregion

        #region Cập nhật
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key = "", string customergroup = "", string customerstatus = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.customergroupValue = customergroup;
            ViewBag.customerstatusValue = customerstatus;
            ViewBag.PN = page;

            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.Compartments = GetCompartments();

            var obj = _tblCustomerService.GetById(Guid.Parse(id));

            if (obj != null && !string.IsNullOrWhiteSpace(obj.Password))
            {
                obj.Password = CryptorEngine.Decrypt(obj.Password, true);
            }
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            return View(obj);
        }

       
        [HttpPost]
        public ActionResult Update(tblCustomer obj, HttpPostedFileBase FileUpload, string key = "", string customergroup = "", string customerstatus = "", string RePassword = "", int page = 1)
        {
            var dictonary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            ViewBag.keyValue = key;
            ViewBag.customergroupValue = customergroup;
            ViewBag.customerstatusValue = customerstatus;
            ViewBag.PN = page;
            ViewBag.Compartments = GetCompartments();
            ViewBag.CustomerGroups = GetMenuList();
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            //Kiểm tra
            var oldObj = _tblCustomerService.GetById(obj.CustomerID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerName))
            {
                ModelState.AddModelError("CustomerName", dictonary["enter_customer_name"]);
                return View(oldObj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                ModelState.AddModelError("CustomerCode", dictonary["enter_customer_code"]);
                return View(oldObj);
            }

            var existed = _tblCustomerService.GetByCode_Id(obj.CustomerCode, obj.CustomerID.ToString());
            if (existed != null)
            {
                ModelState.AddModelError("CustomerCode", dictonary["Customer_code_already_exists"]);
                return View(oldObj);
            }

            if (!string.IsNullOrWhiteSpace(obj.Password))
            {
                if (obj.Password != RePassword)
                {
                    ModelState.AddModelError("Password", dictonary["correct_password"]);
                    return View(oldObj);
                }

                oldObj.Password = CryptorEngine.Encrypt(obj.Password, true);
            }

            //Gán giá trị
            oldObj.Account = obj.Account;
            oldObj.Address = obj.Address;
            oldObj.CustomerCode = obj.CustomerCode;
            oldObj.CompartmentId = !string.IsNullOrEmpty(obj.CompartmentId) ? obj.CompartmentId.Trim() : "";
            oldObj.CustomerGroupID = obj.CustomerGroupID;
            oldObj.CustomerName = obj.CustomerName;
            oldObj.Description = obj.Description;
            oldObj.EnableAccount = obj.EnableAccount;
            oldObj.IDNumber = obj.IDNumber;
            oldObj.Inactive = obj.Inactive;
            oldObj.Mobile = obj.Mobile;
            oldObj.SortOrder = obj.SortOrder;

            if (!string.IsNullOrEmpty(oldObj.Address))
            {
                oldObj.AddressUnsign = StringUtil.RemoveSpecialCharactersVn(oldObj.Address.ToLower()).Replace("-", " ");
            }
            else
            {
                oldObj.AddressUnsign = "";
            }

            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                oldObj.Avatar = string.Format("{0}{1}", url, fileName);
            }

            //Thực hiện cập nhật
            var result = _tblCustomerService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                WriteLog.WriteLogFile(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                UploadFile(FileUpload);

                return RedirectToAction("Index", new { page = page, key = key, customergroup = customergroup, customerstatus = customerstatus, selectedId = oldObj.CustomerID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }
        #endregion

        #region Xóa
        public JsonResult Delete(string id)
        {
            var obj = new tblCustomer();

            //Check tồn tại trong cardcustomer
            var existedInCard = _tblCardService.GetAllByCustomerId(id);
            if (existedInCard.Any())
            {
                var result1 = new MessageReport();
                result1.Message = "Khách hàng đang sử dụng thẻ. Không thể xóa.";
                result1.isSuccess = false;
            }

            //Check tồn tại trong event
            //var existedInEvent = _PK_VehicleEventService.GetAllEventByCustomerId(id);
            //if (existedInEvent.Any())
            //{
            //    var result1 = new Result();
            //    result1.ErrorCode = 500;
            //    result1.Message = "Khách hàng đang tồn tại trong sự kiện. Không thể xóa.";
            //    result1.Success = false;
            //}

            var result = _tblCustomerService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload file
        private void UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                string error = "";

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];

                Common.UploadFile(out error, Server.MapPath(url), fileUpload);
            }
        }
        #endregion

        #region Danh sách hõ trợ



        public List<SelectListModel> GetCompartments()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listResidentGroup = _tblCompartmentService.GetAll().ToList();
            if (listResidentGroup.Any())
            {
                foreach (var item in listResidentGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.CompartmentID.ToString(),  ItemText = item.CompartmentName });
                }
            }
           

            return list;
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
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CustomerGroupID");
            var list = new List<SelectListModel>
            {
                new SelectListModel {  ItemValue = "", ItemText = Dictionary["SlectlistCusGrp"] }
            };
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

        private List<SelectListModel> GetCustomerStatusList()
        {
            return FunctionHelper.CustomerStatusType();
        }
        #endregion

        #region ImportFile
        public PartialViewResult ModalImportCard()
        {
            return PartialView();
        }
        public JsonResult ImportFile()
        {
            var userCard = GetCurrentUser.GetUser();
            var fileUpload = "";
            try
            {
                // upload file
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
                            // đọc file
                            var dt = FunctionHelper.ReadExcelCustomer(path, ref txtError);
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
                                   
                                    // lấy dữ liệu từ file
                                    var stt = item["STT"].ToString().Trim();

                                    // kiêm tra chỉ khi có stt thì ms  thực hiên
                                    if (!string.IsNullOrWhiteSpace(stt))
                                    {
                                        // khách hàng
                                        var customercode = item["Mã KH"].ToString().Trim();
                                        var customername = item["Tên KH"].ToString().Trim();
                                        var idnumber = item["Chứng minh thư"].ToString().Trim();
                                        var address = item["Địa chỉ"].ToString().Trim();
                                        var mobile = item["SĐT"].ToString().Trim();
                                        var customergrname = item["Nhóm KH"].ToString().Trim();
                                        var compartmentId = "";
                                        if (!string.IsNullOrEmpty(item["Căn hộ"].ToString().Trim()))
                                        {
                                            var objCompartment = _tblCompartmentService.GetByNameDetail(item["Căn hộ"].ToString().Trim());
                                            compartmentId = objCompartment.CompartmentID.ToString();
                                        }
                                     

                                        // hoạt động
                                        var islock = string.IsNullOrWhiteSpace(item["Trạng thái"].ToString().Trim()) ? false :
                                            (item["Trạng thái"].ToString().Trim().Equals("Hoạt động") ? false : true);

                                        if (!string.IsNullOrWhiteSpace(customercode))
                                        {

                                        var customerGrId = GetCustomerGroupId(customergrname);

                                        var obj = _tblCustomerService.GetByCode(customercode);
                                        if (obj != null)
                                        {
                                            obj.CustomerName = customername;
                                            obj.IDNumber = idnumber;
                                            obj.Address = address;
                                            obj.Mobile = mobile;
                                            obj.CustomerGroupID = customerGrId;
                                            obj.CompartmentId = compartmentId;
                                            obj.Inactive = islock;
                                            if (!string.IsNullOrWhiteSpace(address))
                                            {
                                                obj.AddressUnsign = StringUtil.RemoveSpecialCharactersVn(obj.Address.ToLower()).Replace("-", " ");
                                            }
                                            var result = _tblCustomerService.Update(obj);
                                            if (result.isSuccess)
                                            {
                                                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                                                WriteLog.WriteLogFile(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                                            }
                                        }


                                        else
                                        {
                                            obj = new tblCustomer
                                            {
                                                CustomerID = Guid.NewGuid(),
                                                CustomerName = customername,
                                                Address = address,
                                                CustomerCode = customercode,
                                                CustomerGroupID = customerGrId,
                                                Mobile = mobile,
                                                Inactive = islock,
                                                IDNumber = idnumber,
                                                AccessLevelID = "",
                                                Finger1 ="",
                                                Finger2 = "",
                                                DevPass ="",
                                                AccessExpireDate =Convert.ToDateTime("2099/12/31"),
                                                
                                            };
                                            if (!string.IsNullOrWhiteSpace(address))
                                            {
                                                obj.AddressUnsign = StringUtil.RemoveSpecialCharactersVn(obj.Address.ToLower()).Replace("-"," ");
                                            }
                                            var result = _tblCustomerService.Create(obj);
                                            if (result.isSuccess)
                                            {
                                                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerID.ToString(), obj.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Create);

                                            }
                                        }
                                        }
                                        // 

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
                WriteLog.Write(resultUp1, userCard, fileUpload, fileUpload, "tblCustomer", ConstField.ParkingCode, ActionConfigO.ImportExcel);

                return Json(resultUp1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                var resultUp3 = new MessageReport();
                resultUp3.isSuccess = false;
                resultUp3.Message = ex.Message;

                return Json(resultUp3, JsonRequestBehavior.AllowGet);
            }
          
        }

        private string GetCustomerGroupId(string name)
        {
            var obj = _tblCustomerGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CustomerGroupID.ToString();
            }
            else
            {
                obj = new tblCustomerGroup
                {
                    CustomerGroupID = Guid.NewGuid(),
                    CustomerGroupName = name,
                    ParentID = "0",
                    Inactive = false
                };
                var result = _tblCustomerGroupService.Create(obj);
                if (result.isSuccess)
                {
                    return obj.CustomerGroupID.ToString();
                }
            }
            return "";
        }
        #endregion
    }
}