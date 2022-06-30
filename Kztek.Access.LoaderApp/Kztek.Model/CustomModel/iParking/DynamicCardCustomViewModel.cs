using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class DynamicCardCustomViewModel
    {
        public string Id { get; set; }
        public string ControllerID { get; set; }

        public string ControllerName { get; set; }

        public string PCID { get; set; }

        public string ComputerName { get; set; }

        public string CardGroupD { get; set; }

        public string CardGroupName { get; set; }
        public int Button { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
