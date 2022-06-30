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
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Third_party
{
    [RoutePrefix("api/3rd_event")]
    [Authorize]
    public class Api_3rdCardEventController : ApiBaseController
    {
        private IAPI_tblCardEventService _tblCardEventService;
        private IAPI_CustomerService _API_CustomerService;
        private IAPI_tblCustomerGroupService _tblCustomerGroup;
        private IAPI_CardGroupService _API_CardGroupService;


        public Api_3rdCardEventController(IAPI_tblCardEventService _tblCardEventService, IAPI_CustomerService _API_CustomerService, IAPI_tblCustomerGroupService _tblCustomerGroup, IAPI_CardGroupService _API_CardGroupService)
        {
            this._tblCardEventService = _tblCardEventService;
            this._API_CustomerService = _API_CustomerService;
            this._tblCustomerGroup = _tblCustomerGroup;
            this._API_CardGroupService = _API_CardGroupService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        [Route("byOutPaging")]
        [HttpGet]
        public async Task<ReportInOut_API_3rd> byOutPaging(string Key, string CardGroupId, string VehicleGroupId, bool IsHaveTimeIn, string Fromdate, string Todate, string PageIndex, string PageSize)
        {
            int TotalItem = 0;
            int TotalPage = 0;
            PageIndex = PageIndex == "0" || String.IsNullOrEmpty(PageIndex) ? "1" : PageIndex;
            PageSize = PageSize == "0" || String.IsNullOrEmpty(PageSize) ? "20" : PageSize;

            var list = _tblCardEventService.GetReportInOut(Key, CardGroupId, IsHaveTimeIn, VehicleGroupId, Fromdate, Todate, Convert.ToInt32(PageIndex), Convert.ToInt32(PageSize), ref TotalItem, ref TotalPage).ToList();
            long totalMoney = _tblCardEventService.GetTotalMoney(Key, CardGroupId, VehicleGroupId, Fromdate, Todate);

            var customers = _API_CustomerService.GetAll();
            var cardGroups = _API_CardGroupService.GetAll().ToList();
            var vehicleGroups = _tblCardEventService.GetAllVehicleGroup().ToList();
            var customerGroups = _tblCustomerGroup.GetAll().ToList();

            if (cardGroups.Count() > 0 || vehicleGroups.Count() > 0 || customerGroups.Count() > 0)
            {
                foreach (var item in list)
                {
                    if (cardGroups.Count() > 0)
                    {
                        item.CardGroupName = String.Empty;
                        if (!String.IsNullOrEmpty(item.CustomerGroupID))
                        {
                            var objCardGroup = cardGroups.Where(n => n.CardGroupID.ToString() == item.CardGroupId).FirstOrDefault();
                            if (objCardGroup != null)
                                item.CardGroupName = objCardGroup.CardGroupName;
                        }


                    }
                    if (vehicleGroups.Count() > 0)
                    {
                        item.VehicleGroupName = String.Empty;
                        if (!String.IsNullOrEmpty(item.VehicleGroupID))
                        {
                            var objVehicleGroup = vehicleGroups.Where(n => n.VehicleGroupID.ToString() == item.VehicleGroupID).FirstOrDefault();
                            if (objVehicleGroup != null)
                                item.CardGroupName = objVehicleGroup.VehicleGroupName;
                        }

                    }
                    if (customerGroups.Count() > 0)
                    {
                        item.CustomerGroupName = String.Empty;
                        if (!String.IsNullOrEmpty(item.CustomerGroupID))
                        {
                            var objCustomerGroup = customerGroups.Where(n => n.CustomerGroupID.ToString() == item.CustomerGroupID).FirstOrDefault();
                            if (objCustomerGroup != null)
                                item.CardGroupName = objCustomerGroup.CustomerGroupName;
                        }

                    }
                    if (customers.Count() > 0)
                    {
                        item.CustomerName = String.Empty;
                        if (!String.IsNullOrEmpty(item.CustomerGroupID))
                        {
                            var objCustomerGroup = customerGroups.Where(n => n.CustomerGroupID.ToString() == item.CustomerGroupID).FirstOrDefault();
                            if (objCustomerGroup != null)
                                item.CardGroupName = objCustomerGroup.CustomerGroupName;
                        }

                    }
                }
            }

            var obj = new ReportInOut_API_3rd()
            {
                PageIndex = Convert.ToInt32(PageIndex),
                PageSize = Convert.ToInt32(PageSize),
                TotalItem = TotalItem,
                TotalPage = TotalPage,
                TotalMoney = totalMoney,
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
        public async Task<string> GetTotalMoney(string Key, string CardGroupId, string VehicleGroupId, string Fromdate, string Todate)
        {
            if (String.IsNullOrEmpty(Fromdate) || String.IsNullOrEmpty(Todate))
                return await Task.FromResult("Vui lòng nhập ngày bắt đầu, ngày kết thúc");

            long totalMoney = _tblCardEventService.GetTotalMoney(Key, CardGroupId, VehicleGroupId, Fromdate, Todate);

            return await Task.FromResult(totalMoney.ToString());
        }

        [Route("GetTotalInOut")]
        [HttpGet]
        public async Task<List<ReportTotalInOut_Day>> GetTotalInOut(string CardGroupId, string VehicleGroupId, string Fromdate, string Todate)
        {
            DateTime fdate = DateTime.Now;
            DateTime tdate = DateTime.Now;
            if (!string.IsNullOrEmpty(Fromdate))
            {
                fdate = Convert.ToDateTime(Fromdate);
                // fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(Todate))
            {
                tdate = Convert.ToDateTime(Todate);
                // toDate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var list = _tblCardEventService.GetTotalInOut_Day(CardGroupId, VehicleGroupId, fdate, tdate).ToList();
            //long totalMoney = _tblCardEventService.GetTotalMoney(CardGroupId, VehicleGroupId, Fromdate, Todate);

            return await Task.FromResult(list);
        }

    }
}
