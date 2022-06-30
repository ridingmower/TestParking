using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class tblLEDCustomViewModel
    {
        public string LEDID { get; set; }

        public string Name { get; set; }

        public string PCID { get; set; }

        public string PCName { get; set; }

        public string Comport { get; set; }

        public Nullable<int> Baudrate { get; set; }

        public bool EnableLED { get; set; }

        public int SortOrder { get; set; }

        public string LedType { get; set; }
    }
}
