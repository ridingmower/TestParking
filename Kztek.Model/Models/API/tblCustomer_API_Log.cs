using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class tblCustomer_API_Log
    {
        public string Id { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public string Mobile { get; set; }
        public string CustomerGroupID { get; set; }
        public string SubmitMessage { get; set; }
        public int SubmitStatus { get; set; }
        public string HTTP { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
