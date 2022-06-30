using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class CardCustom
    {
        public string CardNumber { get; set; }
        public string CardNo { get; set; }
        public string Plate1 { get; set; }
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string CustomerCode { get; set; }
        public string CardGrname{ get; set; }
    }
    public class CustomerCus
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

    }
}
