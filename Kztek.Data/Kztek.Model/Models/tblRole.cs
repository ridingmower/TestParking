using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblRole
    {
        [Key]
        public System.Guid RoleID { get; set; }

        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public string AppCode { get; set; }
    }
}
