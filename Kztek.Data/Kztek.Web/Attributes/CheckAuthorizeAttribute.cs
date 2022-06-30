using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Kztek.Data;
using Kztek.Web.Core.Functions;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;

namespace Kztek.Web.Attributes
{
    public class CheckAuthorizeAttribute : AuthorizeAttribute
    {
        private KztekEntities db = new KztekEntities();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = GetCurrentUser.GetUser();

            if (user != null)
            {
                if (user.Admin)
                {
                    return true;
                }
                else
                {
                    //Lấy controller và action
                    var handler = HttpContext.Current.Handler as System.Web.Mvc.MvcHandler;
                    var controller = handler.RequestContext.RouteData.Values["controller"];
                    var action = handler.RequestContext.RouteData.Values["action"];
                    //Lấy bản ghi menu có controller + action
                    var list = GetAndSetMenuFunctionFromCache();

                    var objMenuFunction = list.FirstOrDefault(n => n.ControllerName.Equals(controller) && n.ActionName.Equals(action));
                    if (objMenuFunction != null)
                    {
                        var lstRole = GetAndSetRoleFromCacheByUserId(user.Id);

                        if (lstRole.Any())
                        {
                            foreach (var item in lstRole)
                            {
                                var lstMenuRole = GetAndSetMenuFromCacheByRoleId(item.RoleId);

                                if (lstMenuRole.Any())
                                {
                                    foreach (var item1 in lstMenuRole)
                                    {
                                        if (item1.MenuId.Equals(objMenuFunction.Id))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/NotAuthorize.cshtml"
            };
        }

        private List<MenuFunction> GetAndSetMenuFunctionFromCache()
        {
            //Danh sách menu
            var list = new List<MenuFunction>();

            if (CacheLayer.Exists(ConstField.AllListMenuFunctionCache))
            {
                list = CacheLayer.Get<List<MenuFunction>>(ConstField.AllListMenuFunctionCache);
            }
            else
            {
                var query = from n in db.MenuFunctions
                            where n.Active && !n.Deleted
                            select n;

                list = query.ToList();
                CacheLayer.Add(ConstField.AllListMenuFunctionCache, list, ConstField.TimeCache);
            }

            return list;
        }

        private List<UserRole> GetAndSetRoleFromCacheByUserId(string userid)
        {
            var formatUser = string.Format("{0}_{1}", ConstField.ListUserRole, userid);

            var list = new List<UserRole>();

            if (CacheLayer.Exists(formatUser))
            {
                list = CacheLayer.Get<List<UserRole>>(formatUser);
            }
            else
            {
                var query = from n in db.UserRoles
                            where n.UserId.Equals(userid)
                            select n;

                list = query.ToList();
                CacheLayer.Add(formatUser, list, ConstField.TimeCache);
            }

            return list;
        }

        private List<RoleMenu> GetAndSetMenuFromCacheByRoleId(string roleid)
        {
            var formatRole = string.Format("{0}_{1}", ConstField.ListRoleMenu, roleid);

            var list = new List<RoleMenu>();

            if (CacheLayer.Exists(formatRole))
            {
                list = CacheLayer.Get<List<RoleMenu>>(formatRole);
            }
            else
            {
                var query = from n in db.RoleMenus
                            where n.RoleId.Equals(roleid)
                            select n;

                list = query.ToList();
                CacheLayer.Add(formatRole, list, ConstField.TimeCache);
            }

            return list;
        }
    }
}
