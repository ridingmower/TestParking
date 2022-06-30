using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblSubSystem
    {
        [Key]
        public System.Guid SubSystemID { get; set; }

        public string ParentID { get; set; }

        public string SubSystemCode { get; set; }

        public string SubSystemName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool Selects { get; set; }

        public bool Inserts { get; set; }

        public bool Updates { get; set; }

        public bool Deletes { get; set; }

        public bool Exports { get; set; }

        public bool Inactive { get; set; }

        public string AppCode { get; set; }
    }
}
