using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking
{
    public class ParkingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Parking";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Parking_default",
                "Parking/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Kztek.Web.Areas.Parking.Controllers" }
            );
        }
    }
}