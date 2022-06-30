using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident
{
    public class ResidentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Resident";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Resident_default",
                "Resident/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Kztek.Web.Areas.Resident.Controllers" }
            );
        }
    }
}