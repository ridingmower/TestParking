using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblThaiSonErrorApiTemp
    {
        [Key]
        public string ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CardNumber { get; set; }
        public string CardNo { get; set; }
        public int Cost { get; set; }
        public DateTime EventTime { get; set; }
        public bool IsExpire { get; set; }
        public string PlateNumber { get; set; }
        public string UserID { get; set; }
        public string ReceiveBillCode { get; set; }   
        public int ErrorTimes { get; set; }
    }

    public class tblThaiSonErrorApiTemp_Custom
    {

        public Int32 RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CardNumber { get; set; }
        public string CardNo { get; set; }
        public int Cost { get; set; }
        public string EventTime { get; set; }
        public bool IsExpire { get; set; }
        public string PlateNumber { get; set; }
        public string UserID { get; set; }
        public string ReceiveBillCode { get; set; }
        public int ErrorTimes { get; set; }
    }
    public class tblThaiSonErrorApiTemp_Excel
    {

        public Int32 RowNumber { get; set; }
        public string CreatedDate { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string PlateNumber { get; set; }
        public string Username { get; set; }
        public string Cost { get; set; }
        public string Status { get; set; }
      
    }
}
