using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class ChartModel
    {
        public string label { get; set; }

        public float data { get; set; }

        public string color { get; set; }

        public ChartModel()
        {
            
        }

        public ChartModel(string label, float data, string color)
        {
            this.label = label;
            this.data = data;
            this.color = color;
        }
    }
}
