using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Service.API;
using Kztek.Service.Payment;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Functions
{
    [RoutePrefix("api/payment")]
    public class API_OnlinePaymentController : ApiController
    {
        private ItblEventPaymentService _tblEventPaymentService;
        private ItblPCService _tblPCService;
        private ItblLaneService _tblLaneService;
        //private ItblCardEventService _tblCardEventService;


        private IAPI_CardViettelService _API_CardViettelService;

        public API_OnlinePaymentController(ItblEventPaymentService _tblEventPaymentService, ItblPCService _tblPCService, ItblLaneService _tblLaneService, IAPI_CardViettelService _API_CardViettelService)
        {
            this._tblEventPaymentService = _tblEventPaymentService;
            this._tblPCService = _tblPCService;
            this._tblLaneService = _tblLaneService;
            this._API_CardViettelService = _API_CardViettelService;
            //this._tblCardEventService = _tblCardEventService;
        }

        [Route("servertime")]
        [HttpGet]
        public async Task<string> ServerTime()
        {
            return await Task.FromResult(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<API_QRCodeResponse> CheckOut(API_QRCodeRequest model)
        {
            var result = new API_QRCodeResponse();

            var uri = "http://203.190.173.21:8008/payment-management/payment/qrCode";

            var postModel = new
            {
                parkingId = 1034,
                parkingCode = "DN_NVL",
                startTime = model.TimeIn,
                endTime = model.TimeOut,
                numberPlate = model.Plate,
                totalAmount = model.Money,
                reqPaymentId = model.EventId
            };

            try
            {
                var response = await ApiHelper.HttpPost(uri, postModel, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

                var _model = await ApiHelper.ConvertResponse<API_QRCodeResponseModel>(response);

                result = _model.qrCodeData;

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ParkingHub>();

                // Notify clients in the group
                //var currentEvent = _tblCardEventService.GetById(model.EventId);

                //var connectedIds = ParkingHub.ConnectedIds.Where(p => p.Value == $"{currentEvent.LaneIDOut}");
                foreach (var item in ParkingHub.ConnectedIds)
                {
                    hubContext.Clients.Client(item.Key).displayQRCodeClient(_model.qrCodeData);
                }

                Log_Payment(_model, model.EventId, model.Plate, model.TimeIn, model.TimeOut);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        [Route("checkstatus")]
        [HttpGet]
        public async Task<API_QRCodeCheckResponse> CheckStatus(string eventid, string orderid)
        {
            var result = new API_QRCodeCheckResponse();

            var uri = "http://203.190.173.21:8008/payment-management/payment/qrCode/status";

            var postModel = new
            {
                parkingId = 1034,
                reqPaymentId = eventid,
                orderId = orderid
            };

            uri = $"{uri}?parkingId={1034}&reqPaymentId={eventid}";

            try
            {
                var response = await ApiHelper.HttpGet(uri, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

                result = await ApiHelper.ConvertResponse<API_QRCodeCheckResponse>(response);

                //File.WriteAllText("d:/checkresult.txt", JsonConvert.SerializeObject(result));
                Log_PaymentStatus(result, eventid);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        [Route("getlistpc")]
        [HttpGet]
        public async Task<List<PcResponse>> GetListPc()
        {
            var result = new List<PcResponse>() { new PcResponse { Id = "", Name = "--Lựa chọn--" } };

            var listPc = _tblPCService.GetAllActive().Select(p => new PcResponse
            {
                Id = p.PCID.ToString(),
                Name = p.ComputerName + $"({p.IPAddress})"
            }).ToList();

            result.AddRange(listPc);

            return await Task.FromResult(result);
        }

        [Route("getlanebypcid/{pcid}")]
        [HttpGet]
        public async Task<List<LaneResponse>> GetLaneByPcId(string pcid)
        {
            var result = new List<LaneResponse>() { new LaneResponse { Id = "", Name = "--Lựa chọn--" } };

            var listLane = _tblLaneService.GetAll().Where(p => p.PCID == pcid)
                .Select(p => new LaneResponse
                {
                    Id = p.LaneID.ToString(),
                    Name = p.LaneName
                }).ToList();

            result.AddRange(listLane);

            return await Task.FromResult(result);
        }

        [Route("saveconnection")]
        [HttpPost]
        public async Task<MessageReport> SaveConnection(API_SignalR_Connection model)
        {
            var report = new MessageReport() { isSuccess = false };
            try
            {
                // Add to the global dictionary of connected ids
                ParkingHub.ConnectedIds.TryAdd(model.ConnectionID, model.LaneID);

                report.isSuccess = true;
                report.Message = "Connection Success";
            }
            catch (Exception ex)
            {
                report.Message = ex.Message;
            }

            return await Task.FromResult(report);
        }

        [Route("bookin")]
        [HttpPost]
        public async Task<API_BookInResponse> BookIn(API_BookInRequest model)
        {
            var result = new API_BookInResponse();

            var uri = "http://203.190.173.21:8080/reservation-management/offStreet/booking/qrcode";

            var postModel = new
            {
                userId = model.CardNumber,
                phone = "0123456789",
                parkingId = 1034,
                parkingCode = "DN_NVL",
                startTime = model.TimeIn,
                numberPlate = model.Plate,
                reqPaymentId = model.EventId
            };

            try
            {
                var response = await ApiHelper.HttpPost(uri, postModel, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

                result = await ApiHelper.ConvertResponse<API_BookInResponse>(response);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        [Route("bookout")]
        [HttpPost]
        public async Task<API_BookOutResponse> BookOut(API_BookOutRequest model)
        {
            var result = new API_BookOutResponse();

            var uri = "http://203.190.173.21:8008/payment-management/payment/qrCode";

            var postModel = new
            {
                parkingId = 1034,
                parkingCode = "DN_NVL",
                startTime = model.TimeIn,
                endTime = model.TimeOut,
                numberPlate = model.Plate,
                totalAmount = model.Money,
                reqPaymentId = model.EventId
            };

            try
            {
                var response = await ApiHelper.HttpPost(uri, postModel, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

                result = await ApiHelper.ConvertResponse<API_BookOutResponse>(response);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private async Task<MessageReport> Log_Payment(API_QRCodeResponseModel model, string eventid, string plate, string timein, string timeout)
        {
            return await _tblEventPaymentService.Create(model, eventid, timein, timeout, plate);
        }

        private async Task<MessageReport> Log_PaymentStatus(API_QRCodeCheckResponse model, string eventid)
        {
            return await _tblEventPaymentService.Update(model, eventid);
        }

        [Route("synccarddata")]
        [HttpGet]
        public async Task<MessageReport> SyncCardData()
        {
            var result = new MessageReport(false, "error");
            var isNewCard = false;
            var isNewCustomer = false;

            try
            {
                //Data from viettel
                var data = await DataFromViettel();

                if (data.statusCode != 200)
                {
                    result = new MessageReport(false, "Failed to get data from Viettel");
                    return result;
                }

                foreach (var item in data.results)
                {
                    try
                    {
                        //cardnumber empty => add viettel id default
                        if (string.IsNullOrWhiteSpace(item.cardNumber))
                        {
                            item.cardNumber = "CardNumber_" + item.id.ToString();
                        }

                        //cardno empty => add viettel id default
                        if (string.IsNullOrWhiteSpace(item.cardNo))
                        {
                            item.cardNo = "CardNo_" + item.id.ToString();
                        }

                        //Get data cardgroup
                        var cardGroupId = GetCardGroupId(item.cardGroup);

                        //Get data customergroup
                        var customerGroupId = GetCustomerGroupId(item.groupUser);

                        //Custom card
                        var cardcustom = GetOrSetCardCustom(item.cardNumber);

                        //Custom customer
                        var customercustom = _API_CardViettelService.GetCustomCustomerByCode(item.userCode);

                        //Thẻ
                        if (cardcustom != null)
                        {
                            cardcustom.ViettelId = item.id.ToString();
                            cardcustom.CardNo = item.cardNo;
                            cardcustom.CardInActive = item.status == 1 ? false : true;
                            cardcustom.CardGroupID = cardGroupId;
                            cardcustom.ViettelType = item.type;
                        }
                        else
                        {
                            //
                            isNewCard = true;

                            //
                            cardcustom = new tblCardSubmit();
                            cardcustom.CardID = Guid.NewGuid().ToString();
                            cardcustom.ViettelId = item.id.ToString();
                            cardcustom.CardNo = item.cardNo;
                            cardcustom.CardNumber = item.cardNumber;
                            cardcustom.CardGroupID = cardGroupId;
                            cardcustom.CardInActive = item.status == 1 ? false : true;
                            cardcustom.DtpDateExpired = Convert.ToDateTime(item.endDate).ToString("dd/MM/yyyy HH:mm:ss");
                            cardcustom.ViettelType = item.type;
                        }

                        //Add vehicle plate
                        cardcustom.Plate1 = item.numberPlate;
                        cardcustom.VehicleName1 = item.numberPlateName;

                        //Khách hàng
                        if (customercustom != null)
                        {
                            customercustom.Address = item.address;
                            customercustom.IDNumber = item.cmnd;
                            customercustom.Mobile = item.phoneNumber;
                            customercustom.CustomerName = item.userName;
                            customercustom.CustomerGroupID = customerGroupId;
                        }
                        else
                        {
                            //
                            isNewCustomer = true;

                            //
                            customercustom = new tblCustomerSubmit();
                            customercustom.CustomerID = Guid.NewGuid().ToString();
                            customercustom.CustomerCode = item.userCode;
                            customercustom.Address = item.address;
                            customercustom.IDNumber = item.cmnd;
                            customercustom.Mobile = item.phoneNumber;
                            customercustom.CustomerName = item.userName;
                            customercustom.CustomerGroupID = customerGroupId;
                        }

                        //Gán khách hàng vào thẻ
                        cardcustom.CustomerID = customercustom.CustomerID.ToString();

                        //Save
                        SetCard_Customer(cardcustom, customercustom, isNewCard, isNewCustomer);

                        var oldexpire = cardcustom.DtpDateExpired;
                        var newexpire = Convert.ToDateTime(item.endDate).ToString("dd/MM/yyyy HH:mm:ss");

                        if (oldexpire != newexpire)
                        {
                            SaveCardExtendProcess(cardcustom, Convert.ToDateTime(item.endDate).ToString("dd/MM/yyyy HH:mm:ss"), "api");
                        }

                        //Còn lại
                        var listAction = GetListActionType(cardcustom);
                        if (listAction.Any())
                        {
                            foreach (var itemAc in listAction)
                            {
                                SaveCardProcess(cardcustom, itemAc, "api");
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                result = new MessageReport(true, "Sync completed");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        private async Task<tblCardViettel> DataFromViettel()
        {
            try
            {
                var uri = ConfigurationManager.AppSettings["Viettel_Host"] + "parking-management/monthly/ticket/syc";

                var postModel = new
                {
                    parkingId = Convert.ToInt32(ConfigurationManager.AppSettings["Viettel_ParkingId"]),
                    parkingCode = ConfigurationManager.AppSettings["Viettel_ParkingCode"],
                    tokenCode = ConfigurationManager.AppSettings["Viettel_TokenCode"]
                };

                //Get data from api viettel
                var response = await ApiHelper.HttpPost(uri, postModel, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

                var result = await ApiHelper.ConvertResponse<tblCardViettel>(response);

                return result;
            }
            catch
            {
                return null;
            }
        }

        #region Support for syncing data
        private string GetCardGroupId(string name)
        {
            var obj = _API_CardViettelService.GetCardGroupByName(name);
            if (obj != null)
            {
                return obj.CardGroupID.ToString();
            }

            return "";
        }

        private string GetCustomerGroupId(string name)
        {
            var obj = _API_CardViettelService.GetCustomerGroupByName(name);
            if (obj != null)
            {
                return obj.CustomerGroupID.ToString();
            }

            return "";
        }

        private tblCardSubmit GetOrSetCardCustom(string cardnumber)
        {
            var card = _API_CardViettelService.GetCardByCardNumber(cardnumber);
            if (card != null)
            {
                var obj = _API_CardViettelService.GetCustomCardById(card.CardID);

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
                str.AppendLine("INSERT INTO [dbo].[tblCard]([CardNo], [CardNumber], [CustomerID], [CardGroupID], [ImportDate], [ExpireDate], [IsLock], [IsDelete], [Plate1], [VehicleName1], [Plate2], [VehicleName2], [Plate3], [VehicleName3], [ViettelId], [ViettelType])");

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
                str.AppendLine(string.Format(", '{0}'", cardsubmit.ViettelId));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.ViettelType));

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

                if (!string.IsNullOrWhiteSpace(cardsubmit.ViettelId))
                    str.AppendLine(string.Format(",[ViettelId] = '{0}'", cardsubmit.ViettelId));

                if (!string.IsNullOrWhiteSpace(cardsubmit.ViettelType))
                    str.AppendLine(string.Format(",[ViettelType] = '{0}'", cardsubmit.ViettelType));

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

            //if (obj.OldDtpDateActive != obj.DtpDateActive)
            //{
            //    obj.isChangeActiveCard = true;
            //}

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
            //if (obj.isChangeActiveCard)
            //{
            //    str.Add("ACTIVE");
            //}

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

        private void SaveCardProcess(tblCardSubmit obj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }
        #endregion
    }
}