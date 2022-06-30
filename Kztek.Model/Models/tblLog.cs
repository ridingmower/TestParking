using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLog
    {
        [Key]
        public System.Guid LogID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string UserName { get; set; }
        public string AppCode { get; set; }
        public string SubSystemCode { get; set; }
        public string ObjectName { get; set; }
        public string Actions { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public string ComputerName { get; set; }
        public string OldInfo { get; set; }
        public string NewInfo { get; set; }
    }
}
