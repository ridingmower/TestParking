using Kztek.Model.CustomModel;
using Kztek.Service.API;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kztek.Web.API.Third_party
{
    [RoutePrefix("api/3rd/Customer")]
    public class API_3rd_CustomerController : ApiBaseController
    {
        private IAPI_CustomerService _API_CustomerService;

        public API_3rd_CustomerController(IAPI_CustomerService _API_CustomerService)
        {
            this._API_CustomerService = _API_CustomerService;
        }

        /// <summary>
        /// get All Customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cardnumber"></param>
        /// <returns></returns>
        [Route("getCustomer")]
        [HttpGet]
        [API_BasicAuthenticationAttribute]
        public HttpResponseMessage GetCustomer(HttpRequestMessage request, string key)
        {
            var obj = _API_CustomerService.GetTop10(key);

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            });
        }
    }
}
