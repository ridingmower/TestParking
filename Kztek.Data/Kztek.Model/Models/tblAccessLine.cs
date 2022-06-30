using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessLine
    {
        [Key]
        public Guid LineID { get; set; }

        [StringLength(50)]
        public string LineName { get; set; }

        [StringLength(50)]
        public string PCID { get; set; }

        public int? CommunicationType { get; set; }

        [StringLength(50)]
        public string Comport { get; set; }

        [StringLength(50)]
        public string Baudrate { get; set; }

        public int? LineTypeID { get; set; }

        [StringLength(50)]
        public string DownloadTime { get; set; }

        public bool? Inactive { get; set; }
    }
}
