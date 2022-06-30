using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessLevelDetail
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string AccessLevelID { get; set; }

        [StringLength(50)]
        public string ControllerID { get; set; }

        [StringLength(500)]
        public string DoorIndexes { get; set; }

        [StringLength(50)]
        public string TimezoneID { get; set; }
    }

    public class tblAccessLevelDetailCustom
    {
        public int Id { get; set; }

        public string AccessLevelID { get; set; }

        public string ControllerID { get; set; }

        public string DoorIndexes { get; set; }

        public string TimezoneID { get; set; }
    }
}
