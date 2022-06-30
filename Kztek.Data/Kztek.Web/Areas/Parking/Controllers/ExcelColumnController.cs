using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ExcelColumnController : Controller
    {
        #region Khai báo services
        private IExcelColumnService _ExcelColumnService;
        private IMenuFunctionService _MenuFunctionService;

        public ExcelColumnController(IExcelColumnService _ExcelColumnService, IMenuFunctionService _MenuFunctionService)
        {
            this._ExcelColumnService = _ExcelColumnService;
            this._MenuFunctionService = _MenuFunctionService;
        }

        #endregion Khai báo services

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
            var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;

            var list = _ExcelColumnService.GetAllPagingByFirst(key, page, pageSize,ref totalPage,ref totalItem);

            var gridModel = PageModelCustom<ExcelColumnCustom>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
         
            TempData["url"] = Request.Url.AbsoluteUri;

            ViewBag.objId = (string)TempData["objId"];

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
        public ActionResult Create()
        {
            ViewBag.DDLMenu = GetMenuList();
            return View();
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
        public ActionResult Create(ExcelColumn obj, bool SaveAndCountinue = false)
        {
            //Danh sách sử dụng
            string url = (string)TempData["url"];
            ViewBag.DDLMenu = GetMenuList();
            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if(string.IsNullOrEmpty(obj.MenuFunctionId) || obj.MenuFunctionId.Equals("0"))
            {
                ModelState.AddModelError("MenuFunctionId", "Vui lòng chọn menu");
                return View(obj);
            }

            obj.Id = Common.GenerateId();

            //Thực hiện thêm mới
            var result = _ExcelColumnService.Create(obj);
            if (result.isSuccess)
            {
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
           
            ViewBag.PN = pageNumber;
            ViewBag.DDLMenu = GetMenuList();
            var obj = _ExcelColumnService.GetById(id);

          
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
        public ActionResult Update(ExcelColumn obj, string objId, int pageNumber = 1)
        {
            //Danh sách sử dụng
            string url = (string)TempData["url"];
            ViewBag.PN = pageNumber;
            ViewBag.DDLMenu = GetMenuList();

            //Kiểm tra
            var oldObj = _ExcelColumnService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (string.IsNullOrEmpty(obj.MenuFunctionId) || obj.MenuFunctionId.Equals("0"))
            {
                ModelState.AddModelError("MenuFunctionId", "Vui lòng chọn menu");
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.ColName = obj.ColName;
            oldObj.ColValue = obj.ColValue;
            oldObj.MenuFunctionId = obj.MenuFunctionId;
            oldObj.Active = obj.Active;

          
            //Thực hiện cập nhật
            var result = _ExcelColumnService.Update(oldObj);
            if (result.isSuccess)
            {
               TempData["objId"] = oldObj.Id.ToString();

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
            var result = _ExcelColumnService.DeleteById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        private List<MenuFunctionSubmit> GetMenuList()
        {
            var list = new List<MenuFunctionSubmit>
            {
                new MenuFunctionSubmit {  Id = "0", MenuName = "- Chọn danh mục -" }
            };
            var MenuList = _MenuFunctionService.GetAllActive();
            var parent = MenuList.Where(c => c.ParentId == "0");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.OrderNumber))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                }
            }
            return list;
        }

        private List<MenuFunctionSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<MenuFunctionSubmit> lst = new List<MenuFunctionSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _MenuFunctionService.GetAllChildByParentId(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                    }
                }
            }
            return lst;
        }
    }
}