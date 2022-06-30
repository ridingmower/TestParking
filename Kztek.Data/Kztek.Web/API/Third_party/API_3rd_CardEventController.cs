using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Service.API;
using Kztek.Service.Mobile;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Third_party
{
    [RoutePrefix("api/3rd/vehicleevent")]
    public class API_3rd_CardEventController : ApiBaseController
    {
        private IAPI_tblCardEventService _tblCardEventService;

        public API_3rd_CardEventController(IAPI_tblCardEventService _tblCardEventService)
        {
            this._tblCardEventService = _tblCardEventService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        [API_BasicAuthenticationAttribute]
        [Route("byOutPaging")]
        [HttpGet]
        public async Task<ReportInOut_API_3rd> byOutPaging(string Key, string CardGroupId, string VehicleGroupId, bool IsHaveTimeIn, string Fromdate, string Todate, int PageIndex, int PageSize)
        {
            int TotalItem = 0;
            int TotalPage = 0;
            var list = _tblCardEventService.GetReportInOut(Key, CardGroupId, IsHaveTimeIn, VehicleGroupId, Fromdate, Todate, PageIndex, PageSize, ref TotalItem, ref TotalPage).ToList();
            var obj = new ReportInOut_API_3rd()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalItem = TotalItem,
                TotalPage = TotalPage,
                ReportInOut_data = list
            };

            return await Task.FromResult(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("totalMoney")]
        [HttpGet]
        [API_BasicAuthenticationAttribute]
        public async Task<string> GetTotalMoney(string Key, string CardGroupId, string VehicleGroupId, string Fromdate, string Todate)
        {
            if (String.IsNullOrEmpty(Fromdate) || String.IsNullOrEmpty(Todate))
                return await Task.FromResult("Vui lòng nhập ngày bắt đầu, ngày kết thúc");

            long totalMoney = _tblCardEventService.GetTotalMoney(Key, CardGroupId, VehicleGroupId, Fromdate, Todate);

            return await Task.FromResult(totalMoney.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        [Route("getallvehiclegroup")]
        [API_BasicAuthenticationAttribute]
        public async Task<List<tblVehicleGroupAPI>> GetAllVehicleGroup()
        {
           var obj = _tblCardEventService.GetAllVehicleGroup();
            var lstVehicleGroup = new List<tblVehicleGroupAPI>();
            foreach (var item in obj.ToList())
            {
                var model = new tblVehicleGroupAPI()
                {
                    VehicleGroupName = item.VehicleGroupName,
                    VehicleGroupID = item.VehicleGroupID
                };
                lstVehicleGroup.Add(model);
            }
            return await Task.FromResult(lstVehicleGroup);
        }

    }
}
