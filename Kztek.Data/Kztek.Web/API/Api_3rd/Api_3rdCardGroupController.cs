using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.API;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Api_3rd
{
    [RoutePrefix("api/3rd_cardgroup")]
    [Authorize]
    public class Api_3rdCardGroupController : ApiController
    {
        private IAPI_CardGroupService _API_CardGroupService;

        public Api_3rdCardGroupController(IAPI_CardGroupService _API_CardGroupService)
        {
            this._API_CardGroupService = _API_CardGroupService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("getall")]
        [HttpGet]
        public async Task<List<tblCardGroup_3rd_API>> getall()
        {
            var list = _API_CardGroupService.GetAll().Select(n => new { n.CardGroupID, n.CardGroupName }).ToList();
            var obj = new List<tblCardGroup_3rd_API>();
            foreach (var item in list)
            {
                var obj2 = new tblCardGroup_3rd_API()
                {
                    CardGroupID = item.CardGroupID.ToString(),
                    CardGroupName = item.CardGroupName,
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(obj);
        }

        /// <summary>
        /// Api thêm mới bản ghi
        /// </summary>
        /// Author          Date            Summary
        /// TrungNQ         14/12/2019      Thêm mới
        /// <param name="value"></param>
        /// <returns> MessageReport </returns>

        [Route("Create")]
        [HttpPost]
        public Report_tblCardGroup Create([FromBody] tblCardGroup obj)
        {
            var result = new Report_tblCardGroup()
            {
                isSuccess = false,
                Message = "",
                CardGroupID = "",
                CardGroupName = ""
            };

            string Err = String.Empty;

            if (obj.CardGroupName == null)
            {
                Err = "Vui lòng nhập tên nhóm thẻ";
            }

            if (obj.CardType.ToString() == null)
            {
                Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập Loại thẻ" : ", Vui lòng nhập Loại thẻ";
            }

            if (String.IsNullOrEmpty(obj.VehicleGroupID.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập loại phương tiện" : ", Vui lòng nhập loại phương tiện";
            }


            if (String.IsNullOrEmpty(obj.LaneIDs.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập làn vào ra" : ", Vui lòng nhập làn vào ra";
            }

            var existedCard = _API_CardGroupService.GetByName(obj.CardGroupName);
            if (existedCard != null)
            {
                Err = "Tên nhóm thẻ đã tồn tại";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }

            obj.CardGroupID = Guid.NewGuid();

            if (string.IsNullOrEmpty(obj.TimePeriods))
            {
                obj.TimePeriods = "00:00-00:00-00:00";
            }

            if (string.IsNullOrEmpty(obj.Costs))
            {
                obj.Costs = "0";
            }



            var resultCreate = _API_CardGroupService.Create(obj);
            if (resultCreate.isSuccess)
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
                result.CardGroupID = obj.CardGroupID.ToString();
                result.CardGroupName = obj.CardGroupName;
                WriteLog.WriteAPI(resultCreate, null, obj.CardGroupID.ToString(), obj.CardGroupName, "tblCardGroup", ConstField.ApiParking, ActionConfigO.Create);
            }
            else
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
            }
            return result;
        }

        [Route("Update")]
        [HttpPut]
        public Report_tblCardGroup Update([FromBody] tblCardGroup obj)
        {
            var result = new Report_tblCardGroup()
            {
                isSuccess = false,
                Message = "",
                CardGroupID = "",
                CardGroupName = ""
            };

            string Err = String.Empty;

            //Kiểm tra
            var oldObj = _API_CardGroupService.GetById(Guid.Parse(obj.CardGroupID.ToString()));

            if (oldObj == null)
            {
                Err = "Nhóm khách hàng không tồn tại";
                result.Message = Err;
                return result;
            }

            if (obj.CardGroupName == null)
            {
                Err = "Vui lòng nhập tên nhóm thẻ";
            }

            if (obj.CardType.ToString() == null)
            {
                Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập Loại thẻ" : ", Vui lòng nhập Loại thẻ";
            }

            if (String.IsNullOrEmpty(obj.VehicleGroupID.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập loại phương tiện" : ", Vui lòng nhập loại phương tiện";
            }


            if (String.IsNullOrEmpty(obj.LaneIDs.ToString()))
            {
                Err += String.IsNullOrEmpty(Err) ? " Vui lòng nhập làn vào ra" : ", Vui lòng nhập làn vào ra";
            }

            var existedCard = _API_CardGroupService.GetByName(obj.CardGroupName);
            if (existedCard != null && existedCard.CardGroupID.ToString() != obj.CardGroupID.ToString())
            {
                Err = "Tên nhóm thẻ đã tồn tại";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }


            //Gán giá trị
            oldObj.CardGroupCode = obj.CardGroupCode;
            oldObj.CardGroupName = obj.CardGroupName;
            oldObj.Description = obj.Description;
            oldObj.CardType = obj.CardType;
            oldObj.VehicleGroupID = obj.VehicleGroupID;
            oldObj.LaneIDs = obj.LaneIDs;
            oldObj.DayTimeFrom = obj.DayTimeFrom;
            oldObj.DayTimeTo = obj.DayTimeTo;
            oldObj.Formulation = obj.Formulation;
            oldObj.EachFee = obj.EachFee;
            oldObj.Block0 = obj.Block0;
            oldObj.Time0 = obj.Time0;
            oldObj.Block1 = obj.Block1;
            oldObj.Time1 = obj.Time1;
            oldObj.Block2 = obj.Block2;
            oldObj.Time2 = obj.Time2;
            oldObj.Block3 = obj.Block3;
            oldObj.Time3 = obj.Time3;
            oldObj.Block4 = obj.Block4;
            oldObj.Time4 = obj.Time4;
            oldObj.Block5 = obj.Block5;
            oldObj.Time5 = obj.Time5;

            if (!string.IsNullOrWhiteSpace(obj.TimePeriods))
            {
                oldObj.TimePeriods = obj.TimePeriods;
            }

            if (!string.IsNullOrWhiteSpace(obj.Costs))
            {
                oldObj.Costs = obj.Costs;
            }

            oldObj.Inactive = obj.Inactive;
            oldObj.SortOrder = obj.SortOrder;
            oldObj.IsHaveMoneyExcessTime = obj.IsHaveMoneyExcessTime;
            oldObj.EnableFree = obj.EnableFree;
            oldObj.FreeTime = obj.FreeTime;
            oldObj.IsCheckPlate = obj.IsCheckPlate;
            oldObj.IsHaveMoneyExpiredDate = obj.IsHaveMoneyExpiredDate;

            //Thực hiện cập nhật
            var resultUpdate = _API_CardGroupService.Update(oldObj);
            if (resultUpdate.isSuccess)
            {
                result.isSuccess = resultUpdate.isSuccess;
                result.Message = resultUpdate.Message;
                result.CardGroupID = obj.CardGroupID.ToString();
                result.CardGroupName = obj.CardGroupName;
                WriteLog.WriteAPI(resultUpdate, null, obj.CardGroupID.ToString(), obj.CardGroupName, "tblCardGroup", ConstField.ApiParking, ActionConfigO.Update);
                //public static void Write(MessageReport report, User currentuser, string objId, string objname, string classname, string appcode, string actions)
            }
            else
            {
                result.isSuccess = resultUpdate.isSuccess;
                result.Message = resultUpdate.Message;
            }
            return result;
        }

        [Route("Delete")]
        [HttpDelete]
        public MessageReport Delete(string id)
        {
            var result = new MessageReport()
            {
                isSuccess = false
            };
            

            //Kiểm tra
            var oldObj = _API_CardGroupService.GetById(Guid.Parse(id));

            if (oldObj == null)
            {
                result.Message = "Nhóm khách hàng không tồn tại";
                return result;
            }

            var resultDel = _API_CardGroupService.DeleteById(id);
            if (resultDel.isSuccess)
                WriteLog.WriteAPI(resultDel, null, id, oldObj.CardGroupName, "tblCardGroup", ConstField.ApiParking, ActionConfigO.Delete);

            return resultDel;
        }

        [Route("GetbyId")]
        [HttpGet]
        public async Task<tblCardGroup> getById(string id)
        {

            var objGetbyId = _API_CardGroupService.GetById(Guid.Parse(id.ToString()));
            
            return await Task.FromResult(objGetbyId);
        }

        [Route("GetAllByName")]
        [HttpGet]
        public async Task<List<tblCardGroup_3rd_API>> GetAllByName(string name)
        {
            var list = _API_CardGroupService.GetAll()
                .Select(n => new { n.CardGroupID, n.CardGroupName })
                .Where(n => n.CardGroupName.Contains(name)).ToList();
            var obj = new List<tblCardGroup_3rd_API>();
            foreach (var item in list)
            {
                var obj2 = new tblCardGroup_3rd_API()
                {
                    CardGroupID = item.CardGroupID.ToString(),
                    CardGroupName = item.CardGroupName,
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(obj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("getList")]
        [HttpGet]
        public async Task<List<tblCardGroup_3rd_API>> getList(string key)
        {
            var list = _API_CardGroupService.GetList(key).Select(n => new { n.CardGroupID, n.CardGroupName }).ToList();
            var obj = new List<tblCardGroup_3rd_API>();
            foreach (var item in list)
            {
                var obj2 = new tblCardGroup_3rd_API()
                {
                    CardGroupID = item.CardGroupID.ToString(),
                    CardGroupName = item.CardGroupName,
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(list != null ? obj : null);
        }
    }
}
