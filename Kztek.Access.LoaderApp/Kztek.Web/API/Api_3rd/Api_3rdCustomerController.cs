using Kztek.Model.CustomModel;
using Kztek.Model.Models;
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
    [RoutePrefix("api/3rd_Customer")]
    [Authorize]
    public class Api_3rdCustomerController : ApiBaseController
    {
        private IAPI_CustomerService _API_CustomerService;
        //private ItblCardSubmitEventService _tblLog;
        private IAPI_tblCustomerGroupService _tblCustomerGroup;

        public Api_3rdCustomerController(IAPI_CustomerService _API_CustomerService, IAPI_tblCustomerGroupService _tblCustomerGroup)
        {
            this._API_CustomerService = _API_CustomerService;
            this._tblCustomerGroup = _tblCustomerGroup;
            //  this._tblLog = _tblLog;
        }
        #region Customer
        /// <summary>
        /// get All Customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cardnumber"></param>
        /// <returns></returns>
        /// 

        [Route("getListCustomer")]
        [HttpGet]
        public HttpResponseMessage GetCustomer(HttpRequestMessage request)
        {
            string key = "";
            var obj = _API_CustomerService.GetTop10(key);

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("getAllCustomer")]
        [HttpGet]
        public async Task<List<tblCustomer_API>> getList(string key)
        {
            var list = _API_CustomerService.GetAllCustomer(key).Select(n => new { n.CustomerID, n.CustomerName, n.CustomerCode,n.CustomerGroupID }).ToList();
            var obj = new List<tblCustomer_API>();
            foreach (var item in list)
            {
                var obj2 = new tblCustomer_API()
                {
                    CustomerID = item.CustomerID.ToString(),
                    CustomerCode = item.CustomerCode,
                    CustomerName = item.CustomerName,
                    CustomerGroupID = item.CustomerGroupID
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(list != null ? obj : null);
        }



        [Route("getCustomerById")]
        [HttpGet]
        public HttpResponseMessage GetCustomerById(HttpRequestMessage request, string CustomerId)
        {
            var obj = _API_CustomerService.GetById(CustomerId);

            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, obj);
                return response;
            });
        }

        /// <summary>
        /// Create card
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public Result_tblCustomer_API3rd Create([FromBody] tblCustomer obj)
        {
            var result = new Result_tblCustomer_API3rd()
            {
                isSuccess = false,
                Message = "",
                CustomerID = "",
                CustomerName = ""
            };
            string Err = String.Empty;
            if (String.IsNullOrEmpty(obj.CustomerName.ToString()))
            {
                Err = " Vui lòng nhập tên khách hàng";
            }

            //if ((String.IsNullOrEmpty(obj.IDNumber) || String.IsNullOrWhiteSpace(obj.Mobile)))
            //{

            //    Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập số số điện thoại hoặc căn cước / cmtnd" : ", Vui lòng nhập số số điện thoại hoặc căn cước / cmtnd";
            //}
            if (String.IsNullOrWhiteSpace(obj.Mobile))
            {

                Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập số số điện thoại" : ", Vui lòng nhập số số điện thoại";
            }


            var existedCard = _API_CustomerService.GetByMobileOrIdNumber(obj.Mobile, obj.IDNumber);
            if (existedCard != null)
            {
                Err += String.IsNullOrEmpty(Err) ? " Sđt đã được đăng ký" : ", Số sđt đã được đăng ký";
            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }

            var model = new tblCustomer()
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = obj.CustomerName,
                Address = obj.Address,
                IDNumber = obj.IDNumber,
                Mobile = obj.Mobile,
                CustomerGroupID = obj.CustomerGroupID,
                EnableAccount = true,
                Inactive = false,
                SortOrder = 1,
                AccessLevelID = "",
                Finger1 = "",
                Finger2 = "",
                DevPass = "",
                AccessExpireDate = Convert.ToDateTime("2099/12/31"),
                CompartmentId = !string.IsNullOrEmpty(obj.CompartmentId) ? obj.CompartmentId.Trim() : "",

            };

            var resultCreate = _API_CustomerService.Create(model);

            WriteLog.WriteAPI(resultCreate, null, model.CustomerID.ToString(), "", "tblCustomer", ConstField.ApiParking, ActionConfigO.Create);

            //Trả lại response
            if (resultCreate.isSuccess)
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
                result.CustomerID = model.CustomerID.ToString();
                result.CustomerName = model.CustomerName;
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
        public Result_tblCustomer_API3rd Update([FromBody] tblCustomer obj)
        {
            var result = new Result_tblCustomer_API3rd()
            {
                isSuccess = false,
                Message = "",
                CustomerID = "",
                CustomerName = ""
            };
            string Err = String.Empty;

            var objOld = _API_CustomerService.GetById(obj.CustomerID.ToString());
            if (objOld == null)
            {
                result.isSuccess = false;
                result.Message = "Khách hàng không tồn tại";
                return result;
            }

            if (String.IsNullOrEmpty(obj.CustomerName.ToString()))
            {
                Err = " Vui lòng nhập tên khách hàng";
            }

            if (String.IsNullOrWhiteSpace(obj.Mobile))
            {

                Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập số số điện thoại" : ", Vui lòng nhập số số điện thoại";
            }


            var existedCard = _API_CustomerService.GetByMobileOrIdNumber(obj.Mobile, obj.IDNumber);
            if (existedCard != null && existedCard.CustomerID.ToString() != obj.CustomerID.ToString())
            {
                if (existedCard.CustomerGroupID != obj.CustomerGroupID)
                {
                    Err += String.IsNullOrEmpty(Err) ? " Số cmnd hoặc sđt đã được đăng ký" : ", Số cmnd hoặc sđt đã được đăng ký";
                }

            }

            if (!String.IsNullOrEmpty(Err))
            {
                result.Message = Err;
                return result;
            }



            objOld.CustomerName = obj.CustomerName;
            objOld.Address = obj.Address;
            objOld.IDNumber = obj.IDNumber;
            objOld.Mobile = obj.Mobile;
            objOld.CustomerGroupID = obj.CustomerGroupID;
            objOld.EnableAccount = true;
            objOld.Inactive = false;


            var result_update = _API_CustomerService.Update(objOld);

            //// Create Log
            //var objLog = new tblCustomer_API_Log()
            //{
            //     Id = Guid.NewGuid().ToString(),
            //    CustomerID = model.CustomerID.ToString(),
            //    CustomerName = obj.CustomerName,
            //    Address = obj.Address,//Đợi KH
            //    IDNumber = obj.IDNumber,
            //    CustomerGroupID = obj.CustomerGroupID,
            //    Mobile = obj.Mobile,
            //    CreateDate = DateTime.Now,
            //    SubmitMessage = result.Message,
            //    SubmitStatus = result.isSuccess ? 1 : 0,
            //    HTTP = "PUT",
            //};

            //_tblLog.Create(objLog);
            //Trả lại response
            if (result_update.isSuccess)
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
                result.CustomerID = obj.CustomerID.ToString();
                result.CustomerName = obj.CustomerName;

                WriteLog.WriteAPI(result_update, null, objOld.CustomerID.ToString(), "", "tblCustomer", ConstField.ApiParking, ActionConfigO.Update);

            }
            else
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
            }
            return result;
        }

        [Route("getAllByNameOrMobile")]
        [HttpGet]
        public async Task<List<tblCustomer_API3rd>> GetAllByNameOrMobile(string key)
        {
            var list = _API_CustomerService.GetByMobileOrName(key);

            return await Task.FromResult(list.ToList());
        }

        [Route("delete")]
        [HttpDelete]
        public MessageReport DeleteById(string id)
        {
            var obj = _API_CustomerService.GetById(id);
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Khách hàng không tồn tại trong hệ thống";
                result1.isSuccess = false;
                return result1;
            }

            var result = _API_CustomerService.DeleteById(id);

            if (result.isSuccess)
                WriteLog.WriteAPI(result, null, id, "", "tblCustomer", ConstField.ApiParking, ActionConfigO.Delete);


            return result;
        }
        #endregion

        #region customer_Group
        /// <summary>
        /// Create card
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CreateCustomerGroup")]
        [HttpPost]
        public Result_tblCustomerGroup CreateCustomerGroup([FromBody] tblCustomerGroup obj)
        {
            var result = new Result_tblCustomerGroup()
            {
                isSuccess = false,
                Message = "",
                CustomerGroupID = "",
                CustomerGroupName = ""
            };

            if (String.IsNullOrEmpty(obj.CustomerGroupName))
            {
                result.Message = "Vui lòng nhập tên nhóm khách hàng";
                return result;
            }

            var existedCard = _tblCustomerGroup.GetByName(obj.CustomerGroupName);
            if (existedCard != null)
            {
                result.Message = "Nhóm khách hàng đã tồn tại";
                return result;
            }

            

            obj.CustomerGroupID = Guid.NewGuid();

            obj.CustomerGroupCode = null;
            //obj.CustomerGroupName = obj.CustomerGroupName;
            obj.Description = obj.Description;
            obj.Inactive = false;
            obj.ParentID = String.IsNullOrWhiteSpace(obj.ParentID) ? "0" : obj.ParentID;
            obj.IsCompany = false;

            var resultCreate = _tblCustomerGroup.Create(obj);
            //Trả lại response
            if (resultCreate.isSuccess)
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
                result.CustomerGroupName = obj.CustomerGroupName;
                result.CustomerGroupID = obj.CustomerGroupID.ToString();
                WriteLog.WriteAPI(resultCreate, null, obj.CustomerGroupID.ToString(), "", "tblCustomerGroup", ConstField.ApiParking, ActionConfigO.Create);

            }
            else
            {
                result.isSuccess = resultCreate.isSuccess;
                result.Message = resultCreate.Message;
            }
            return result;

        }


        [Route("updateCustomerGroup")]
        [HttpPut]
        public Result_tblCustomerGroup updateCustomerGroup([FromBody] tblCustomerGroup obj)
        //public HttpResponseMessage updateCustomerGroup(HttpRequestMessage request, tblCustomerGroup obj)
        {


            var result = new Result_tblCustomerGroup()
            {
                isSuccess = false,
                Message = "",
                CustomerGroupID = "",
                CustomerGroupName = ""
            };

            var oldObj = _tblCustomerGroup.GetById(obj.CustomerGroupID);
            if (oldObj == null)
            {
                result.Message = "Nhóm khách hàng không tồn tại";
                return result;
            }


            if (String.IsNullOrEmpty(obj.CustomerGroupName))
            {
                result.Message = "Vui lòng nhập tên nhóm khách hàng";
                return result;
            }

            var existedCard = _tblCustomerGroup.GetByName(obj.CustomerGroupName);
            if (existedCard != null && existedCard.CustomerGroupID.ToString() != obj.CustomerGroupID.ToString())
            {
                result.Message = "Nhóm khách hàng đã tồn tại";
                return result;
            }


            oldObj.CustomerGroupName = obj.CustomerGroupName;
            oldObj.ParentID = obj.ParentID;
            oldObj.Ordering = obj.Ordering;


            var result_update = _tblCustomerGroup.Update(oldObj);

            WriteLog.WriteAPI(result_update, null, obj.CustomerGroupID.ToString(), "", "tblCustomerGroup", ConstField.ApiParking, ActionConfigO.Update);


            //_tblLog.Create(objLog);
            //Trả lại response
            if (result_update.isSuccess)
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
                result.CustomerGroupID = obj.CustomerGroupID.ToString();
                result.CustomerGroupName = obj.CustomerGroupName;
            }
            else
            {
                result.isSuccess = result_update.isSuccess;
                result.Message = result_update.Message;
            }
            return result;
        }


        [Route("GetListCustomerGroup")]
        [HttpGet]
        public HttpResponseMessage GetListCustomerGroup(HttpRequestMessage request)
        {
            var obj = _tblCustomerGroup.GetAll().ToList();

            var model = new List<tblCustomerGroupAPI>();

            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var model2 = new tblCustomerGroupAPI()
                    {
                        CustomerGroupName = item.CustomerGroupName,
                        ParentID = item.ParentID,
                        Ordering = item.Ordering,
                        CustomerGroupID = item.CustomerGroupID.ToString()
                    };
                    model.Add(model2);
                }

            }
            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllCustomerGroup")]
        [HttpGet]
        public async Task<List<tblCustomerGroupAPI>> GetAllCustomerGroup(string key)
        {
            var list = _tblCustomerGroup.GetAllCustomerGroup(key).Select(n => new { n.CustomerGroupID, n.CustomerGroupName, n.ParentID, n.Ordering }).ToList();
            var obj = new List<tblCustomerGroupAPI>();
            foreach (var item in list)
            {
                var obj2 = new tblCustomerGroupAPI()
                {
                    CustomerGroupID = item.CustomerGroupID.ToString(),
                    CustomerGroupName = item.CustomerGroupName,
                    ParentID = item.ParentID,
                    Ordering = item.Ordering
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(list != null ? obj : null);
        }

        [Route("getCustomerGroupById")]
        [HttpGet]
        public HttpResponseMessage getCustomerGroupById(HttpRequestMessage request, string CustomerGroupId)
        {
            //var obj = _tblCustomerGroup.GetById(CustomerGroupId);
            var customerGroup = _tblCustomerGroup.GetById(Guid.Parse(CustomerGroupId));
            //Trả lại response
            return CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, customerGroup);
                return response;
            });
        }

        [Route("GetAllByNameCustomerGroup")]
        [HttpGet]
        public async Task<List<tblCustomerGroupAPI>> GetAllByName(string name)
        {
            var list = _tblCustomerGroup.GetAll()
                .Select(n => new { n.CustomerGroupID, n.CustomerGroupName, n.ParentID, n.Ordering });

            if (!String.IsNullOrEmpty(name))
            {
                list = list.Where(n => n.CustomerGroupName.Contains(name)).ToList();
            };

            var obj = new List<tblCustomerGroupAPI>();
            foreach (var item in list)
            {
                var obj2 = new tblCustomerGroupAPI()
                {
                    CustomerGroupID = item.CustomerGroupID.ToString(),
                    CustomerGroupName = item.CustomerGroupName,
                    ParentID = item.ParentID,
                    Ordering = item.Ordering
                };

                obj.Add(obj2);
            }
            return await Task.FromResult(obj);
        }
        [Route("deleteCusGrp")]
        [HttpDelete]
        public MessageReport DeleteById_CustomerGrp(string id)
        {
            var obj = _tblCustomerGroup.GetById(Guid.Parse(id));
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = "Nhóm khách hàng không tồn tại";
                result1.isSuccess = false;
                return result1;
            }

            var result = _tblCustomerGroup.DeleteById(id);

            if (result.isSuccess)
                WriteLog.WriteAPI(result, null, id, "", "tblCustomerGroup", ConstField.ApiParking, ActionConfigO.Delete);



            return result;
        }
        #endregion
    }
}
