using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblAlarm
    {
        [Key]
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string UserID { get; set; }
        public string LaneID { get; set; }
        public string PicDir { get; set; }
        public string AlarmCode { get; set; }
        public string Description { get; set; }
    }
}
