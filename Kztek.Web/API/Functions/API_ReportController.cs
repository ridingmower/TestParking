using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kztek.Web.API.Functions
{
    [RoutePrefix("api/report")]
    public class API_ReportController : ApiController
    {
        private ItblCardService _tblCardService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCustomerGroupService _tblCustomerGroupService;

        public API_ReportController(ItblCardService _tblCardService, ItblSystemConfigService _tblSystemConfigService, ItblCustomerGroupService _tblCustomerGroupService)
        {
            this._tblCardService = _tblCardService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
        }

        private byte[] PK_CardCustomerFormatCell2(List<tbl_CardExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            //var user = GetCurrentUser.GetUser();
            var user = new User() { Name = "APP" };
            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionaryAction["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);
            var systemconfig = _tblSystemConfigService.GetDefault();
            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["NumberRow"], ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNumber"], ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardGroup"], ItemValue = "CardGroupNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["ExpireDate"], ItemValue = "DateExpire" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Plate"], ItemValue = "Plates" });

            if (systemconfig != null && systemconfig.FeeName.Contains("TRANSERCO"))
            {
                listColumn.Add(new SelectListModel { ItemText = Dictionary["ContractCode"], ItemValue = "Description" });
            }

            listColumn.Add(new SelectListModel { ItemText = Dictionary["VehicleNames"], ItemValue = "VehicleNames" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerCode"], ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerName"], ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerGroupName"], ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["IdentityCard"], ItemValue = "CMT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["phone"], ItemValue = "SĐT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Inactive"], ItemValue = "Inactive" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày đăng kí", ItemValue = "Ngày đăng kí" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            if (systemconfig != null && !systemconfig.FeeName.Contains("TRANSERCO"))
            {
                dt.Columns.Remove("Description");
            }
            //Xuất file
            return GetByte(dt, listColumn, dtHeader, filename, sheetname, comments);
        }

        private byte[] GetByte(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments);
            var data = ReadToEnd(stream);

            return data;
        }

        [HttpPost]
        [Route("getcardlist")]
        public IHttpActionResult GetCardList(string key = "", string cardgroups = "", string customerid = "", string customergroups = "", string fromdate = "", string todate = "", bool desc = false, string columnQuery = "ImportDate", string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false)
        {
            var str = GetListChild("", customergroups);

            var listExcel = _tblCardService.GetExcelCardByFirstParkingTSQL(key, cardgroups, customerid, str, fromdate, todate, desc, columnQuery, ischeckbytime, "", active, isfindautocapture);
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");

            var dataBytes = PK_CardCustomerFormatCell2(listExcel, Dictionary["TitleEx"], "Sheet1", "", Dictionary["Title"], fromdate + " - " + todate);

            var dataStream = new MemoryStream(dataBytes);

            return new FileResult(dataStream, Request, Dictionary["TitleEx"]);
        }

        [HttpPost]
        [Route("getcardlistHRM")]
        public HttpResponseMessage GetCardListHRM(string key = "", string cardgroups = "", string customerid = "", string customergroups = "", string fromdate = "", string todate = "", bool desc = false, string columnQuery = "ImportDate", string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false)
        {
            var str = GetListChild("", customergroups);

            var listExcel = _tblCardService.GetExcelCardByFirstParkingTSQL(key, cardgroups, customerid, str, fromdate, todate, desc, columnQuery, ischeckbytime, "", active, isfindautocapture);
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");

            var dataBytes = PK_CardCustomerFormatCell2(listExcel, Dictionary["TitleEx"], "Sheet1", "", Dictionary["Title"], fromdate + " - " + todate);

            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = Dictionary["TitleEx"];
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
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

        private byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }

    public class FileResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string fileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public FileResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            fileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}