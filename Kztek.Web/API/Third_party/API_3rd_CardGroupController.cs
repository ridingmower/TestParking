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
    [RoutePrefix("api/3rd/cardgroup")]
    public class API_3rd_CardGroupController : ApiBaseController
    {
        private IAPI_CardGroupService _API_CardGroupService;

        public API_3rd_CardGroupController(IAPI_CardGroupService _API_CardGroupService)
        {
            this._API_CardGroupService = _API_CardGroupService;
        }

        /// <summary>
        /// get All CardGroup
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cardnumber"></param>
        /// <returns></returns>
        [Route("getlistcardgroup")]
        [HttpGet]
        [API_BasicAuthenticationAttribute]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {

            var obj = _API_CardGroupService.GetAll().Select(n=>new {n.CardGroupID,n.CardGroupName});

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            });
        }
    }
}
