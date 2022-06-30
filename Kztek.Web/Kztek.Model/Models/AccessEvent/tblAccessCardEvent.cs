using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.AccessEvent
{
    public class tblAccessCardEvent
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string CardNumber { get; set; }
        public string ControllerID { get; set; }
        public string CardGroupID { get; set; }
        public int ReaderIndex { get; set; }
        public string EventStatus { get; set; }
        public string CardNo { get; set; }
        public string EventType { get; set; }
    }
}
