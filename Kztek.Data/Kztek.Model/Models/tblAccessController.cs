using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessController
    {
        [Key]
        public Guid ControllerID { get; set; }

        [StringLength(50)]
        public string ControllerName { get; set; }

        [StringLength(50)]
        public string LineID { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public string ControllerGroupId { get; set; }
        public bool IsAddOutput { get; set; }
        public int NumberOutput { get; set; }

        public int FunctionController { get; set; }
    }
}
