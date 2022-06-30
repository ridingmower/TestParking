using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLockerProcess
    {
        [Key]
        public string Id { get; set; }

        public string LockerName { get; set; }

        public int LockerReaderIndex { get; set; }

        public string ControllerID { get; set; }

        public string CardNumber { get; set; }

        public string CardNo { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        public string ActionLocker { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }

    public class ReportLockerProcess
    {
        [Key]
        public string Id { get; set; }

        public string LockerName { get; set; }

        public int LockerReaderIndex { get; set; }

        public string ControllerID { get; set; }

        public string ControllerName { get; set; }

        public string CardNumber { get; set; }

        public string CardNo { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string ActionLocker { get; set; }

        public string Description { get; set; }

        public string RowNumber { get; set; }
    }
}
