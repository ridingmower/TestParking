using System.Web.Mvc;

namespace Kztek.Web.Areas.Access
{
    public class AccessAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Access";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Access_default",
                "Access/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Kztek.Web.Areas.Access.Controllers" }
            );
        }
    }
}