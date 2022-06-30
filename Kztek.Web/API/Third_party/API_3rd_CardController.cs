using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Service.API;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Third_party
{
    [RoutePrefix("api/3rd/card")]
    public class API_3rd_CardControllerController : ApiController
    {
        private IAPI_CardGroupService _API_CardGroupService;
        private IAPI_tblCardEventService _tblCardEventService;
        private IAPI_tblCardService _tblCard;
        private ItblCardSubmitEventService _tblLog;
        private IAPI_CustomerService _tblCustomer;
        private IAPI_tblCustomerGroupService _tblCustomerGroup;


        public API_3rd_CardControllerController(IAPI_CardGroupService _API_CardGroupService, IAPI_tblCardEventService _tblCardEventService,
            IAPI_tblCardService _tblCard, ItblCardSubmitEventService _tblLog, IAPI_CustomerService _tblCustomer, IAPI_tblCustomerGroupService _tblCustomerGroup)
        {
            this._API_CardGroupService = _API_CardGroupService;
            this._tblCardEventService = _tblCardEventService;
            this._tblCard = _tblCard;
            this._tblLog = _tblLog;
            this._tblCustomer = _tblCustomer;
            this._tblCustomerGroup = _tblCustomerGroup;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("getbycardnumbercardno")]
        [HttpGet]
        [API_BasicAuthenticationAttribute]
        public async Task<tblCard_API> GetByCardNumberOrCardNo(HttpRequestMessage request, string key)
        {
            var obj = _tblCard.GetCardByCardNumberOrCardNo(key,false);
            //Trả lại response
            var model = new tblCard_API()
            {
                CardNo = obj.CardNo,
                CardNumber = obj.CardNumber,
                Plate = obj.Plate1,
                VehicleName = obj.VehicleName1,
                //CustomerId = obj.CustomerID,
                ExpireDate = obj.ExpireDate,
                RegisterDate = obj.DateRegister
            };

            return await Task.FromResult(model);
        }

        ///// <summary>
        ///// Create card
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("Create")]
        //[HttpPost]
        //public HttpResponseMessage Create(HttpRequestMessage request, tblCard_API obj)
        //{
        //    var result = new MessageReport(false, "Có lỗi xảy ra");
        //    string Err = String.Empty;
        //    if ((String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
        //            || (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
        //            || String.IsNullOrEmpty(obj.CardGroupID))
        //    {
        //        if (String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
        //        {
        //            Err = "Vui lòng nhập CardNo";
        //        }
        //        if (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
        //        {
        //            Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập CardNumber" : ", Vui lòng nhập CardNumber";
        //        }
        //        if (String.IsNullOrEmpty(obj.CardGroupID))
        //        {
        //            Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập nhóm thẻ" : ", Vui lòng nhập nhóm thẻ";
        //        }


        //    }
        //    if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
        //    {
        //        Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày hết hạn" : ", Vui lòng nhập ngày hết hạn";
        //    }

        //    if (String.IsNullOrEmpty(obj.ExpireDate.ToString()))
        //    {
        //        Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày đăng ký" : ", Vui lòng nhập ngày đăng ký";
        //    }

        //    var existedCard = _tblCard.GetByCardNumber_Id(obj.CardNumber);
        //    if (existedCard != null)
        //    {
        //        Err += String.IsNullOrEmpty(Err) ? " Mã thẻ đã tồn tại" : ", Mã thẻ đã tồn tại";
        //    }

        //    if (!String.IsNullOrEmpty(Err))
        //    {
        //        return CreateHttpResponse(request, () =>
        //        {
        //            result.isSuccess = false;
        //            result.Message = Err;
        //            var response = request.CreateResponse(result.isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        //            return response;
        //        });
        //    }

        //    var model = new tblCard()
        //    {
        //        CardID = Guid.NewGuid(),
        //        CardNo = obj.CardNo,
        //        CardNumber = obj.CardNumber.Trim(),
        //        CardGroupID = obj.CardGroupID,
        //        //CustomerID = String.IsNullOrEmpty(obj.CustomerId) ? "" : obj.CustomerId,
        //        AccessLevelID = "",
        //        ChkRelease = false,
        //        ImportDate = DateTime.Now,
        //        DateRegister = obj.RegisterDate,
        //        DateRelease = null,
        //        ExpireDate = obj.ExpireDate,
        //        DateActive = DateTime.Now,
        //        Description = "",
        //        IsDelete = false,
        //        IsLock = false,
        //        Plate1 = obj.Plate,
        //        Plate2 = String.Empty,
        //        Plate3 = String.Empty,
        //        VehicleName1 = obj.VehicleName,
        //        VehicleName2 = String.Empty,
        //        VehicleName3 = String.Empty,
        //        AccessExpireDate = Convert.ToDateTime("2099/12/31"),
        //        DateCancel = DateTime.Now,
        //        isAutoCapture = false,
        //        IsLost = false
        //    };

        //    result = _tblCard.Create(model);

        //    //lưu log
        //    var objLog = new tblCardSubmitEvent()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        CardId = model.CardID.ToString(),
        //        CardNo = model.CardNo,
        //        CardNumberBefore = "",//Đợi KH
        //        CardNumberSave = obj.CardNumber,
        //        CardGroupId = obj.CardGroupID,
        //        Plate = obj.Plate,
        //        VehicleName = obj.VehicleName,
        //        DateExpired = obj.ExpireDate,
        //        DateRegisted = obj.RegisterDate,
        //        SubmitMessage = result.Message,
        //        SubmitStatus = result.isSuccess ? 1 : 0,
        //        HTTP = "POST",
        //        DateCreated = DateTime.UtcNow
        //    };
        //    _tblLog.Create(objLog);

        //    //Trả lại response
        //    return CreateHttpResponse(request, () =>
        //    {
        //        var response = request.CreateResponse(result.isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        //        return response;
        //    });
        //}

        [API_BasicAuthenticationAttribute]
        [Route("update")]
        [HttpPost]
        public async Task<MessageReport> Update(tblCard_API obj)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            #region valid
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

            if (String.IsNullOrEmpty(obj.RegisterDate.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập ngày đăng ký" : ", Vui lòng nhập ngày đăng ký";
            }

            if (String.IsNullOrEmpty(obj.CustomerName))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập tên khách hàng" : ", Vui lòng nhập tên khách hàng";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.isSuccess = false;
                result.Message = Err;
                return await Task.FromResult(result);
            }
            #endregion

            var objCustomer = _tblCustomer.GetByNameOrAdd(obj.CustomerName, obj.CustomerAdd);
            string customerID = String.Empty;
            if (objCustomer == null)
            {
                //check CustomerGroup
                var CustomerGroupObj = new tblCustomerGroup();
                CustomerGroupObj = _tblCustomerGroup.GetByName("Nhóm khách hàng Api");
                if (CustomerGroupObj == null)
                {
                    var objCusGrp = new tblCustomerGroup()
                    {
                        CustomerGroupID = Guid.NewGuid(),
                        CustomerGroupName = "Nhóm khách hàng Api",
                        ParentID = "0",
                        CustomerGroupCode = null,
                        Description = "Nhóm khách hàng tạo từ Api",
                        Inactive = false,
                        SortOrder = _tblCustomerGroup.GetAll().Max(n => n.SortOrder) + 1,
                        Ordering = _tblCustomerGroup.GetAll().Max(n => n.Ordering) + 1,
                        IsCompany = false,
                        Tax = null
                    };

                    var resultCusRgp = _tblCustomerGroup.Create(objCusGrp);
                    if (resultCusRgp.isSuccess == false)
                    {
                        result.isSuccess = false;
                        result.Message = "tạo mới nhóm khách hàng không thành công vui lòng kiểm tra lại";
                        return await Task.FromResult(result);
                    }
                    else
                    {
                        CustomerGroupObj = objCusGrp;
                    }
                }
                //create customer
                var objCus = new tblCustomer()
                {
                    CustomerID = Guid.NewGuid(),
                    CustomerCode = "Api_" + Guid.NewGuid().ToString(),
                    CustomerName = obj.CustomerName,
                    Address = obj.CustomerAdd,
                    IDNumber = null,
                    Mobile = null,
                    CustomerGroupID = CustomerGroupObj.CustomerGroupID.ToString(),
                    Description = "Khách hàng tạo từ Api",
                    EnableAccount = false,
                    Account = null,
                    Password = null,
                    Avatar = null,
                    Inactive = false,
                    SortOrder = _tblCustomer.GetAll().Max(n => n.SortOrder) + 1,
                    CompartmentId = null,
                    AccessLevelID = "",
                    Finger1 = "",
                    Finger2 = "",
                    DevPass = "",
                    UserIDofFinger = 0,
                    AccessExpireDate = Convert.ToDateTime("2099/12/31"),
                    ContractStartDate = null,
                    ContractEndDate = null,
                    AddressUnsign = null,
                };
                var resultCus = _tblCustomer.Create(objCus);
                if (resultCus.isSuccess == false)
                {
                    result.isSuccess = false;
                    result.Message = "Tạo mới khách hàng không thành công vui lòng kiểm tra lại";
                    return await Task.FromResult(result);
                }
                else
                {
                    customerID = objCus.CustomerID.ToString();
                }

            }
            else
            {
                customerID = objCustomer.CustomerID.ToString();
            }

            #region update create card
            var http = String.Empty;
            string cardId = String.Empty;
            //if (String.IsNullOrWhiteSpace(obj.CardNumber))
            //{
            //    result = create_card(obj, cardId);
            //    if (result.isSuccess == true)
            //    {
            //        cardId = cardId;
            //        http = "POST";
            //    }
            //}
            //else
            //{
                var model = _tblCard.GetByCardNumber_Id(obj.CardNumber);
                if (model != null) //update
                {
                    if (model.CardNumber != obj.CardNumber)
                        Err += String.IsNullOrEmpty(Err) ? " Mã thẻ đã tồn tại" : ", Mã thẻ đã tồn tại";
                    else
                    {
                        model.CardNo = obj.CardNo;
                        model.CardNumber = obj.CardNumber.Trim();
                        model.CardGroupID = obj.CardGroupID;
                        model.CustomerID = customerID;//objCustomer.CustomerID;
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
                        model.isAutoCapture = obj.isAutoCapture;
                        result = _tblCard.Update(model);
                        http = "PUT";
                        cardId = model.CardID.ToString();
                    }
                }
                else
                {
                obj.CustomerId = customerID;
                    result = create_card(obj, cardId);
                    if (result.isSuccess == true)
                    {
                        cardId = cardId;
                        http = "POST";
                    }

                }
            //}
            #endregion

            // Create Log
            var objLog = new tblCardSubmitEvent()
            {
                Id = Guid.NewGuid().ToString(),
                CardId = cardId,
                CardNo = obj.CardNo,
                CardNumberBefore = null,//Đợi KH
                CardNumberSave = obj.CardNumber,
                CardGroupId = obj.CardGroupID,
                Plate = obj.Plate,
                VehicleName = obj.VehicleName,
                DateExpired = obj.ExpireDate,
                DateRegisted = obj.RegisterDate,
                SubmitMessage = result.Message,
                SubmitStatus = result.isSuccess ? 1 : 0,
                HTTP = http,
                DateCreated = DateTime.UtcNow
            };

            _tblLog.Create(objLog);
            //Trả lại response
            return await Task.FromResult(result);
        }

        /// <summary>
        /// create card
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        /// 
        private MessageReport create_card(tblCard_API obj, string cardId)
        {
            var modelCreate = new tblCard()
            {
                CardID = Guid.NewGuid(),
                CardNo = obj.CardNo,
                CardNumber = obj.CardNumber.Trim(),
                CardGroupID = obj.CardGroupID,
                CustomerID = obj.CustomerId,
                AccessLevelID = "",
                ChkRelease = false,
                ImportDate = DateTime.Now,
                DateRegister = obj.RegisterDate,
                DateRelease = null,
                ExpireDate = obj.ExpireDate,
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
                isAutoCapture = obj.isAutoCapture,
                IsLost = false
            };

            var result = _tblCard.Create(modelCreate);
            cardId = modelCreate.CardID.ToString();
            return result;
        }

        [Route("delete")]
        [HttpDelete]
        [API_BasicAuthenticationAttribute]
        public async Task<MessageReport> DeleteById(string CardNumber)
        {
            if (String.IsNullOrWhiteSpace(CardNumber))
            {
                var result1 = new MessageReport();
                result1.Message = "Vui lòng nhập mã thẻ cần xóa";
                result1.isSuccess = false;
                return await Task.FromResult(result1);
            }

            var obj = _tblCard.GetByCardNumber_Id(CardNumber);
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ không tồn tại trong hệ thống";
                result1.isSuccess = false;
                return await Task.FromResult(result1);
            }

            var existedInEvent = _tblCardEventService.CheckCardInEvent(obj.CardNumber);
            if (existedInEvent)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ đang tồn tại trong sự kiện. Không thể xóa";
                result1.isSuccess = false;
                return await Task.FromResult(result1);
            }

            var result = _tblCard.DeleteById(obj);

            var objLog = new tblCardSubmitEvent()
            {
                Id = Guid.NewGuid().ToString(),
                CardId = obj.CardID.ToString(),
                CardNo = obj.CardNo,
                CardNumberBefore = null,
                CardNumberSave = CardNumber,
                CardGroupId = obj.CardGroupID,
                Plate = obj.Plate1,
                VehicleName = obj.VehicleName1,
                DateExpired = obj.ExpireDate,
                DateRegisted = obj.DateRegister,
                SubmitMessage = result.Message,
                SubmitStatus = result.isSuccess ? 1 : 0,
                HTTP = "DELETE",
                DateCreated = DateTime.UtcNow
            };
            _tblLog.Create(objLog);

            return await Task.FromResult(result);
        }
    }
}
