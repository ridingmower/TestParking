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
    public class AQUA_ActiveCardController : Controller
    {
        private ItblCardService _tblCardService;     
        private ItblActiveCardService _tblActiveCardService;
        private IOrderActiveCardService _OrderActiveCardService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblFeeService _tblFeeService;
        private ItblSystemConfigService _tblSystemConfigService;

        public AQUA_ActiveCardController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerGroupService _tblCustomerGroupService, ItblFeeService _tblFeeService, ItblActiveCardService _tblActiveCardService, ItblSystemConfigService _tblSystemConfigService, IOrderActiveCardService _OrderActiveCardService)
        {
            this._tblCardService = _tblCardService;
            this._OrderActiveCardService = _OrderActiveCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblFeeService = _tblFeeService;
            this._tblActiveCardService = _tblActiveCardService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [CheckSessionLogin]
        public ActionResult Index()
        {

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0);
            ViewBag.CustomerGroups = GetMenuList();

            return View();
        }

        [CheckSessionLogin]
        public ActionResult AQUAIndex()
        {

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0);
            ViewBag.CustomerGroups = GetMenuList();

            return View();
        }

        public PartialViewResult boxCards(string key = "", string newDateActive = "", string strIDCards = "", string address = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "", int page = 1)
        {
            var pageSize = 20;
            double totalmoney = 0;
            int total = 0;
            var customergroups = GetListChild("", customergroup);

            var list = _tblCardService.AQUA_GetAllPagingByFirstSQL(key, address, strIDCards, anotherkey, cardgroups, customergroups, fromdate, todate, newDateActive, ref totalmoney, ref total, page, pageSize);

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize, total);
            ViewBag.strIDCards = strIDCards;
            ViewBag.Total = totalmoney;
            return PartialView(gridModel);
        }

        public PartialViewResult AQUA_boxCards(string key = "", string newDateActive = "", string strIDCards = "", string address = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "", int page = 1)
        {
            var pageSize = 20;
            double totalmoney = 0;
            int total = 0;
            var customergroups = GetListChild("", customergroup);

            var list = _tblCardService.AQUA_GetAllPagingByFirstSQL(key, address, strIDCards, anotherkey, cardgroups, customergroups, fromdate, todate, newDateActive, ref totalmoney, ref total, page, pageSize);

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize, total);
            ViewBag.strIDCards = strIDCards;
            ViewBag.Total = totalmoney;
            return PartialView(gridModel);
        }


        public JsonResult GetListCardNumberExtendAll(string key = "", string newDateActive = "", string strIDCards = "", string address = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "")
        {
            
            var customergroups = GetListChild("", customergroup);

            var obj = _tblCardService.AQUA_GetListCardNumberExtendAll(key,strIDCards, anotherkey, cardgroups, customergroups, fromdate, todate, newDateActive);
          
            return Json(obj != null ? obj.CardNumber : "",JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult BoxMoneyByCardGroup(string key = "", string newDateActive = "", string strIDCards = "", string address = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "")
        {
            
            var customergroups = GetListChild("", customergroup);

            var list = _tblCardService.AQUA_MoneyByGroup(key, address, strIDCards, anotherkey, cardgroups, customergroups, fromdate, todate, newDateActive);

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0);

            return PartialView(list);
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

        public JsonResult AddNewListCardSub(string listId, string price)
        {
            var list = _tblCardService.GetAllActiveByListId(listId).ToList();

            if (list.Any())
            {
                GetSetFromSession(list, price);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<tblCardExtend> GetSetFromSession(List<tblCardExtend> list, string price)
        {
            var host = Request.Url.Host;

            var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
            if (listCardChoice != null)
            {
                foreach (var item in list)
                {
                    if (!listCardChoice.Any(n => n.CardID.ToString().Equals(item.CardID.ToString())))
                    {
                        item.Price = price;
                        listCardChoice.Add(item);
                    }                 
                }
            }
            else
            {
                foreach (var item in list)
                {
                    item.Price = price;                  
                }
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
                result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
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
                result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["LstDelSuccess"];
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
            var dateactive =   obj.DateActive != null ? (obj.DateActive) : DateTime.Now.ToString("yyyy/MM/dd");
            fee = "0";
            try
            {
                var order = new OrderActiveCard
                {
                    Id = Guid.NewGuid().ToString(),
                    DateCreated = DateTime.Now,
                    Price = 0
                };
                _OrderActiveCardService.Create(order);

                                                                                                                                                    var orderid = order != null ? order.Id : "";

                var customergroups = GetListChild("", obj.CustomerGroup);
                //Danh sách thẻ lấy theo query
                var isSuccess = _tblCardService.AQUA_AddCardExpire(obj.KeyWord, orderid, obj.strIDCards, obj.AnotherKey, obj.CardGroup, "", customergroups, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);

                if (isSuccess)
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                    result.isSuccess = isSuccess;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateFailed"];
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

        public JsonResult ExtendAllCard_v2(ActiveCardCustomViewModel obj)
        {
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var fee = obj.FeeLevel.Replace(".", "").Replace(",", "");
            var dateactive = Convert.ToDateTime(obj.DateActive);
            fee = "0";
            try
            {
                var order = new OrderActiveCard
                {
                    Id = Guid.NewGuid().ToString(),
                    DateCreated = DateTime.Now,
                    Price = !string.IsNullOrEmpty(obj.FeeLevel) ? Convert.ToInt32(obj.FeeLevel.Replace(".", "").Replace(",", "")) : 0
                };
                _OrderActiveCardService.Create(order);

                var orderid = order != null ? order.Id : "";

                var customergroups = GetListChild("", obj.CustomerGroup);
                //Danh sách thẻ lấy theo query
                var isSuccess = _tblCardService.AQUA_AddCardExpire_v2(obj.KeyWord, orderid, obj.strIDCards, obj.AnotherKey, obj.CardGroup, "", customergroups, 0, dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);

                if (isSuccess)
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                    result.isSuccess = isSuccess;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateFailed"];
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
            var cardnumbers = "";
            var host = Request.Url.Host;
            var fee = obj.FeeLevel.Replace(".", "").Replace(",", "");
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var dateactive = Convert.ToDateTime(obj.DateActive);

            fee = !string.IsNullOrWhiteSpace(fee) ? fee : "0";

            try
            {
                var list = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];
                if (list != null && list.Any())
                {
                    var count = 0;
                    

                    foreach (var item in list)
                    {
                        count++;
                        cardnumbers += string.Format("'{0}'{1}", item.CardNumber, list.Count == count ? "" : ",");
                    }

                    isSuccess = _tblCardService.AQUA_AddCardExpireByListCardNumber(cardnumbers, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);
                }
                else
                {
                    var result1 = new MessageReport();
                    result1.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["select_card_activate"];
                    result1.isSuccess = false;

                    return Json(result1, JsonRequestBehavior.AllowGet);
                }

                if (isSuccess)
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                    result.isSuccess = isSuccess;

                    Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)] = new List<tblCardExtend>();

                    var model = new { result = result, cardnumbers = cardnumbers };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new MessageReport();
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateFailed"];
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
        /// <summary>
        /// bỏ tích từng dòng
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="price"></param>
        /// <param name="priceUnCheck"></param>
        /// <param name="strIDCards"></param>
        /// <returns></returns>
        public JsonResult UnCheckExtendCard(string Id, int price, int priceUnCheck, string strIDCards)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(strIDCards))
            {
                list = strIDCards.Split(',').ToList();
            }

            if (list.Contains(Id))
            {
                list.Remove(Id);
                priceUnCheck = priceUnCheck - price;
            }
            else
            {
                list.Add(Id);
                priceUnCheck = priceUnCheck + price;
            }

            strIDCards = string.Join(",", list);

            var result = new
            {
                strIDCards = strIDCards,
                priceUnCheck = priceUnCheck
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// bỏ tích tất cả
        /// </summary>
        /// <param name="strIds"></param>
        /// <param name="isCheck"></param>
        /// <param name="priceUnCheck"></param>
        /// <param name="strIDCards"></param>
        /// <returns></returns>
        public JsonResult UnCheckALL(string strIds, bool isCheck, int priceUnCheck, string strIDCards)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(strIDCards))
            {
                list = strIDCards.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(strIds))
            {
                var listobj = strIds.Split(',').ToList();

                if(listobj != null && listobj.Count > 0)
                {
                    foreach(var item in listobj)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var array = item.Split('/');
                            if (list.Contains(array[0]))
                            {
                                if (isCheck)
                                {
                                    list.Remove(array[0]);
                                    priceUnCheck = priceUnCheck - Convert.ToInt32(array[1]);
                                }
                            }
                            else
                            {
                                if (!isCheck)
                                {
                                    list.Add(array[0]);                                
                                    priceUnCheck = priceUnCheck + Convert.ToInt32(array[1]);
                                }                               
                            }
                        }
                    }
                }
               
            }           

            strIDCards = string.Join(",", list);

            var result = new
            {
                strIDCards = strIDCards,
                priceUnCheck = priceUnCheck
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckFeeCardGroup(string CardGroupID)
        {
            var result = new MessageReport(true, "");

            if (!string.IsNullOrEmpty(CardGroupID))
            {
                var objFee = _tblFeeService.GetByCateId_Extend(CardGroupID);
                if (objFee == null)
                {
                    result = new MessageReport(false, "Nhóm thẻ chưa có thông tin phí thuê bao.");
                }
            }
            else
            {
                result = new MessageReport(false, "Vui lòng chọn nhóm thẻ");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TotalPriceBoxCardChoice()
        {
            var host = Request.Url.Host;
            var listCardChoice = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];

            var totalprice = 0;
            if (listCardChoice != null && listCardChoice.Count > 0)
            {
                foreach (var item in listCardChoice)
                {
                    totalprice += Convert.ToInt32(item.Price);
                }
            }

            return Json(totalprice > 0 ? totalprice.ToString("###,###") : "0", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Tính tiền / ngày
        /// </summary>
        /// <param name="objFee"></param>
        /// <returns></returns>
        int GetPriceOfDay(tblFee objFee)
        {
            var price = 0;

            if (objFee != null)
            {
                var arrUnit = objFee.Units.Split('_');
                switch (arrUnit[1].ToString())
                {
                    case "Ngày":
                        price = objFee.FeeLevel / Convert.ToInt32(arrUnit[0]);
                        break;
                    case "Tháng":
                        price = objFee.FeeLevel / (Convert.ToInt32(arrUnit[0]) * 30);
                        break;
                    case "Quý":
                        price = objFee.FeeLevel / (Convert.ToInt32(arrUnit[0]) * 90);
                        break;
                    case "Năm":
                        price = objFee.FeeLevel / (Convert.ToInt32(arrUnit[0]) * 365);
                        break;
                }
            }

            return price;
        }

        /// <summary>
        /// In hóa đơn
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintBill(string cardnumbers,string type,string id)
        {
            ViewBag.Card = cardnumbers;

            var list = new List<tblActiveCardCustomViewModel>();

            //in tại form gia hạn
            if(type == "1")
            {
                var orderid = _tblActiveCardService.GetOrderIdByCardNumbers(cardnumbers);
                //list = _tblActiveCardService.GetBill(cardnumbers);
                list = _tblActiveCardService.GetBill_v2(orderid);
            }
            else
            {
                //in tại danh sách biên lai
                list = _tblActiveCardService.GetBill_v2(id);
            }
          
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(list);
        }

        public ActionResult PrintBill_v2(string cardnumbers, string type, string id)
        {
            ViewBag.Card = cardnumbers;

            var list = new List<tblActiveCardCustomViewModel>();
            var objOrder = new OrderActiveCard();
            //in tại form gia hạn
            if (type == "1")
            {
                var orderid = _tblActiveCardService.GetOrderIdByCardNumbers(cardnumbers);
                //list = _tblActiveCardService.GetBill(cardnumbers);
                list = _tblActiveCardService.GetBill_v2(orderid);
                objOrder = _OrderActiveCardService.GetById(orderid);
            }
            else
            {
                //in tại danh sách biên lai
                list = _tblActiveCardService.GetBill_v2(id);
                objOrder = _OrderActiveCardService.GetById(id);
            }

            ViewBag.TotalPrice = objOrder != null ? objOrder.Price : 0;
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(list);
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
                return Json(fee.FeeLevel.ToString("###,###") + " / " + fee.Units.Replace("_"," "), JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}