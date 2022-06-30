using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblThaiSonSuccessApi
    {
        [Key]
        public string ID { get; set; }
        public DateTime FirstSendTime { get; set; }
        public DateTime SuccessTime { get; set; }
        public int SendType { get; set; }       
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public int Cost { get; set; }
        public string Response { get; set; }
        public string PlateNumber { get; set; }
        public string UserID { get; set; }
        public string ReceiveBillCode { get; set; }
        public int ErrorTimes { get; set; }
      
    }
    public class tblThaiSonSuccessApi_custom
    {
       
        public Int32 RowNumber { get; set; }
        public DateTime FirstSendTime { get; set; }
        public string ReceiveBillCode { get; set; }
        public string UserID { get; set; }
        public int SendType { get; set; }
        public int ErrorTimes { get; set; }
        public string PlateNumber { get; set; }
        public DateTime SuccessTime { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public int Cost { get; set; }
        //public string Response { get; set; }

    }
    public class tblThaiSonSuccessApi_Excel
    {

        public Int32 RowNumber { get; set; }
        public string FirstSendTime { get; set; }
        public string ReceiveBillCode { get; set; }
        public string UserName { get; set; }
        public string SendTypeName { get; set; }
        public int ErrorTimes { get; set; }
        public string PlateNumber { get; set; }
        public string SuccessTime { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Cost { get; set; }
        //public string Response { get; set; }

    }
}
