using Kztek.Data.Infrastructure;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Attributes
{
    public class CheckAuthorizeHomeInGroup: System.Web.Mvc.AuthorizeAttribute
    {
        public string GroupId { get; set; }

        //private Repository repository = new Repository();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = GetCurrentUser.GetUser();

            if (user != null)
            {
                if (user.Admin)
                {
                    var groupId = ConfigurationManager.AppSettings["FunctionGroupAllow"].ToString();
                    if (groupId.Contains(GroupId))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var groupId = ConfigurationManager.AppSettings["FunctionGroupAllow"].ToString();
                    if (!groupId.Contains(GroupId))
                    {
                        return false;
                    }

                    //if (!user.GroupAllowList.Contains(GroupId))
                    //{
                    //    return false;
                    //}

                    return true;
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
    }
}