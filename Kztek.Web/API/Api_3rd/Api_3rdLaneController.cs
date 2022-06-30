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

namespace Kztek.Web.API.Api_3rd
{
    [RoutePrefix("api/3rd_Lane")]
    [Authorize]
    public class Api_3rdLaneController : ApiBaseController
    {
        private IAPI_tblLaneService _tblLaneService;

        public Api_3rdLaneController(IAPI_tblLaneService _tblLaneService)
        {
            this._tblLaneService = _tblLaneService;
        }

        /// <summary>
        /// 
        /// </summary>
        [Route("getall")]
        public async Task<List<tblLane>> GetAllLane()
        {
            var obj = _tblLaneService.GetAllLane();
            return await Task.FromResult(obj.ToList());
        }


        [Route("getByName")]
        [HttpGet]
        public async Task<List<tblLaneAPI>> GetAllByName(string name)
        {
            var list = _tblLaneService.GetAllLane();
            if (!String.IsNullOrEmpty(name))
            {
                list = list.Where(n => n.LaneName.Contains(name)).ToList();
            }

            var lst = new List<tblLaneAPI>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var model2 = new tblLaneAPI()
                    {
                        LaneID = item.LaneID.ToString(),
                        LaneCode = item.LaneCode,
                        LaneName = item.LaneName
                    };
                    lst.Add(model2);
                }

            }

            return await Task.FromResult(lst);
        }

       

    }
}
