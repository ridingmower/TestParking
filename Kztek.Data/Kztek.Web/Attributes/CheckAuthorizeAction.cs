using Kztek.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Kztek.Web.Core.Functions;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;

namespace Kztek.Web.Attributes
{
    public class CheckAuthorizeAction
    {
        public bool Check { get; set; }

        private static KztekEntities db = new KztekEntities();

        private static CheckAuthorizeAction _Instance = new CheckAuthorizeAction();
        public static CheckAuthorizeAction Instance
        {
            get { return CheckAuthorizeAction._Instance; }
            set { CheckAuthorizeAction._Instance = value; }
        }

        public void CheckPemission(string ControllerName, string ActionName)
        {
            this.Check = false;

            var user = GetCurrentUser.GetUser();
            if (user != null)
            {
                if (user.Admin)
                {
                    this.Check = true;
                }
                else
                {
                    //Lấy bản ghi menu có controller + action
                    var list = GetAndSetMenuFunctionFromCache();

                    var objMenuFunction = list.FirstOrDefault(n => n.ControllerName.Equals(ControllerName) && n.ActionName.Equals(ActionName));
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
                                            this.Check = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool CheckActionPemission(string ControllerName, string ActionName)
        {
            var isCheck = false;

            var user = GetCurrentUser.GetUser();
            if (user != null)
            {
                if (user.Admin)
                {
                    isCheck = true;
                }
                else
                {
                    //Lấy bản ghi menu có controller + action
                    var list = GetAndSetMenuFunctionFromCache();

                    var objMenuFunction = list.FirstOrDefault(n => n.ControllerName.Equals(ControllerName) && n.ActionName.Equals(ActionName));
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
                                            isCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return isCheck;
        }

        private static List<MenuFunction> GetAndSetMenuFunctionFromCache()
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

        private static List<UserRole> GetAndSetRoleFromCacheByUserId(string userid)
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

        private static List<RoleMenu> GetAndSetMenuFromCacheByRoleId(string roleid)
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