using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class tblCustomer_Excel
    {
        public int NumberRow { get; set; }
        
        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string CustomerIdentify { get; set; } = "";
   

        public string CustomerAddress { get; set; } = "";

        public string CustomerMobile { get; set; } = "";

        public string CustomerGroupName { get; set; } = "";
        public string CompartmentId { get; set; } = "";
        //public string Cards { get; set; } = "";

        //public string Plates { get; set; } = "";

        public string Active { get; set; }
    }

    public class tblCustomer_ExcelTRANSERCO
    {
        public int NumberRow { get; set; }

        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string CustomerIdentify { get; set; } = "";
        public string ContractCode { get; set; } = "";

        public string CustomerAddress { get; set; } = "";

        public string CustomerMobile { get; set; } = "";

        public string CustomerGroupName { get; set; } = "";

        public string Cards { get; set; } = "";

        public string Plates { get; set; } = "";

        public string Active { get; set; }
    }
}
