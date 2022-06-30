using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblPrintIndex
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string EventID { get; set; }
        public Nullable<int> PrintIndex { get; set; }
        public string Para3 { get; set; }
        public string Para1 { get; set; }
        public string Para2 { get; set; }
    }
}
