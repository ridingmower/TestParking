using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblDispenser
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string ControllerID { get; set; }
        public string ControllerName { get; set; }
        public string State { get; set; }
    }
}
