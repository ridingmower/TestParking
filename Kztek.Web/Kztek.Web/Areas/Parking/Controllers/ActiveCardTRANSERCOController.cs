using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ActiveCardTRANSERCOController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblFeeService _tblFeeService;

        public ActiveCardTRANSERCOController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerGroupService _tblCustomerGroupService, ItblFeeService _tblFeeService)
        {
            this._tblCardService = _tblCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblFeeService = _tblFeeService;
        }

        [CheckSessionLogin]
        public ActionResult Index()
        {

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0);
            ViewBag.CustomerGroups = GetMenuList();

            return View();
        }

        public PartialViewResult boxCards(string key = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "", int page = 1)
        {
            var pageSize = 20;
            var customergroups = GetListChild("", customergroup);

            var list = _tblCardService.GetAllPagingByFirst(key, anotherkey, cardgroups, customergroups, fromdate, todate, page, pageSize);

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize);

            return PartialView(gridModel);
        }

        public PartialViewResult boxCardChoices(string key = "")
        {
            var host = Request.Url.Host;
            var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
            if (!string.IsNullOrWhiteSpace(key))
            {
                listCardChoice = listCardChoice.Where(n => n.CardNumber.Contains(key) || n.CardNo.Contains(key)).ToList();
            }

            return PartialView(listCardChoice);
        }

        public JsonResult AddNewListCardSub(string listId)
        {
            var list = _tblCardService.GetAllActiveByListId(listId).ToList();

            if (list.Any())
            {
                GetSetFromSession(list);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<tblCardExtend> GetSetFromSession(List<tblCardExtend> list)
        {
            var host = Request.Url.Host;

            var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
            if (listCardChoice != null)
            {
                foreach (var item in list)
                {
                    if (!listCardChoice.Any(n => n.CardID.ToString().Equals(item.CardID.ToString())))
                    {
                        listCardChoice.Add(item);
                    }
                }
            }
            else
            {
                listCardChoice = list;
            }

            Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)] = listCardChoice;

            return listCardChoice;
        }

        public JsonResult DeleteOneSelectedCard(string id)
        {
            try
            {
                var host = Request.Url.Host;
                var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
                var listMap = listCardChoice.ToList();

                if (listCardChoice.Any())
                {
                    foreach (var item in listMap)
                    {
                        if (item.CardID.ToString().Equals(id))
                        {
                            listCardChoice.Remove(item);
                        }
                    }
                }

                Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)] = listCardChoice;

                var result = new MessageReport();
                result.Message = "Xóa thành công";
                result.isSuccess = true;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAllSelectedCard()
        {
            try
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)] = new List<tblCardExtend>();

                var result = new MessageReport();
                result.Message = "Xóa tất cả danh sách thành công";
                result.isSuccess = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ExtendAllCard(ActiveCardCustomViewModel obj)
        {
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var fee = obj.FeeLevel.Replace(".", "").Replace(",", "");
            var dateactive = Convert.ToDateTime(obj.DateActive);

            try
            {
                //Danh sách thẻ lấy theo query
                var isSuccess = _tblCardService.AddCardExpireTRANSERCO(obj.KeyWord,obj.fromdate,obj.todate, obj.AnotherKey, obj.CardGroup, "", obj.CustomerGroup, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays, obj.isTransferPayment);

                if (isSuccess)
                {
                    var result = new MessageReport();
                    result.Message = "Cập nhật thành công";
                    result.isSuccess = isSuccess;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new MessageReport();
                    result.Message = "Không thành công";
                    result.isSuccess = isSuccess;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ExtendSelectedCard(ActiveCardCustomViewModel obj)
        {
            var isSuccess = false;
            var host = Request.Url.Host;
            var fee = obj.FeeLevel.Replace(".", "").Replace(",", "");
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var dateactive = Convert.ToDateTime(obj.DateActive);
            var newarr = "(";
            if (!string.IsNullOrEmpty(obj.arrCardID))
            {
                var arr = obj.arrCardID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (arr.Length > 0)
                {
                    int count = 0;
                    foreach (var _ar in arr)
                    {
                        count++;
                        if (!string.IsNullOrEmpty(_ar))
                        {
                            newarr += "'" + _ar + "'" + (count < arr.Length ? "," : ")");                          
                        }
                    }
                }
            }

            fee = !string.IsNullOrWhiteSpace(fee) ? fee : "0";

            try
            {
                var list = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
                if (list != null && list.Any())
                {
                    var count = 0;
                    var cardnumbers = "";

                    foreach (var item in list)
                    {
                        count++;
                        cardnumbers += string.Format("'{0}'{1}", item.CardNumber, list.Count == count ? "" : ",");
                    }

                    isSuccess = _tblCardService.AddCardExpireByListCardNumberTRANSERCO(cardnumbers, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays, newarr);
                }
                else
                {
                    var result1 = new MessageReport();
                    result1.Message = "Vui lòng chọn thẻ muốn gia hạn";
                    result1.isSuccess = false;

                    return Json(result1, JsonRequestBehavior.AllowGet);
                }

                if (isSuccess)
                {
                    var result = new MessageReport();
                    result.Message = "Cập nhật thành công";
                    result.isSuccess = isSuccess;

                    Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)] = new List<tblCardExtend>();

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new MessageReport();
                    result.Message = "Không thành công";
                    result.isSuccess = isSuccess;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var result = new MessageReport();
                result.Message = ex.Message;
                result.isSuccess = false;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }







        #region Danh sách hõ trợ
        public IEnumerable<tblCardGroup> GetCardGroup()
        {
            return _tblCardGroupService.GetAllActive();
        }

        /// <summary>
        /// Danh sách menu cấp cha
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <returns></returns>
        private List<SelectListModel> GetMenuList()
        {
            var list = new List<SelectListModel>();
            //{
            //    new SelectListModel {  ItemValue = "", ItemText = "- Chọn danh mục -" }
            //};
            var MenuList = _tblCustomerGroupService.GetAllActive().ToList();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.SortOrder))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new SelectListModel { ItemValue = item1.ItemValue.ToString(), ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Đệ quy để lấy danh sách con
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private List<SelectListModel> Children(string parentID)
        {
            //Khai báo danh sách
            List<SelectListModel> lst = new List<SelectListModel>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _tblCustomerGroupService.GetAllChildByParentID(parentID.ToString()).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new SelectListModel { ItemValue = item1.ItemValue, ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                    }
                }
            }
            return lst;
        }

        private string GetListChild(string str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += id + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.CustomerGroupID.ToString() + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }
        #endregion

        public JsonResult FillFee(string id)
        {
            var fee = _tblFeeService.GetByCateId(id);
            if (fee != null)
            {
                return Json(fee.FeeLevel.ToString("###,###"), JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}