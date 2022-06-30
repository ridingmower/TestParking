using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLockerPC
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        public string PCName { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
