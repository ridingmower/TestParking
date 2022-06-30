using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblController
    {
        [Key]
        public System.Guid ControllerID { get; set; }

        public string ControllerCode { get; set; }

        public string ControllerName { get; set; }

        public Nullable<int> CommunicationType { get; set; }

        public string Comport { get; set; }

        public string Baudrate { get; set; }

        public Nullable<int> LineTypeID { get; set; }

        public Nullable<int> Reader1Type { get; set; }

        public Nullable<int> Reader2Type { get; set; }

        public string PCID { get; set; }

        public int Address { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }
    }
}
