using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_ApartmentController : Controller
    {
        #region Khai báo services

        public static string url;
        public static string objId;
        private IBM_ApartmentService _BM_ApartmentService;
        private IBM_Apartment_MemberService _BM_Apartment_MemberService;
        private IBM_Apartment_ServiceService _BM_Apartment_ServiceService;
        private IBM_ApartmentUseService _BM_ApartmentUseService;
        private IBM_ResidentService _BM_ResidentService;
        private IBM_ResidentGroupService _BM_ResidentGroupService;
        private IBM_BuildingService _BM_BuildingService;
        private IBM_Building_ServiceService _BM_Building_ServiceService;
        private IBM_FloorService _BM_FloorService;
        public BM_ApartmentController(IBM_ApartmentService _BM_ApartmentService, IBM_BuildingService _BM_BuildingService, IBM_FloorService _BM_FloorService, IBM_ResidentService _BM_ResidentService, IBM_ResidentGroupService _BM_ResidentGroupService, IBM_ApartmentUseService _BM_ApartmentUseService, IBM_Building_ServiceService _BM_Building_ServiceService, IBM_Apartment_MemberService _BM_Apartment_MemberService, IBM_Apartment_ServiceService _BM_Apartment_ServiceService)
        {
            this._BM_ApartmentService = _BM_ApartmentService;
            this._BM_Apartment_MemberService = _BM_Apartment_MemberService;
            this._BM_BuildingService = _BM_BuildingService;
            this._BM_FloorService = _BM_FloorService;
            this._BM_ResidentService = _BM_ResidentService;
            this._BM_ResidentGroupService = _BM_ResidentGroupService;
            this._BM_ApartmentUseService = _BM_ApartmentUseService;
            this._BM_Building_ServiceService = _BM_Building_ServiceService;
            this._BM_Apartment_ServiceService = _BM_Apartment_ServiceService;
        }

        #endregion Khai báo services

        #region DDL

        public List<SelectListModel> BM_ApartmentUseToDDL()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Lựa chọn --" }, };
            var lst = _BM_ApartmentUseService.GetAll().ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.Id, ItemText = item.Name });
                }
            }
            return list;
        }


        public List<SelectListModel> ResidentGroupToDDL()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Lựa chọn --" }, };
            var lst = _BM_ResidentGroupService.GetAll().ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.Id, ItemText = item.Name });
                }
            }
            return list;
        }

    
        #endregion

        #region Danh sách

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="key"></param>
        /// <param name="cardgroup"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", int page = 1)
        {
            //var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var list = _BM_ApartmentService.GetApartmentPaging(key, page, pageSize,ref totalItem);

            var gridModel = PageModelCustom<BM_ApartmentCustom>.GetPage(list, page, pageSize, totalItem);

            ViewBag.KeyWord = key;


            url = Request.Url.AbsoluteUri;

            ViewBag.objId = objId;

            return View(gridModel);
        }

        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.Building = _BM_BuildingService.BuildingIdToDDL();
            ViewBag.Purpose = BM_ApartmentUseToDDL();
            ViewBag.TypeElec = FunctionHelper.TypeElec();

            var model = _BM_ApartmentService.GetById(id);
            if (model != null)
            {
                var listMB = _BM_Apartment_MemberService.GetMemberApartment(model.Id);
                ViewBag.strEmployeeChoose = JsonConvert.SerializeObject(listMB);

                var listSV = _BM_Apartment_ServiceService.GetServiceApartment(model.Id);
                ViewBag.strServiceChoose = JsonConvert.SerializeObject(listSV);
            }
            else
            {
                model = new BM_Apartment();
               
            }

           

            return View(model);
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="txtFeeLevel">Phí</param>
        /// <param name="SaveAndCountinue">Tiếp tục hay không</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(BM_Apartment obj, string strEmployeeChoose = "", string strServiceChoose = "",string objId = "",string strfloor = "", bool SaveAndCountinue = false)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            ViewBag.Building = _BM_BuildingService.BuildingIdToDDL();
            ViewBag.Purpose = BM_ApartmentUseToDDL();
            ViewBag.TypeElec = FunctionHelper.TypeElec();
            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

           
            var model = _BM_ApartmentService.GetById(objId);
            //Kiểm tra
            if (model == null)
            {
                //Thêm mới
                model = new BM_Apartment
                {
                    Id = Common.GenerateId(),
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    Acreage = obj.Acreage,
                    ApartmentUseId = obj.ApartmentUseId,
                    BuildingId = obj.BuildingId,
                    FloorId = strfloor,
                    Code = obj.Code,
                    Description = obj.Description,
                    ElecticityType = obj.ElecticityType,
                    Name = obj.Name,
                    Note = obj.Note
                };

                //Thực hiện thêm mới
                result = _BM_ApartmentService.Create(model);
            }
            else
            {
                //Cập nhật
                model.Acreage = obj.Acreage;
                model.ApartmentUseId = obj.ApartmentUseId;
                model.BuildingId = obj.BuildingId;
                model.FloorId = strfloor;
                model.Code = obj.Code;
                model.Description = obj.Description;
                model.ElecticityType = obj.ElecticityType;
                model.Name = obj.Name;

                //Thực hiện cập nhật
                result = _BM_ApartmentService.Update(model);
            }

           
            if (result.isSuccess)
            {
                //xóa danh sách thành viên cũ
                _BM_Apartment_MemberService.DeleteMemberApartment(model.Id);

                //xóa danh sách dịch vụ cũ
                _BM_Apartment_ServiceService.DeleteServiceApartment(model.Id);

                //thêm thành viên
                if (!string.IsNullOrEmpty(strEmployeeChoose))
                {
                    var listResident = JsonConvert.DeserializeObject<List<BM_ResidentCustom>>(strEmployeeChoose);
                    if(listResident != null && listResident.Count() > 0)
                    {
                        foreach(var item in listResident)
                        {
                            var objMB = new BM_Apartment_Member
                            {
                                Id = Common.GenerateId(),
                                IsDeleted = false,
                                ApartmentId = model.Id,
                                ResidentId = item.Id,
                                RoleId = ""
                            };
                            _BM_Apartment_MemberService.Create(objMB);
                        }
                    }
                }

                //thêm dịch vụ
                if (!string.IsNullOrEmpty(strServiceChoose))
                {
                    var listService = JsonConvert.DeserializeObject<List<BM_Building_ServiceCustom>>(strServiceChoose);
                    if (listService != null && listService.Count() > 0)
                    {
                        foreach (var item in listService)
                        {
                            var arr = item.SchedulePay.Split(';');

                            var objSV = new BM_Apartment_Service
                            {
                                Id = Common.GenerateId(),
                                IsDeleted = false,
                                ApartmentId = model.Id,
                                Price = item.Price,
                                SchedulePay = arr.Length > 0 ? arr[0]: "",
                                ScheduleType = arr.Length > 0 && arr[1].Contains("hàng tháng") ? 1 : 2,
                                ServiceId = item.Id,
                                StartDate = Convert.ToDateTime(item.DateStart),
                                EndDate = Convert.ToDateTime(item.DateEnd)
                            };
                            _BM_Apartment_ServiceService.Create(objSV);
                        }
                    }
                }

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">ID bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int pageNumber = 1)
        {
            var obj = _BM_ApartmentService.GetById(id);
            ViewBag.Building = _BM_BuildingService.BuildingIdToDDL();
            ViewBag.Purpose = BM_ApartmentUseToDDL();
            ViewBag.TypeElec = FunctionHelper.TypeElec();
            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="txtFeeLevel">Phí</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(BM_Apartment obj, string objId, int pageNumber = 1)
        {
            //Danh sách sử dụng
            ViewBag.Building = _BM_BuildingService.BuildingIdToDDL();
            ViewBag.Purpose = BM_ApartmentUseToDDL();
            ViewBag.TypeElec = FunctionHelper.TypeElec();
            //Kiểm tra
            var oldObj = _BM_ApartmentService.GetById(objId);
            if (oldObj == null)
            {
                ViewBag.Error = "Thông tin không tồn tại";
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                ModelState.AddModelError("Name", "Vui lòng nhập tên");
                return View(obj);
            }

            var existed = _BM_ApartmentService.GetByName(obj.Name);
            if (existed != null && existed.Id != objId)
            {
                ModelState.AddModelError("Name", "Thông tin đã tồn tại");
                return View(obj);
            }

            //Gán giá trị
            oldObj.Name = obj.Name;
            oldObj.Description = obj.Description;


            //Thực hiện cập nhật
            var result = _BM_ApartmentService.Update(oldObj);
            if (result.isSuccess)
            {

                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author                  Date            Comments
        /// TrungNQ                 18/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var obj = new BM_Apartment();

            var result = _BM_ApartmentService.DeleteById(id, ref obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        #region Danh sách tầng
        public PartialViewResult Partial_ListFloor(string buildingid, string floorid)
        {
            //var list = _BM_FloorService.FloorByBuildingToDDL(buildingid);
            //ViewBag.FloorId = floorid;
            return PartialView(null);
        }
        #endregion

        #region Đăng ký thành viên

        /// <summary>
        /// Modal tìm kiếm thành viên
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ModalSearch()
        {
            ViewBag.ResidentGroup = ResidentGroupToDDL();
            return PartialView();
        }

        /// <summary>
        /// Danh sách thành viên tìm kiếm
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Partial_SearchResident(string key, string group, string employees, int page = 1)
        {
            int pageSize = 3;
            int total = 0;
            var list = _BM_ResidentService.GetResidentPaging(key, group, page, pageSize, ref total);

           
            var gridModel = PageModelCustom<BM_ResidentCustom>.GetPage(list, page, pageSize, total);

            if (!string.IsNullOrEmpty(employees))
            {
                ViewBag.Employees = employees.Split(',').ToList();
            }


            return PartialView(gridModel);
        }
        /// <summary>
        /// click chọn thành viên
        /// </summary>
        /// <param name="strIds">danh sách check thêm</param>
        /// <param name="isCheck">check hay uncheck</param>
        /// <param name="strEmployee">danh sách đã check</param>
        /// <param name="type">1 - là checkall, 2 - check từng cái</param>
        /// <returns></returns>
        public JsonResult ChooseResident(string strIds, bool isCheck, string strEmployee, string type)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(strEmployee))
            {
                list = strEmployee.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(strIds))
            {
                //check all
                if (type.Equals("1"))
                {
                    var listobj = strIds.Split(',').ToList();

                    if (listobj != null && listobj.Count > 0)
                    {
                        foreach (var item in listobj)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (list.Contains(item))
                                {
                                    if (!isCheck)
                                    {
                                        list.Remove(item);
                                    }
                                }
                                else
                                {
                                    if (isCheck)
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (list.Contains(strIds))
                    {
                        if (!isCheck)
                        {
                            list.Remove(strIds);
                        }
                    }
                    else
                    {
                        if (isCheck)
                        {
                            list.Add(strIds);
                        }
                    }
                }

            }

            strEmployee = string.Join(",", list);

            return Json(strEmployee, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// thêm thành viên đã chọn vào danh sách
        /// </summary>
        /// <param name="strEmployee">danh sách thành viên chọn thêm</param>
        /// <param name="strEmployeeChoose">danh sách thành viên đã có</param>
        /// <returns></returns>
        public JsonResult AddToListChoose(string strEmployee, string strEmployeeChoose)
        {
            var message = new MessageReport(true, "Đăng ký thành công");

            var list = strEmployee.Split(',').ToList();

            var listchoose = new List<BM_ResidentCustom>();

            if (!string.IsNullOrEmpty(strEmployeeChoose))
            {
                listchoose = JsonConvert.DeserializeObject<List<BM_ResidentCustom>>(strEmployeeChoose);

            }

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var arr = item.Split(';');

                    var objchoose = listchoose.FirstOrDefault(x => x.Id == arr[0]);

                    //nếu chưa có thì thêm
                    if (objchoose == null)
                    {
                        var newobj = new BM_ResidentCustom
                        {
                            Id = arr[0],
                            ResidentGroupId = arr[4],
                            Email = arr[3],
                            Name = arr[1],
                            Mobile = arr[2]
                        };

                        listchoose.Add(newobj);
                    }
                }
            }

            strEmployeeChoose = JsonConvert.SerializeObject(listchoose);

            var result = new { strEmployeeChoose = strEmployeeChoose, message = message };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Xóa thành viên
        /// </summary>
        /// <param name="id">id thông tin cần xóa</param>
        /// <param name="strEmployeeChoose">danh sách thành viên đã có</param>
        /// <returns></returns>
        public JsonResult RemoveResident(string id, string strEmployeeChoose)
        {
            if (!string.IsNullOrEmpty(strEmployeeChoose) && !string.IsNullOrEmpty(id))
            {
                var listchoose = JsonConvert.DeserializeObject<List<BM_ResidentCustom>>(strEmployeeChoose);

                var objchoose = listchoose.FirstOrDefault(n => n.Id == id);

                if (objchoose != null)
                {
                    listchoose.Remove(objchoose);
                }


                strEmployeeChoose = JsonConvert.SerializeObject(listchoose);
            }


            return Json(strEmployeeChoose, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Danh sách thành viên đã chọn
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Partial_ChooseResident(string strEmployeeChoose, string status, int page = 1)
        {

            var listobj = new List<BM_ResidentCustom>();
            int pagesize = 2;
            int totalitem = 0;
            ViewBag.Status = status;
            if (!string.IsNullOrEmpty(strEmployeeChoose))
            {
                listobj = JsonConvert.DeserializeObject<List<BM_ResidentCustom>>(strEmployeeChoose);

                //sắp xếp theo tên và thêm số thứ tự
                listobj = listobj.OrderBy(n => n.Name).Select((o, i) => { o.RowNumber = i + 1; return o; }).ToList();

                totalitem = listobj.Count;
            }

            //phân trang
            var newlist = listobj.Skip((page - 1) * pagesize).Take(pagesize).ToList();

            if (newlist.Count == 0 && page > 1)
            {
                page = page - 1;
                newlist = listobj.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }


            var gridModel = PageModelCustom<BM_ResidentCustom>.GetPage(newlist, page, pagesize, totalitem);

            return PartialView(gridModel);
        }

        #endregion

        #region Đăng ký dịch vụ

        /// <summary>
        /// Modal tìm kiếm 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ModalSearchService()
        {
            
            return PartialView();
        }

        /// <summary>
        /// Danh sách  tìm kiếm
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Partial_SearchService(string key, string services, int page = 1)
        {
            int pageSize = 3;
            int total = 0;
            var list = _BM_Building_ServiceService.GetResidentPaging(key, page, pageSize, ref total);


            var gridModel = PageModelCustom<BM_Building_ServiceCustom>.GetPage(list, page, pageSize, total);

            if (!string.IsNullOrEmpty(services))
            {
                ViewBag.Services = services.Split(',').ToList();
            }


            return PartialView(gridModel);
        }

        /// <summary>
        /// click chọn thành viên
        /// </summary>
        /// <param name="strIds">danh sách check thêm</param>
        /// <param name="isCheck">check hay uncheck</param>
        /// <param name="strEmployee">danh sách đã check</param>
        /// <param name="type">1 - là checkall, 2 - check từng cái</param>
        /// <returns></returns>
        public JsonResult ChooseService(string strIds, bool isCheck, string strService, string type)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(strService))
            {
                list = strService.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(strIds))
            {
                //check all
                if (type.Equals("1"))
                {
                    var listobj = strIds.Split(',').ToList();

                    if (listobj != null && listobj.Count > 0)
                    {
                        foreach (var item in listobj)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (list.Contains(item))
                                {
                                    if (!isCheck)
                                    {
                                        list.Remove(item);
                                    }
                                }
                                else
                                {
                                    if (isCheck)
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (list.Contains(strIds))
                    {
                        if (!isCheck)
                        {
                            list.Remove(strIds);
                        }
                    }
                    else
                    {
                        if (isCheck)
                        {
                            list.Add(strIds);
                        }
                    }
                }

            }

            strService = string.Join(",", list);

            return Json(strService, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// thêm dịch vụ đã chọn vào danh sách
        /// </summary>
        /// <param name="strEmployee">danh sách chọn thêm</param>
        /// <param name="strEmployeeChoose">danh sách đã có</param>
        /// <returns></returns>
        public JsonResult AddServiceToListChoose(string strService, string strServiceChoose)
        {
            var message = new MessageReport(true, "Đăng ký thành công");

            var list = strService.Split(',').ToList();

            var listchoose = new List<BM_Building_ServiceCustom>();

            if (!string.IsNullOrEmpty(strServiceChoose))
            {
                listchoose = JsonConvert.DeserializeObject<List<BM_Building_ServiceCustom>>(strServiceChoose);

            }

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var arr = item.Split('-');

                    var objchoose = listchoose.FirstOrDefault(x => x.Id == arr[0]);

                    //nếu chưa có thì thêm
                    if (objchoose == null)
                    {
                        var newobj = new BM_Building_ServiceCustom
                        {
                            Id = arr[0],
                            DateEnd = arr[4],
                            DateStart = arr[3],
                            Name = arr[1],
                            Price =!string.IsNullOrEmpty(arr[2]) ? Convert.ToDecimal(arr[2]) : 0,
                            SchedulePay = arr[5]
                        };

                        listchoose.Add(newobj);
                    }
                }
            }

            strServiceChoose = JsonConvert.SerializeObject(listchoose);

            var result = new { strServiceChoose = strServiceChoose, message = message };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Xóa dịch vụ
        /// </summary>
        /// <param name="id">id thông tin cần xóa</param>
        /// <param name="strEmployeeChoose">danh sách thành viên đã có</param>
        /// <returns></returns>
        public JsonResult RemoveService(string id, string strServiceChoose)
        {
            if (!string.IsNullOrEmpty(strServiceChoose) && !string.IsNullOrEmpty(id))
            {
                var listchoose = JsonConvert.DeserializeObject<List<BM_Building_ServiceCustom>>(strServiceChoose);

                var objchoose = listchoose.FirstOrDefault(n => n.Id == id);

                if (objchoose != null)
                {
                    listchoose.Remove(objchoose);
                }


                strServiceChoose = JsonConvert.SerializeObject(listchoose);
            }


            return Json(strServiceChoose, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Danh sách dịch vụ đã chọn
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Partial_ChooseService(string strServiceChoose, string status, int page = 1)
        {

            var listobj = new List<BM_Building_ServiceCustom>();
            int pagesize = 2;
            int totalitem = 0;
            ViewBag.Status = status;
            if (!string.IsNullOrEmpty(strServiceChoose))
            {
                listobj = JsonConvert.DeserializeObject<List<BM_Building_ServiceCustom>>(strServiceChoose);

                //sắp xếp theo tên và thêm số thứ tự
                listobj = listobj.OrderBy(n => n.Name).Select((o, i) => { o.RowNumber = i + 1; return o; }).ToList();

                totalitem = listobj.Count;
            }

            //phân trang
            var newlist = listobj.Skip((page - 1) * pagesize).Take(pagesize).ToList();

            if (newlist.Count == 0 && page > 1)
            {
                page = page - 1;
                newlist = listobj.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }


            var gridModel = PageModelCustom<BM_Building_ServiceCustom>.GetPage(newlist, page, pagesize, totalitem);

            return PartialView(gridModel);
        }

        #endregion
    }
}