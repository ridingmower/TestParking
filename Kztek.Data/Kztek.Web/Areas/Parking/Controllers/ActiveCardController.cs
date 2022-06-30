using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class ActiveCardController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblFeeService _tblFeeService;
        private ItblSystemConfigService _tblSystemConfigService;
        private IExtendCardService _ExtendCardService;
        public ActiveCardController(ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerGroupService _tblCustomerGroupService, ItblFeeService _tblFeeService, ItblSystemConfigService _tblSystemConfigService, IExtendCardService _ExtendCardService)
        {
            this._tblCardService = _tblCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblFeeService = _tblFeeService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._ExtendCardService = _ExtendCardService;
        }

        [CheckSessionLogin]
        public ActionResult Index()
        {

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0 || n.CardType == 3);
            ViewBag.CustomerGroups = GetMenuList();
            var systemconfig = _tblSystemConfigService.GetDefault();

            ViewBag.ISCOMA = systemconfig != null ? (systemconfig.FeeName.Contains("COMA6") ? true : false) : false;
            return View();
        }
        [CheckSessionLogin]
        public ActionResult Extend()
        {

            ViewBag.CardGroups = GetCardGroup().Where(n => n.CardType == 0 || n.CardType == 3);
            ViewBag.CustomerGroups = GetMenuList();
            var systemconfig = _tblSystemConfigService.GetDefault();

            ViewBag.ISCOMA = systemconfig != null ? (systemconfig.FeeName.Contains("COMA6") ? true : false) : false;
            return View();
        }

        public PartialViewResult boxCards(string key = "", string anotherkey = "", string cardgroups = "", string customergroup = "", string fromdate = "", string todate = "", int page = 1)
        {
            var pageSize = 20;
            int total = 0;
            var customergroups = GetListChild("", customergroup);

            //var list = _tblCardService.GetAllPagingByFirst(key, anotherkey, cardgroups, customergroups, fromdate, todate, page, pageSize);

            //var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize);

            var list = _tblCardService.GetAllPagingByFirst_SQL(key, anotherkey, cardgroups, customergroups, fromdate, todate,ref total, page, pageSize);

            var gridModel = PageModelCustom<tblCardExtend>.GetPage(list, page, pageSize, total);

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
            var dateactive = Convert.ToDateTime(obj.DateActive);

            try
            {
                //Danh sách thẻ lấy theo query
                var isSuccess = _tblCardService.AddCardExpire(obj.KeyWord, obj.AnotherKey, obj.CardGroup,"", obj.CustomerGroup, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);

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
                    var cardnumbers = "";

                    foreach (var item in list)
                    {
                        count++;
                        cardnumbers += string.Format("'{0}'{1}", item.CardNumber, list.Count == count ? "" : ",");
                    }

                    isSuccess = _tblCardService.AddCardExpireByListCardNumber(cardnumbers, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);
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


        public JsonResult ExtendAllOneMonth(ActiveCardCustomViewModel obj)
        {
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var fee = obj.FeeLevel.Replace(".", "").Replace(",", "");
            var dateactive = Convert.ToDateTime(obj.DateActive);

            try
            {
                //Danh sách thẻ lấy theo query
                var isSuccess = _tblCardService.AddCardExpireOneMonth(obj.KeyWord, obj.AnotherKey, obj.CardGroup, "", obj.CustomerGroup, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);

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


        public JsonResult ExtendSelectedOneMonth(ActiveCardCustomViewModel obj)
        {
            var isSuccess = false;
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
                    var cardnumbers = "";

                    foreach (var item in list)
                    {
                        count++;
                        cardnumbers += string.Format("'{0}'{1}", item.CardNumber, list.Count == count ? "" : ",");
                    }

                    isSuccess = _tblCardService.AddCardExpireByListCardNumberOneMonth(cardnumbers, int.Parse(fee), dateextend, GetCurrentUser.GetUser().Id, obj.isAllowNegativeDays);
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

        public PartialViewResult Partial_Month(string datenew,string datestart)
        {
            var list = new List<ExtendModel>();
            var datenow = !string.IsNullOrEmpty(datestart) ? Convert.ToDateTime(datestart) : DateTime.Now;
            var dateextend = Convert.ToDateTime(datenew);

            if (dateextend.Date >= datenow.Date)
            {
                int months = dateextend.Subtract(datenow).Days / 30 + 1;
                var a = Enumerable.Range(0, months)
        .Select(offset => datenow.AddMonths(offset))
        .ToList();

                var dateend = a.FirstOrDefault(n => n == dateextend);
                if(dateend == DateTime.MinValue)
                {
                    a.Add(dateextend);
                }
               

                if (a != null && a.Count > 0)
                {
                    for(int i = 0; i < a.Count - 1; i++)
                    {
                        list.Add(new ExtendModel { Id = Guid.NewGuid().ToString(),OldDate = a[i].ToString("dd/MM/yyyy"), NewDate = a[i + 1].ToString("dd/MM/yyyy"), Money = 0,Date = DateTime.Now.ToString("dd/MM/yyyy") });
                    }
                }
            }

            ViewBag.Json = JsonConvert.SerializeObject(list);

            return PartialView(list);
        }

        /// <summary>
        /// gia hạn bên phải
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult ExtendSelectedCardV2(ActiveCardCustomViewModel obj)
        {
            var host = Request.Url.Host;      
            try
            {
                //danh sách thẻ đã chọn
                var list = (List<tblCardExtend>)Session[string.Format("{0}_{1}", SessionConfig.CardActiveParkingSession, host)];

                //gia hạn
                var result = Extend(list.Select(n => n.CardNumber).ToList(), obj);

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

        /// <summary>
        /// gia hạn bên trái
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult ExtendAllCardV2(ActiveCardCustomViewModel obj)
        {
            try
            {
                var customergroups = GetListChild("", obj.CustomerGroup);

                var list = _tblCardService.GetCard(obj.KeyWord, obj.AnotherKey, obj.CardGroup, customergroups, "", "");
                
                //gia hạn
                var result = Extend(list,obj);

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

        /// <summary>
        /// thực hiện gia hạn
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        MessageReport Extend(List<string> list, ActiveCardCustomViewModel obj)
        {
            bool isSuccess = false;
            var dateextend = Convert.ToDateTime(obj.DateExtend).ToString("MM/dd/yyyy");
            var user = GetCurrentUser.GetUser().Id;
            var host = Request.Url.Host;
            int money = 0;
            var result1 = new MessageReport(false, FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateFailed"]);

            if (list != null && list.Any())
            {
                var count = 0;

                //thêm gia hạn từng thẻ
                foreach (var item in list)
                {
                    var subid = Guid.NewGuid().ToString();
                    var cardnumber = string.Format("'{0}'", item);
                    count++;

                    //chi tiết từng tháng của thẻ
                    if (!string.IsNullOrEmpty(obj.Json))
                    {
                        var datas = JsonConvert.DeserializeObject<List<ExtendModel>>(obj.Json);

                        if (datas != null && datas.Count > 0)
                        {
                            foreach (var itemM in datas)
                            {
                                //tổng tiền
                                money += itemM.Money;

                                //thêm từng tháng
                                _ExtendCardService.AddNew(cardnumber, itemM.Money, itemM.OldDate, itemM.NewDate,itemM.Date, user, obj.isAllowNegativeDays, subid, Guid.NewGuid().ToString(), dateextend);
                            }

                        }
                    }

                    isSuccess = _tblCardService.AddCardExpireByListCardNumber_V2(cardnumber, money, dateextend, user, obj.isAllowNegativeDays, false, "", subid);

                    money = 0;
                }


            }

            if (isSuccess)
            {
                result1 = new MessageReport(true, FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"]);
            }

            return result1;
        }

        public JsonResult ChangeDateMoney(ExtendModel obj)
        {
            var datas = new List<ExtendModel>();
            if (!string.IsNullOrEmpty(obj.Json))
            {
                datas = JsonConvert.DeserializeObject<List<ExtendModel>>(obj.Json);

                if (datas != null && datas.Count > 0)
                {
                    var model = datas.FirstOrDefault(n => n.Id == obj.Id);
                    if (model != null)
                    {
                        if (obj.Type == "1")
                        {
                            model.Money = obj.Money;
                        }
                        else
                        {
                            model.Date = obj.Date;
                        }

                    }

                }
            }

            var str = JsonConvert.SerializeObject(datas);

            return Json(str, JsonRequestBehavior.AllowGet);
        }
    }
}