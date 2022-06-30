using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class ActiveCardCustomViewModel
    {
        public string KeyWord { get; set; }

        public string CardGroup { get; set; }

        public string Date { get; set; }

        public string CustomerGroup { get; set; }


        public string DateExtend { get; set; }

        public string FeeLevel { get; set; }

        public string AccessLevel { get; set; }

        public bool isAllowNegativeDays { get; set; }

        public string DateActive { get; set; }

        public string fromdate { get; set; }

        public string todate { get; set; }

        public string AnotherKey { get; set; }

        public string arrCardID { get; set; }
        public bool isTransferPayment { get; set; }

        public string strIDCards { get; set; }
        public string TotalMoney { get; set; }
        public string Json { get; set; }
    }

    public class ExtendModel
    {
        public string Id { get; set; }

        public int Money { get; set; }

        public string OldDate { get; set; }
        public string NewDate { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }

        public string Json { get; set; }
    }
}
