using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLockerController
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        public string ControllerName { get; set; }

        [StringLength(50)]
        public string LineID { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        public string Address { get; set; }
    }
}
