using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessDoor
    {
        [Key]
        public Guid DoorID { get; set; }

        [StringLength(50)]
        public string DoorName { get; set; }

        [StringLength(50)]
        public string ControllerID { get; set; }

        [StringLength(50)]
        public string ReaderIndex { get; set; }

        public bool? Inactive { get; set; }

        public int Ordering { get; set; }
    }
}
