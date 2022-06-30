using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using Kztek.Service.API;
using Kztek.Service.Mobile;
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
    [RoutePrefix("api/3rd_vehiclegroup")]
    [Authorize]
    public class Api_3rdVehicleGroupController : ApiBaseController
    {
        private IAPI_tblVehicleGroupService _tblVehicleGroupService;

        public Api_3rdVehicleGroupController(IAPI_tblVehicleGroupService _tblVehicleGroupService)
        {
            this._tblVehicleGroupService = _tblVehicleGroupService;
        }

        /// <summary>
        /// 
        /// </summary>
        [Route("getall")]
        public async Task<List<tblVehicleGroupAPI>> GetAllVehicleGroup()
        {
            var obj = _tblVehicleGroupService.GetAllVehicleGroup();
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

        [Route("getVehicleGroupById")]
        [HttpGet]
        public tblVehicleGroup GetVehicleGroupById(string VehicleGroupId)
        {
            var obj = _tblVehicleGroupService.GetById(Guid.Parse(VehicleGroupId));
            return obj;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public Result_tblVehicleGroupAPI Create([FromBody] tblVehicleGroup obj)
        {
            var result = new Result_tblVehicleGroupAPI()
            {
                isSuccess = false,
                Message = "",
                VehicleGroupID = "",
                VehicleGroupName = ""
            };
            string Err = String.Empty;
            if (String.IsNullOrEmpty(obj.VehicleGroupName.ToString()))
            {
                result.Message = "Vui lòng nhập tên nhóm phương tiện";
                return result;
            }

            var existedCard = _tblVehicleGroupService.GetByName(obj.VehicleGroupName);
            if (existedCard != null)
            {
                result.Message = "Nhóm phương tiện đã tồn tại";
                return result;
            }


            var model = new tblVehicleGroup();
            model.VehicleGroupID = Guid.NewGuid();
            model.VehicleGroupName = obj.VehicleGroupName;
            model.VehicleGroupCode = String.Empty;
            model.VehicleType = obj.VehicleType;
            model.LimitNumber = obj.LimitNumber;
            model.Inactive = false;
            model.SortOrder = obj.SortOrder;

            var resultCreate = _tblVehicleGroupService.Create(model);

            //Trả lại response
            if (resultCreate.isSuccess)
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
                result.VehicleGroupID = model.VehicleGroupID.ToString();
                result.VehicleGroupName = model.VehicleGroupName;

                WriteLog.WriteAPI(resultCreate, null, model.VehicleGroupID.ToString(), "", "tblVehicleGroup", ConstField.ApiParking, ActionConfigO.Create);

            }
            else
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
            }
            return result;
        }

        [Route("update")]
        [HttpPut]
        public Result_tblVehicleGroupAPI Update([FromBody] tblVehicleGroup obj)
        {
            var result = new Result_tblVehicleGroupAPI()
            {
                isSuccess = false,
                Message = "",
                VehicleGroupID = "",
                VehicleGroupName = ""
            };

            var objOld = _tblVehicleGroupService.GetById(obj.VehicleGroupID);

            if (objOld == null)
            {
                result.Message = "Nhóm phương tiện không tồn tại";
                return result;
            }

            if (String.IsNullOrEmpty(obj.VehicleGroupName.ToString()))
            {
                result.Message = "Vui lòng nhập tên nhóm phương tiện";
                return result;
            }

            var existedCard = _tblVehicleGroupService.GetByName(obj.VehicleGroupName);
            if (existedCard != null && existedCard.VehicleGroupID != obj.VehicleGroupID)
            {
                result.Message = "Nhóm phương tiện đã tồn tại";
                return result;
            }

            objOld.VehicleGroupName = obj.VehicleGroupName;
            objOld.VehicleGroupCode = String.Empty;
            objOld.VehicleType = obj.VehicleType;
            objOld.LimitNumber = obj.LimitNumber;
            objOld.Inactive = obj.Inactive;
            objOld.SortOrder = obj.SortOrder;


            var result_update = _tblVehicleGroupService.Update(objOld);
            if (result_update.isSuccess)
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
                result.VehicleGroupID = obj.VehicleGroupID.ToString();
                result.VehicleGroupName = obj.VehicleGroupName;

                WriteLog.WriteAPI(result_update, null, objOld.VehicleGroupID.ToString(), "", "tblVehicleGroup", ConstField.ApiParking, ActionConfigO.Update);

            }
            else
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
            }
            return result;
        }

        [Route("getAllByName")]
        [HttpGet]
        public async Task<List<tblVehicleGroupAPI>> GetAllByName(string name)
        {
            var list = _tblVehicleGroupService.GetAllVehicleGroup()
                .Select(n => new { n.VehicleGroupName, n.VehicleGroupID });
            if (!String.IsNullOrEmpty(name))
            {
                list =list.Where(n => n.VehicleGroupName.Contains(name)).ToList();
            };
                
            var obj = new List<tblVehicleGroupAPI>();
            foreach (var item in list)
            {
                var obj2 = new tblVehicleGroupAPI()
                {
                    VehicleGroupID = item.VehicleGroupID,
                    VehicleGroupName = item.VehicleGroupName,
                };
                obj.Add(obj2);
            }
            return await Task.FromResult(obj);
        }

        [Route("getById")]
        [HttpGet]
        public async Task<tblVehicleGroup> getById(string id)
        {
            var obj = _tblVehicleGroupService.GetById(Guid.Parse(id));
            return await Task.FromResult(obj);
        }

        [Route("delete")]
        [HttpDelete]
        public MessageReport DeleteById(string id)
        {
            var obj = _tblVehicleGroupService.GetById(Guid.Parse(id));
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Thẻ không tồn tại trong hệ thống";
                result1.isSuccess = false;
                return result1;
            }

            var result = _tblVehicleGroupService.DeleteById(id);
            if (result.isSuccess)
                WriteLog.WriteAPI(result, null, id, "", "tblVehicleGroup", ConstField.ApiParking, ActionConfigO.Delete);

            return result;
        }

    }
}
