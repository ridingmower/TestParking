using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class tblControllerCustomViewModel
    {
        public string ControllerID { get; set; }

        public string ControllerName { get; set; }

        public string Comport { get; set; }

        public string PCID { get; set; }

        public string PCName { get; set; }

        public bool Inactive { get; set; }

        public int SortOrder { get; set; }
    }
}
