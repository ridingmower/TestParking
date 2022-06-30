using Kztek.Security;
using Kztek.Web.Core.Functions;
using System.Web.Mvc;

namespace Kztek.Web.Attributes
{
    public class CheckSessionLogin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!GetCurrentUser.CheckCurrentLogin())
            {
                filterContext.Result = new RedirectResult("/Login");
            }

            if (SecureDongleProvider.CheckHardKey() == false)
            {
                filterContext.Result = new RedirectResult("/Login");
            }
        }
    }
}