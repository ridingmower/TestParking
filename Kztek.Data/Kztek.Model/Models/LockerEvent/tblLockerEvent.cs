using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.LockerEvent
{
    public class tblLockerEvent
    {
        [Key]
        public string Id { get; set; }

        public string EventCode { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }

        public string LockerIndex { get; set; }

        public string ControllerID { get; set; }

        public string FaceID { get; set; }

        public string PicIn { get; set; }

        public string PicOut { get; set; }

        public string EventType { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }

        public string RegisterId { get; set; }

        public string EventStatus { get; set; }
    }

    public class tblLockerEvent_Report
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }

        public string EventCode { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }

        public string LockerIndex { get; set; }

        public string ControllerID { get; set; }

        public string FaceID { get; set; }

        public string PicIn { get; set; }

        public string PicOut { get; set; }

        public string EventType { get; set; }

        public DateTime DateCreated { get; set; }
        public string DateCreatedValue { get; set; }
        public bool IsDeleted { get; set; }

        public string RegisterId { get; set; }
    }
}
