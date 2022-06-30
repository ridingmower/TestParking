using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblPC
    {
        [Key]
        public System.Guid PCID { get; set; }

        public string ComputerCode { get; set; }

        public string ComputerName { get; set; }

        public string GateID { get; set; }

        public string IPAddress { get; set; }

        public string PicPathIn { get; set; }

        public string PicPathOut { get; set; }

        public string VideoPath { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }
    }
}
