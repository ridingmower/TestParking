using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessUploadDetail
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [StringLength(100)]
        public string CardNumber { get; set; }

        [StringLength(100)]
        public string UserIDofFinger { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Action { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Status { get; set; }

        [StringLength(100)]
        public string ControllerID { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        [StringLength(100)]
        public string UserID { get; set; }
    }
}
