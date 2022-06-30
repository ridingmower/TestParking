using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblChangeEvent
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string EventID { get; set; }
        public string CardNumber { get; set; }
        public string UserID { get; set; }
        public bool IsDelete { get; set; }
    }
}
