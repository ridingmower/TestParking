using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblRolePermissionMaping
    {
        [Key]
        public System.Guid ID { get; set; }

        public string RoleID { get; set; }

        public string SubSystemID { get; set; }

        public bool Selects { get; set; }

        public bool Inserts { get; set; }

        public bool Updates { get; set; }

        public bool Deletes { get; set; }

        public bool Exports { get; set; }
    }
}
