using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker
{
    public class LockerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Locker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Locker_default",
                "Locker/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Kztek.Web.Areas.Locker.Controllers" }
            );
        }
    }
}