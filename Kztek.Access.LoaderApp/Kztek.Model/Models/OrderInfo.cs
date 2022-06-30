using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class OrderInfo
    {
        [Key]
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public string PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }


    }
}
