using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
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
    public class tblSubCardController : Controller
    {
        public static string url;
        public static string objId;
        private ItblSubCardService _tblSubCardService;
        private ItblSystemConfigService _tblSystemConfigService;
        public tblSubCardController(ItblSubCardService _tblSubCardService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSubCardService = _tblSubCardService;
            this._tblSystemConfigService = _tblSystemConfigService;
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
        public ActionResult Index(string key, int page = 1, string chkExport = "0")
        {
            var pageSize = 20;
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblSubCard", "Index");

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _tblSubCardService.Excel(key);

                //Xuất file theo format
                FormatCell(listExcel, Dictionary["TitleExcel"], "Sheet1", "", Dictionary["Title"]);

                return RedirectToAction("ReportIn");
            }
            #endregion

            var list = _tblSubCardService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblSubCard>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            url = Request.Url.AbsoluteUri;
            ViewBag.objId = objId;
            return View(gridModel);
        }
        private void FormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "reportIn");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["stt"], ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["mainCard"], ItemValue = "Mã thẻ chính" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["codeCard"], ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["cardNo"], ItemValue = "CardNo" });
           

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
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
            ViewBag.Url = url;
            return View();
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
        public ActionResult Create(tblSubCard obj, bool SaveAndCountinue = false)
        {
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            //Danh sách sử dụng
            ViewBag.Url = url;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.MainCard))
            {
                ModelState.AddModelError("MainCard", DictionaryAction["enter_MainCard_name"]);
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.CardNumber))
            {
                ModelState.AddModelError("CardNumber", DictionaryAction["Please_enter_the_Card_Number"]);
                return View(obj);
            }

            var cgroup = _tblSubCardService.GetByCard("",obj.CardNumber);

            if (cgroup != null)
            {
                ModelState.AddModelError("CardNumber", DictionaryAction["Card_code_already_exists"]);
                return View(obj);
            }

            //Gán giá trị
            obj.IsDelete = false;
            obj.MainCard = !string.IsNullOrEmpty(obj.MainCard) ? obj.MainCard.Trim() : "";
            obj.CardNumber = !string.IsNullOrEmpty(obj.CardNumber) ? obj.CardNumber.Trim() : "";
            obj.CardNo = !string.IsNullOrEmpty(obj.CardNo) ? obj.CardNo.Trim() : "";

            //Thực hiện thêm mới
            var result = _tblSubCardService.Create(obj);
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
            var obj = _tblSubCardService.GetById(!string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0);
            ViewBag.Url = url;
            TempData["obj"] = obj;

            objId = obj.ID.ToString();
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
        public ActionResult Update(tblSubCard obj, string id)
        {
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            ViewBag.Url = url;

            //Kiểm tra
            var oldObj = _tblSubCardService.GetById(!string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0);
            if (oldObj == null)
            {
                ViewBag.Error = DictionaryAction["record_does_not_exist"];
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            if (string.IsNullOrEmpty(obj.MainCard))
            {
                ModelState.AddModelError("MainCard", DictionaryAction["enter_MainCard_name"]);
                return View(oldObj);
            }

            if (string.IsNullOrEmpty(obj.CardNumber))
            {
                ModelState.AddModelError("CardNumber", DictionaryAction["Please_enter_the_Card_Number"]);
                return View(oldObj);
            }

            var cgroup = _tblSubCardService.GetByCard("", obj.CardNumber);

            if (cgroup != null && !cgroup.ID.ToString().Equals(id))
            {
                ModelState.AddModelError("CardNumber", DictionaryAction["Card_code_already_exists"]);
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.MainCard = !string.IsNullOrEmpty(obj.MainCard) ? obj.MainCard.Trim() : "";
            oldObj.CardNumber = !string.IsNullOrEmpty(obj.CardNumber) ? obj.CardNumber.Trim() : "";
            oldObj.CardNo = !string.IsNullOrEmpty(obj.CardNo) ? obj.CardNo.Trim() : "";

            //Thực hiện cập nhật
            var result = _tblSubCardService.Update(oldObj);
            if (result.isSuccess)
            {
                objId = oldObj.ID.ToString();
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
      

        #endregion

        public JsonResult Delete(string id)
        {
            var obj = new tblSubCard();

            var result = _tblSubCardService.DeleteById(Convert.ToInt32(id), ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoComplete(string key)
        {
            var cus = _tblSubCardService.AutoComplete(key);
  
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        #region Import
        public PartialViewResult ModalImport()
        {
            return PartialView();
        }

        public void DownloadFile()
        {
            Common.DownloadFile(Server.MapPath("~/Templates/addSubCard.xlsx"), "addSubCard.xlsx");
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

                                    //////Lấy dữ liệu từ file
                                    var stt = item["STT"].ToString().Trim();

                                    //Kiểm tra chỉ khi có STT thì mới thực hiện
                                    if (!string.IsNullOrWhiteSpace(stt))
                                    {
                                        //Thẻ
                                        var cardno = item["CardNo"].ToString().Trim();
                                        var cardnumber = item["Mã thẻ"].ToString().Trim();
                                        var maincard = item["Mã thẻ chính"].ToString().Trim();

                                        if(!string.IsNullOrEmpty(cardnumber) && !string.IsNullOrEmpty(maincard))
                                        {
                                            var objSubCard = _tblSubCardService.GetByCard("", cardnumber);

                                            if(objSubCard != null)
                                            {
                                                objSubCard.CardNo = cardno;
                                                objSubCard.CardNumber = cardnumber;
                                                objSubCard.MainCard = maincard;

                                                _tblSubCardService.Update(objSubCard);
                                            }
                                            else
                                            {
                                                objSubCard = new tblSubCard()
                                                {
                                                    CardNo = cardno,
                                                    CardNumber = cardnumber,
                                                    MainCard = maincard,
                                                    IsDelete = false
                                                };

                                                _tblSubCardService.Create(objSubCard);
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
                WriteLog.Write(resultUp1, userCard, fileUpload, fileUpload, "tblSubCard", ConstField.ParkingCode, ActionConfigO.ImportExcel);

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
        #endregion
    }
}