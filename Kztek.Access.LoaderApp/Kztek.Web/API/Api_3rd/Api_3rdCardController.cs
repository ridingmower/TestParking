using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Service.API;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace Kztek.Web.API.Api_3rd
{
    [RoutePrefix("api/3rd_card")]
    [Authorize]
    public class Api_3rdCardController : ApiBaseController
    {
        private IAPI_tblCardService _tblCard;
        private IAPI_tblCardEventService _tblCardEventService;
        private IAPI_CustomerService _API_CustomerService;
       private IAPI_CardGroupService _API_CardGroupService;
        private ItblCardSubmitEventService _tblLog;



        public Api_3rdCardController(IAPI_CardGroupService _API_CardGroupService, IAPI_tblCardEventService _tblCardEventService, IAPI_tblCardService _tblCard, ItblCardSubmitEventService _tblLog, IAPI_CustomerService _API_CustomerService)
        {
            this._tblCardEventService = _tblCardEventService;
            this._tblCard = _tblCard;
            this._API_CardGroupService = _API_CardGroupService;
            this._API_CustomerService = _API_CustomerService;
            this._tblLog = _tblLog;
        }
        #region Api old version  - dùng cho các công trình cũ đang chạy không sửa
        [Route("GetList")]
        [HttpGet]
        public async Task<tblCard_API_3rd_Out> listCard(string Key, string CardgroupsId, string CustomerId, string CustomerGroupsId, string Active, string Fromdate, string Todate, string PageIndex, string PageSize)
        {
            var TotalItem = 0;
            int TotalPage = 0;
            PageIndex = PageIndex == "0" || String.IsNullOrEmpty(PageIndex) ? "1" : PageIndex;
            PageSize = PageSize == "0" || String.IsNullOrEmpty(PageSize) ? "20" : PageSize;

            // var list = _tblCard.GetAllPaging(key, cardgroupsId, customerId, customergroupsId, fromdate, todate, active, ref total, Convert.ToInt32(PageIndex), Convert.ToInt32(PageSize));
            var list = _tblCard.GetAllPaging(Key, CardgroupsId, CustomerId, CustomerGroupsId, Active, Fromdate, Todate, Convert.ToInt32(PageIndex), Convert.ToInt32(PageSize), ref TotalItem, ref TotalPage).ToList();
            var obj = new tblCard_API_3rd_Out()
            {
                PageIndex = Convert.ToInt32(PageIndex),
                PageSize = Convert.ToInt32(PageSize),
                TotalItem = TotalItem,
                TotalPage = TotalPage,
                Out_data = list
            };

            return await Task.FromResult(obj);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("getbycardnumbercardno")]
        [HttpGet]
        public HttpResponseMessage GetByCardNumberOrCardNo(HttpRequestMessage request, string key)
        {
            bool exactSearch = false;
            var obj = _tblCard.GetCardByCardNumberOrCardNo(key, exactSearch);
            //Trả lại response
            var model = new tblCard_API_3rd();
            if (obj != null)
            {
                var objCus = new tblCustomer();
                if (!String.IsNullOrEmpty(obj.CustomerID))
                    objCus = _API_CustomerService.GetById(obj.CustomerID);

                model.CardId = obj.CardID.ToString();
                model.CardGroupID = obj.CardGroupID;
                model.CardNo = obj.CardNo;
                model.CardNumber = obj.CardNumber;
                model.Plate = obj.Plate1;
                model.VehicleName = obj.VehicleName1;
                model.CustomerId = obj.CustomerID;
                model.CustomerName = objCus != null ? objCus.CustomerName : String.Empty;
                model.CustomerMobile = objCus != null ? objCus.Mobile : String.Empty;
                model.CustomerId = obj.CustomerID;
                model.ExpireDate = obj.ExpireDate;
                model.RegisterDate = obj.DateRegister;
                model.RegisterDate = obj.DateRegister;
                model.Money = "0";
            }


            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        #endregion

        #region Api New version
        /// <summary>
        ///getbycardnumber
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 

        [Route("getbycardnumber")]
        [HttpGet]
        public async Task<tblCard_API_3rd_ByCardNumber> GetByCardNumber(string key)
        {
            var obj = _tblCard.GetByCardNumber_Id(key);
            var model = new tblCard_API_3rd_ByCardNumber();
            if (obj != null)
            {
                model.CardNo = obj.CardNo;
                model.CardNumber = obj.CardNumber;
                model.CardGroupID = obj.CardGroupID;
                model.Plate = obj.Plate1;
                model.VehicleName = obj.VehicleName1;
                model.CustomerID = obj.CustomerID;
                model.ExpireDate = obj.ExpireDate;
                model.RegisterDate = obj.DateRegister;
                model.IsLock = obj.IsLock;
            }

            return await Task.FromResult(obj != null ? model : null);
        }


        [Route("GetAllPaging")]
        [HttpGet]
        public async Task<tblCard_API_3rd_Out> GetAllPaging(string Key, string CardgroupsId, string CustomerGroupsId, string CustomerId, string IsLock, string Fromdate, string Todate, string PageIndex, string PageSize)
        {
            var TotalItem = 0;
            int TotalPage = 0;
            PageIndex = PageIndex == "0" || String.IsNullOrEmpty(PageIndex) ? "1" : PageIndex;
            PageSize = PageSize == "0" || String.IsNullOrEmpty(PageSize) ? "20" : PageSize;

            var list = _tblCard.GetAllPaging_New(Key, CardgroupsId, CustomerId, CustomerGroupsId, IsLock, Fromdate, Todate, Convert.ToInt32(PageIndex), Convert.ToInt32(PageSize), ref TotalItem, ref TotalPage).ToList();
            var obj = new tblCard_API_3rd_Out()
            {
                PageIndex = Convert.ToInt32(PageIndex),
                PageSize = Convert.ToInt32(PageSize),
                TotalItem = TotalItem,
                TotalPage = TotalPage,
                Out_data = list
            };

            return await Task.FromResult(obj);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("getByCardNumberOrCardNo")]
        [HttpGet]
        public HttpResponseMessage GetByCardNumberOrCardNo_New(HttpRequestMessage request, string key)
        {
            bool exactSearch = false;
            var obj = _tblCard.GetCardByCardNumberOrCardNo(key, exactSearch);
            //Trả lại response
            var model = new tblCard_API_3rd();
            if (obj != null)
            {
                var objCus = new tblCustomer();
                if (!String.IsNullOrEmpty(obj.CustomerID))
                    objCus = _API_CustomerService.GetById(obj.CustomerID);

                model.CardId = obj.CardID.ToString();
                model.CardGroupID = obj.CardGroupID;
                model.CardNo = obj.CardNo;
                model.CardNumber = obj.CardNumber;
                model.Plate = obj.Plate1;
                model.VehicleName = obj.VehicleName1;
                model.CustomerId = obj.CustomerID;
                model.CustomerName = objCus != null ? objCus.CustomerName : String.Empty;
                model.CustomerMobile = objCus != null ? objCus.Mobile : String.Empty;
                model.CustomerId = obj.CustomerID;
                model.ExpireDate = obj.ExpireDate;
                model.RegisterDate = obj.DateRegister;
                model.RegisterDate = obj.DateRegister;
                model.Money = "0";
            }


            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj != null ? model : null);
                return response;
            });
        }


        #endregion

        //private void SaveCardProcess(tblCard obj, string plate, string jsonobj, string action, string userid)
        //{
        //    var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID,Plates,OldInfoCP) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}',N'{7}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID, plate, jsonobj);

        //    SqlExQuery<tblCardProcess>.ExcuteNone(str);
        //}


        /// <summary>
        /// Create card
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public Result_tblCardAPI Create([FromBody] tblCard_API_3rd obj)
        {
            var result = new Result_tblCardAPI()
            {
                isSuccess = false,
                Message = "",
                CardNumber = ""
            };

            string Err = String.Empty;
            if ((String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                    || (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                    || String.IsNullOrEmpty(obj.CardGroupID))
            {
                if (String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                {
                    Err = "Vui lòng nhập CardNo";
                }
                if (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                {
                    Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập CardNumber" : ", Vui lòng nhập CardNumber";
                }
                if (String.IsNullOrEmpty(obj.CardGroupID))
                {
                    Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập nhóm thẻ" : ", Vui lòng nhập nhóm thẻ";
                }


            }
            if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày hết hạn" : ", Vui lòng nhập ngày hết hạn";
            }

            if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày đăng ký" : ", Vui lòng nhập ngày đăng ký";
            }

            var existedCard = _tblCard.GetByCardNumber_Id(obj.CardNumber);
            if (existedCard != null)
            {
                Err += String.IsNullOrEmpty(Err) ? " Mã thẻ đã tồn tại" : ", Mã thẻ đã tồn tại";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }

            var model = new tblCard()
            {
                CardID = Guid.NewGuid(),
                CardNo = obj.CardNo,
                CardNumber = obj.CardNumber.Trim(),
                CardGroupID = obj.CardGroupID,
                CustomerID = String.IsNullOrEmpty(obj.CustomerId) ? "" : obj.CustomerId,
                AccessLevelID = "",
                ChkRelease = false,
                ImportDate = DateTime.Now,
                DateRegister = obj.RegisterDate,
                DateRelease = null,
                //ExpireDate = obj.ExpireDate,
                DateActive = DateTime.Now,
                Description = "",
                IsDelete = false,
                IsLock = false,
                Plate1 = obj.Plate,
                Plate2 = String.Empty,
                Plate3 = String.Empty,
                VehicleName1 = obj.VehicleName,
                VehicleName2 = String.Empty,
                VehicleName3 = String.Empty,
                AccessExpireDate = Convert.ToDateTime("2099/12/31"),
                DateCancel = DateTime.Now,
                isAutoCapture = false,
                IsLost = false
            };

            if (Convert.ToDateTime(obj.RegisterDate) > Convert.ToDateTime("1753-1-1"))
                model.DateRegister = Convert.ToDateTime(obj.RegisterDate);

            

            var result_create = _tblCard.Create(model);
            if (result_create.isSuccess)
            {
                result.isSuccess = result_create.isSuccess;
                result.Message = result_create.Message;
                result.CardNumber = obj.CardNumber;

                var re = new MessageReport();
                re.isSuccess = result_create.isSuccess;
                re.Message = result_create.Message;

                _tblCard.SaveCardProcess(model,  "ADD ", "Thêm từ API");

                WriteLog.WriteAPI(re, null, model.CardID.ToString(), model.CardNumber.Trim(), "tblCard", ConstField.ApiParking, ActionConfigO.Create);
            }
            else
            {
                result.isSuccess = result_create.isSuccess;
                result.Message = result_create.Message;
            }
            return result;
        }


        [Route("update")]
        [HttpPut]

        public Result_tblCardAPI Update([FromBody] tblCard_API_3rd obj)
        {
            var result = new Result_tblCardAPI()
            {
                isSuccess = false,
                Message = "",
                CardNumber = ""
            };
            var model1 = _tblCard.GetById(Guid.Parse(obj.CardId));
            var plate1 = model1.Plate1;
            var cardgr = model1.CardGroupID;
            var customerid = model1.CustomerID;


            string Err = String.Empty;
            if ((String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                    || (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                    || String.IsNullOrEmpty(obj.CardGroupID))
            {
                if (String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                {
                    Err = "Vui lòng nhập CardNo";
                }
                if (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                {
                    Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập CardNumber" : ", Vui lòng nhập CardNumber";
                }
                if (String.IsNullOrEmpty(obj.CardGroupID))
                {
                    Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập nhóm thẻ" : ", Vui lòng nhập nhóm thẻ";
                }


            }
            if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày hết hạn" : ", Vui lòng nhập ngày hết hạn";
            }

            if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày đăng ký" : ", Vui lòng nhập ngày đăng ký";
            }
            var oldObj = _tblCard.GetCustomById(Guid.Parse( obj.CardId));
            var model = new tblCard();
             model = _tblCard.GetByCardNumber_Id(obj.CardNumber);
            if (model != null && model.CardNumber != obj.CardNumber)
            {
                if (model.CardNumber != obj.CardNumber)
                    Err += String.IsNullOrEmpty(Err) ? " Mã thẻ đã tồn tại" : ", Mã thẻ đã tồn tại";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }

            var oldIslock = model.IsLock;

            var olddate = DateTime.Now.Date;

            if (model != null)
            {
                //Lấy thông tin ngày hết hạn cũ
                olddate = Convert.ToDateTime(model.ExpireDate);

                model.CardNo = obj.CardNo;
                model.CardNumber = obj.CardNumber.Trim();
                model.CardGroupID = obj.CardGroupID;
                model.CustomerID = String.IsNullOrEmpty(obj.CustomerId) ? "" : obj.CustomerId;
                model.AccessLevelID = "";
                model.ChkRelease = false;
                model.ImportDate = DateTime.Now;
                model.DateRegister = obj.RegisterDate;
                model.DateRelease = null;
                model.ExpireDate = obj.ExpireDate;
                model.DateActive = DateTime.Now;
                model.Description = String.Empty;
                model.IsDelete = false;
                model.IsLock = obj.IsLock;
                model.Plate1 = obj.Plate;
                model.Plate2 = String.Empty;
                model.Plate3 = String.Empty;
                model.VehicleName1 = obj.VehicleName;
                model.VehicleName2 = String.Empty;
                model.VehicleName3 = String.Empty;
                model.AccessExpireDate = Convert.ToDateTime("2099/12/31");
                model.DateCancel = DateTime.Now;
                model.isAutoCapture = false;
            }

            if (Convert.ToDateTime(obj.ExpireDate) > Convert.ToDateTime("1753-1-1"))
                model.ExpireDate = Convert.ToDateTime(obj.ExpireDate);

            var result_update = _tblCard.Update(model);

            if (result_update.isSuccess)
            {
                if (olddate != model.ExpireDate)
                {
                    int mo = 0;
                    bool chbEnableDateActive = false;

                    //nếu có tiền
                    if (!string.IsNullOrEmpty(obj.Money))
                    {
                        //kiểm tra xem có phải kiểu số không
                        bool isNumeric = int.TryParse(obj.Money, out mo);

                        //nếu không thì tiền  = 0
                        if (!isNumeric)
                        {
                            mo = 0;
                        }
                    }

                    if (olddate != model.ExpireDate && model.ExpireDate > Convert.ToDateTime("1753-1-1"))
                    {

                        //nếu ngày cũ lớn hơn ngày mới là gia hạn âm ngày
                        if (olddate.Date > Convert.ToDateTime(model.ExpireDate).Date)
                        {
                            chbEnableDateActive = true;
                        }

                        var a = _tblCard.AddNewActiveCard("'" + model.CardNumber + "'", mo, olddate.ToString("yyyy/MM/dd"), Convert.ToDateTime(model.ExpireDate).ToString("yyyy/MM/dd"), "Gia hạn từ API", chbEnableDateActive);
                    }

                    
                }
                if (oldIslock != obj.IsLock)
                {
                    var userCard = GetCurrentUser.GetUser();
                    string action = "UNLOCK";
                    if (obj.IsLock)
                        action = "LOCK";
                    _tblCard.SaveCardProcessExpend(model, "", "", action, userCard.Id);
                }
                var grCardOld = new tblCardGroup();
                var grCardNew = new tblCardGroup();
                var s = oldObj.CardGroupID;
                if (!oldObj.CardGroupID.Equals(""))
                {
                    Guid ids = Guid.Parse(oldObj.CardGroupID);
                    grCardOld = _API_CardGroupService.GetById(ids);
                }
               
                var objCusOld = _API_CustomerService.GetById(model1.CustomerID);
                var jsStrOld = (model1.Plate1 != null ?  "Biển số :" +  model1.Plate1 : "") + ( grCardOld.CardGroupName != "" ?  " ,Nhóm thẻ : " + grCardOld.CardGroupName : "") + (objCusOld.CustomerName != "" ? ",Tên KH : " + objCusOld.CustomerName : "") + ( objCusOld.Mobile != null ? ",SĐT : " + objCusOld.Mobile : "") + (objCusOld.CustomerCode != "" ? ",Mã KH : " + objCusOld.CustomerCode : "");

                if (  obj.Plate != plate1  && ( obj.CardGroupID != cardgr || obj.CustomerId != customerid ))
                {
                    _tblCard.SaveCardProcessExpend(model, obj.Plate, jsStrOld, "UPDATEINFO", "Thêm từ API");
                }
                else if ((obj.CardGroupID == cardgr && obj.CustomerId == customerid )&& obj.Plate != plate1)
                {
                    _tblCard.SaveCardProcessExpend(model, obj.Plate, jsStrOld, "CHANGEPLA", "Thêm từ API");
                }
                else if (obj.CardGroupID != cardgr || obj.CustomerId != customerid &&( obj.Plate ==plate1))
                {
                    _tblCard.SaveCardProcessExpend(model, obj.Plate, jsStrOld, "UPDATEINFO", "Thêm từ API");
                }
              
            }

            //Trả lại response
            if (result_update.isSuccess)
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
                result.CardNumber = obj.CardNumber;

                var re = new MessageReport();
                re.isSuccess = result.isSuccess;
                re.Message = result.Message;

                WriteLog.WriteAPI(re, null, model.CardID.ToString(), model.CardNumber.Trim(), "tblCard", ConstField.ApiParking, ActionConfigO.Update);
            }
            else
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
            }
            return result;
        }


        [Route("delete")]
        [HttpDelete]
        public MessageReport DeleteById(string cardNumber)
        {
            var obj = _tblCard.GetByCardNumber_Id(cardNumber);
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ không tồn tại hoặc đã bị xóa trong hệ thống";
                result1.isSuccess = false;
                return result1;
            }
            if (obj.IsDelete)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ đã bị xóa";
                result1.isSuccess = false;
                return result1;
            }

            var existedInEvent = _tblCardEventService.GetAllByCardNumber(obj.CardNumber);
            if (existedInEvent.Any())
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ đang tồn tại trong sự kiện. Không thể xóa";
                result1.isSuccess = false;
                return result1;
            }

            var result = _tblCard.DeleteById(obj);

            //lưu log
            var re = new MessageReport();
            re.isSuccess = result.isSuccess;
            re.Message = result.Message;
            //Lưu cardprocess
            _tblCard.SaveCardProcess(obj, "DELETE", "Xóa từ API");
            WriteLog.WriteAPI(re, null, obj.CardID.ToString(), obj.CardNumber.Trim(), "tblCard", ConstField.ApiParking, ActionConfigO.Delete);
            return result;
        }


        private List<string> GetListActionType(tblCardSubmit_API obj)
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

            


            //Phát thẻ
            if (obj.isReleaseCard)
            {
                str.Add("RELEASE");//11
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
            //Cập nhật thông tin
            if (obj.OldCustomerName != obj.CustomerName || obj.OldCustomerMobile != obj.CustomerMobile || obj.OldCustomerCode != obj.CustomerCode || obj.OldCardGroupID != obj.CardGroupID && (obj.Plate1 == obj.OldPlate1 || obj.Plate2 == obj.OldPlate2 || obj.Plate3 != obj.OldPlate3))
            {
                //Đổi thông tin thẻ
                str.Add("UPDATEINFO");//10
            }
            else if (obj.OldCustomerName != obj.CustomerName || obj.OldCustomerMobile != obj.CustomerMobile || obj.OldCustomerCode != obj.CustomerCode || obj.OldCardGroupID != obj.CardGroupID && (obj.Plate1 != obj.OldPlate1 || obj.Plate2 != obj.OldPlate2 || obj.Plate3 != obj.OldPlate3))
            {
                //Đổi thông tin thẻ
                str.Add("UPDATEINFO");//10
            }
            else if (obj.Plate1 != obj.OldPlate1 || obj.Plate2 != obj.OldPlate2 || obj.Plate3 != obj.OldPlate3 && (obj.OldCustomerName == obj.CustomerName || obj.OldCustomerMobile == obj.CustomerMobile || obj.OldCustomerCode == obj.CustomerCode || obj.OldCardGroupID == obj.CardGroupID))
            {
                //Đổi biển số
                str.Add("CHANGEPLA");//10
            }



            return str;
        }
    }
}
