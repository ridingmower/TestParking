using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Payment
{
    public class tblEventPayment
    {
        [Key]
        public string EventId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public string Plate { get; set; }

        public int Money { get; set; }

        public string OrderId { get; set; }

        public int PaymentStatus { get; set; }

        public bool isSuccessQRCode { get; set; } //isSuccess on send request 

        public bool isSuccessPay { get; set; }

        public string ResponseContentQRCode { get; set; } //Content api response

        public string ResponseContentPay { get; set; }
    }
}
