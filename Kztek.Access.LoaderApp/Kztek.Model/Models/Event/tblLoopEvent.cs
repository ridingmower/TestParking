using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblLoopEvent
    {
        [Key]
        public System.Guid Id { get; set; }
        public string EventCode { get; set; }
        public string Plate { get; set; }
        public Nullable<System.DateTime> DatetimeIn { get; set; }
        public Nullable<System.DateTime> DatetimeOut { get; set; }
        public string PicDirIn { get; set; }
        public string PicDirOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string CustomerName { get; set; }
        public bool IsEditPlateIn { get; set; }
        public bool IsEditPlateOut { get; set; }
        public decimal Moneys { get; set; }
        public bool IsFree { get; set; }
        public string FreeType { get; set; }
        public string CarType { get; set; }
        public bool IsDelete { get; set; }
        public string Voucher { get; set; }
    }
}
