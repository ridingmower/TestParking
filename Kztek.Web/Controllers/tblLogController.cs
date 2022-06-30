using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class tblLogController : Controller
    {
        private ItblLogService _tblLogService;
        private IUserService _UserService;

        public tblLogController(ItblLogService _tblLogService, IUserService _UserService)
        {
            this._tblLogService = _tblLogService;
            this._UserService = _UserService;
        }

        public ActionResult Index(string key, string users, string actions, string fromdate, string todate, int page = 1, string group = "")
        {
            var name = FunctionHelper.getCurrentGroup(group);

            if (string.IsNullOrWhiteSpace(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrWhiteSpace(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var pageSize = 20;

            //Lấy danh sách phân trang
            var list = _tblLogService.GetAllPagingByFirst(key, users, actions, fromdate, todate, page, pageSize, name);

            //Đổ lên grid
            var gridModel = PageModelCustom<tblLog>.GetPage(list, page, pageSize);

            ViewBag.keyValue = key;
            ViewBag.usersValue = users;
            ViewBag.actionsValue = actions;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.GroupID = group;

            ViewBag.ActionValues = ActionList().ToDataTableNullable();
            ViewBag.UserValues = UserList().ToDataTableNullable();

            return View(gridModel);
        }

        private List<SelectListModel> ActionList()
        {
            var list = new List<SelectListModel>();

            list.Add(new SelectListModel { ItemText = ActionConfigO.Create, ItemValue = ActionConfigO.Create });
            list.Add(new SelectListModel { ItemText = ActionConfigO.Update, ItemValue = ActionConfigO.Update });
            list.Add(new SelectListModel { ItemText = ActionConfigO.Delete, ItemValue = ActionConfigO.Delete });
            list.Add(new SelectListModel { ItemText = ActionConfigO.Login, ItemValue = ActionConfigO.Login });

            return list;
        }

        private IEnumerable<User> UserList()
        {
            return _UserService.GetAllActive();
        }
    }
}