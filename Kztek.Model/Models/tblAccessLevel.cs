using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessLevel
    {
        [Key]
        public Guid AccessLevelID { get; set; }

        [StringLength(50)]
        public string AccessLevelName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public bool Inactive { get; set; }
    }
}
