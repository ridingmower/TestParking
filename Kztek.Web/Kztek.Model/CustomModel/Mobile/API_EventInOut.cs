using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.Model.CustomModel.Mobile
{
    public class API_EventInOut
    {
        public string CardNumber { get; set; }
        public string LaneId { get; set; }
        public string UserId { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}
