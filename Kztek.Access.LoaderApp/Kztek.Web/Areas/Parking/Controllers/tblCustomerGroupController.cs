using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kztek.Web.Core.Functions;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblCustomerGroupController : Controller
    {
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblSystemConfigService _tblSystemConfigService;

        public tblCustomerGroupController(ItblCustomerGroupService _tblCustomerGroupService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page, string group = "", string selectedId = "")
        {
            var lst = _tblCustomerGroupService.GetAll().ToList();

            //Viewbag
            ViewBag.Keyword = key;
            //ViewBag.ListMenu = _MenuFunctionService.GetAll().ToList();

            ViewBag.GroupID = group;
            ViewBag.selectedIdValue = selectedId;

            return View(lst);
        }

        #endregion Danh sách

        public PartialViewResult MenuChilden(List<tblCustomerGroup> childList, List<tblCustomerGroup> AllList, string selectedId = "")
        {
            //Viewbag
            ViewBag.ListMenu = AllList;
            ViewBag.selectedIdValue = selectedId;

            return PartialView(childList);
        }

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string controllername, string id)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            //ViewBag.parentidValue = parentid;
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
            int count = _tblCustomerGroupService.CountParent();
            var obj = new tblCustomerGroup()
            {                        
                Ordering = count + 1,
                Inactive = false
            };

            if (!string.IsNullOrEmpty(id))
            {
                var obj2 = _tblCustomerGroupService.GetById(Guid.Parse(id));
                if (obj2 != null)
                {
                    obj.ParentID = obj2.ParentID;              
                    obj.Inactive = obj2.Inactive;
                    obj.Ordering = obj2.Ordering + 1;
                }

            }

            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(tblCustomerGroup obj, bool SaveAndCountinue = false)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerGroupName))
            {
                ModelState.AddModelError("CustomerGroupName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["cusGrp_name"]);
                return View(obj);
            }

            obj.CustomerGroupID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblCustomerGroupService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerGroupID.ToString(), obj.CustomerGroupName, "tblCustomerGroup", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { selectedId = obj.CustomerGroupID, id = obj.CustomerGroupID });
                }

                return RedirectToAction("Index", new { selectedId = obj.CustomerGroupID });
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id)
        {
            

            var obj = _tblCustomerGroupService.GetById(Guid.Parse(id));

            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            ViewBag.DDLMenu = (obj != null) ? GetMenuList().Where(n => n.CustomerGroupID != obj.CustomerGroupID.ToString() ) : GetMenuList();

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblCustomerGroup obj)
        {
            ViewBag.DDLMenu = (obj != null) ? GetMenuList().Where(n => n.CustomerGroupID != obj.CustomerGroupID.ToString()) : GetMenuList();

            var oldObj = _tblCustomerGroupService.GetById(obj.CustomerGroupID);
            if (oldObj == null)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CustomerGroupName))
            {
                ModelState.AddModelError("CustomerGroupName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["cusGrp_name"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            oldObj.CustomerGroupCode = obj.CustomerGroupCode;
            oldObj.CustomerGroupName = obj.CustomerGroupName;
            oldObj.Description = obj.Description;
            oldObj.Inactive = obj.Inactive;
            oldObj.ParentID = obj.ParentID;
            oldObj.Ordering = obj.Ordering;
            oldObj.IsCompany = obj.IsCompany;
            oldObj.Tax = obj.Tax;

            //Thực hiện cập nhật
            var result = _tblCustomerGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerGroupID.ToString(), obj.CustomerGroupName, "tblCustomerGroup", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { selectedId = obj.CustomerGroupID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa nhiều

        public JsonResult MutilDelete(string lstId)
        {
            bool isSucccess = _tblCustomerGroupService.DeleteByIds(lstId);
            if (isSucccess)
            {
                MessageReport report = new MessageReport(true, "Xóa thành công");
                WriteLog.Write(report, GetCurrentUser.GetUser(), lstId, lstId, "tblCustomerGroup", ConstField.ParkingCode, ActionConfigO.Delete);
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa nhiều

        private List<tblCustomerGroupSubmit> GetMenuList()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CustomerGroupID");

            var list = new List<tblCustomerGroupSubmit>
            {
                new tblCustomerGroupSubmit {  CustomerGroupID = "0", CustomerGroupName = Dictionary["SlectlistCusGrp"] }
            };
            var MenuList = _tblCustomerGroupService.GetAllActive();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.Ordering))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new tblCustomerGroupSubmit { CustomerGroupID = item.CustomerGroupID.ToString(), CustomerGroupName = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new tblCustomerGroupSubmit { CustomerGroupID = item1.CustomerGroupID, CustomerGroupName = item.CustomerGroupName + " / " + item1.CustomerGroupName });
                        }
                        //Phân tách các danh mục
                        list.Add(new tblCustomerGroupSubmit { CustomerGroupID = "-1", CustomerGroupName = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new tblCustomerGroupSubmit { CustomerGroupID = "-1", CustomerGroupName = "-----" });
                    }
                }
            }
            return list;
        }

        private List<tblCustomerGroupSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<tblCustomerGroupSubmit> lst = new List<tblCustomerGroupSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _tblCustomerGroupService.GetAllChildActiveByParentID(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new tblCustomerGroupSubmit { CustomerGroupID = item.CustomerGroupID.ToString(), CustomerGroupName = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new tblCustomerGroupSubmit { CustomerGroupID = item1.CustomerGroupID, CustomerGroupName = item.CustomerGroupName + " / " + item1.CustomerGroupName });
                        }
                    }
                }
            }
            return lst;
        }
    }
}