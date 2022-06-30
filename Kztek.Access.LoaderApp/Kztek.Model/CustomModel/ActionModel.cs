using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class ActionModel
    {
        public string CurrentControllerName { get; set; }

        public string CurrentActionName { get; set; }

        public string CurrentObjId { get; set; }

        public Nullable<int> CurrentPageIndex { get; set; }
    }
}
