using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblVoucher
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string EventID { get; set; }
        public string Voucher { get; set; }
    }
}
