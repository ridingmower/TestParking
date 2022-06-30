using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Service.Admin;
using Kztek.Service.API;
using Kztek.Service.Mobile;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kztek.Web.API.Functions
{
    [RoutePrefix("api/api_card")]
    public class API_CardController : ApiBaseController
    {
        private IAPI_CardService _API_CardService;
        private ItblCardService _tblCardService;
        private IAPI_AuthService _API_AuthService;

        public API_CardController(IAPI_CardService _API_CardService, ItblCardService _tblCardService, IAPI_AuthService _API_AuthService)
        {
            this._API_CardService = _API_CardService;
            this._tblCardService = _tblCardService;
            this._API_AuthService = _API_AuthService;
        }

        /// <summary>
        /// Lấy dữ liệu theo biển số
        /// </summary>
        /// Author      Date        Note
        /// TrungNQ     11/03/2019  Thêm mới
        /// <param name="request"></param>
        /// <param name="data">Danh sách biển số</param>
        /// <returns></returns>
        [Route("getcardbyplates")]
        [HttpPost]
        public HttpResponseMessage GetCardByPlates(HttpRequestMessage request, PMC_Customer_Company_Plate model)
        {
            //Check auth
            //var obj = _API_AuthService.GetDefault();
            //if (true)
            //{

            //}

            //Danh sách
            var list = _API_CardService.GetListActiveCardByPlates(model.Plates);

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, list);
                return response;
            });
        }

        /// <summary>
        /// Gia hạn khi member click chấp nhận 
        /// </summary>
        /// Author      Date        Note
        /// TrungNQ     14/03/2019  Thêm mới
        /// <param name="request"></param>
        /// <param name="result">{ CardNumber, DayExtend, Money.. }</param>
        /// <returns></returns>
        [Route("extendcardbyorder")]
        [HttpPost]
        public HttpResponseMessage ExtendCardByOrder(HttpRequestMessage request, PMC_CustomerOrder model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            //Check Auth
            var objAuth = _API_AuthService.GetDefault();
            if (objAuth != null)
            {
                if (objAuth.AccessToken != model.AccessToken)
                {
                    result = new MessageReport(false, "Token không khớp");

                    //Trả lại response
                    return CreateHttpResponse(request, () =>
                    {
                        var response = request.CreateResponse(result.isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
                        return response;
                    });
                }
            }

            //Xử lý logic gia hạn
            var obj = _tblCardService.GetByCardNumber(model.CardNumber);

            if(obj != null)
            {
                string dateextend = Convert.ToDateTime(obj.ExpireDate).AddDays(model.DayExtend).ToString("MM/dd/yyyy");
                var money = model.Money.ToString().Replace(".", "").Replace(",", "");
                
                var isSuccess = _tblCardService.AddCardExpireByListCardNumber("'" + model.CardNumber + "'", int.Parse(money), dateextend, "Từ APP", false);

                if (isSuccess)
                {
                    //result = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"]); 
                    result = new MessageReport(true, "Gia hạn thành công");
                }
                else
                {
                    //result = new MessageReport(false, FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateFailed"]);   
                    result = new MessageReport(false, "Gia hạn thất bại");
                }
            }
            else
            {
                result = new MessageReport(false, "Thẻ không tồn tại");
            }

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(result.isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
                return response;
            });
        }

        /// <summary>
        /// Lấy thông tin thẻ,kh qua mã thẻ
        /// </summary>
        /// Author      Date        Note
        /// LamHN     22/04/2019  Thêm mới
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Route("getcardbycardnumber")]
        [HttpGet]
        public HttpResponseMessage GetCardByCardNumber(HttpRequestMessage request, string cardnumber)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
       
            //Xử lý logic gia hạn
            var obj = _API_CardService.GetInfoByCardNumber(cardnumber);

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            });
        }
    }
}
