using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.LockerEvent
{
    public class tblLockerAlarm
    {
        [Key]
        public string Id { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }

        public string LockerIndex { get; set; }

        public string ControllerID { get; set; }

        public string PicDir { get; set; }

        public string FaceID { get; set; }

        public string EventType { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }

        public string AlarmCode { get; set; }

        public int EventCode { get; set; }
    }

    public class tblLockerAlarmReport
    {
        public string Id { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }

        public string CardGruopName { get; set; }

        public string LockerIndex { get; set; }

        public string ControllerID { get; set; }

        public string ControllerName { get; set; }

        public string PicDir { get; set; }

        public string FaceID { get; set; }

        public string EventType { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }

        public string AlarmCode { get; set; }

        public string RowNumber { get; set; }

        public int EventCode { get; set; }
    }
}
