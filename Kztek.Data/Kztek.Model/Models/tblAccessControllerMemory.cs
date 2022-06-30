using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessControllerMemory
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ControllerID { get; set; }

        public int MemoryID { get; set; }

        [StringLength(20)]
        public string CardNumber { get; set; }
    }
}
