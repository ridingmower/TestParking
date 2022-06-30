using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLED
    {
        [Key]
        public int LEDID { get; set; }

        public string LEDName { get; set; }

        public string PCID { get; set; }

        public string Comport { get; set; }

        public Nullable<int> Baudrate { get; set; }

        public int SideIndex { get; set; } = 0;
        public int RowNumber { get; set; }

        public int Address { get; set; } = 0;

        public string LedType { get; set; }

        public bool EnableLED { get; set; }
    }

    public class tblLEDView
    {
        [Key]
        public int LEDID { get; set; }

        public string LEDName { get; set; }

        public string PCID { get; set; }

        public string Comport { get; set; }

        public Nullable<int> Baudrate { get; set; }

        public int? SideIndex { get; set; } = 0;
        public int RowNumber { get; set; }

        public int? Address { get; set; } = 0;

        public string LedType { get; set; }

        public bool EnableLED { get; set; }
    }
}
