using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessTimezone
    {
        [Key]
        public int Id { get; set; }

        public int? TimeZoneID { get; set; }

        [StringLength(50)]
        public string TimezoneName { get; set; }

        [StringLength(50)]
        public string Sun { get; set; }

        [StringLength(50)]
        public string Mon { get; set; }

        [StringLength(50)]
        public string Tue { get; set; }

        [StringLength(50)]
        public string Wed { get; set; }

        [StringLength(50)]
        public string Thu { get; set; }

        [StringLength(50)]
        public string Fri { get; set; }

        [StringLength(50)]
        public string Sat { get; set; }

        public bool? Inactive { get; set; }
    }

    public class tblAccessTimezoneSubmit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số cửa time zone")]
        public int TimeZoneID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string TimeZoneName { get; set; }

        public string SunFrom { get; set; } = "00:00";

        public string SunTo { get; set; } = "00:00";

        public string MonFrom { get; set; } = "00:00";

        public string MonTo { get; set; } = "00:00";

        public string TueFrom { get; set; } = "00:00";

        public string TueTo { get; set; } = "00:00";

        public string WedFrom { get; set; } = "00:00";

        public string WedTo { get; set; } = "00:00";

        public string ThuFrom { get; set; } = "00:00";

        public string ThuTo { get; set; } = "00:00";

        public string FriFrom { get; set; } = "00:00";

        public string FriTo { get; set; } = "00:00";

        public string SatFrom { get; set; } = "00:00";

        public string SatTo { get; set; } = "00:00";

        public bool Inactive { get; set; }
    }
}
