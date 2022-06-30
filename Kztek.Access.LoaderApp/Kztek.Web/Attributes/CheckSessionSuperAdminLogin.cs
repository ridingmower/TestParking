using System;
using System.Web.Mvc;
using Kztek.Web.Core.Functions;

namespace Kztek.Web.Attributes
{
    public class CheckSessionSuperAdminLogin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!GetCurrentSuperAdmin.CheckCurrentLogin())
            {
                filterContext.Result = new RedirectResult("/Login");
            }
        }
    }
}