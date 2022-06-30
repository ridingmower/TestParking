using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class tblLaneCustomViewModel
    {
        public string LaneID { get; set; }

        public string LaneName { get; set; }

        public string PCID { get; set; }

        public string PCName { get; set; }

        public int LaneType { get; set; }

        public bool Inactive { get; set; }

        public int SortOrder { get; set; }
    }
}
