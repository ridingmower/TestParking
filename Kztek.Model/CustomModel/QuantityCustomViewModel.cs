using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class QuantityCustomViewModel
    {
        [StringLength(255)]
        public string Name { get; set; }

        public int? Quantity { get; set; }
    }

    public class QuantityChartCustomViewModel
    {
        [StringLength(255)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? Quantity { get; set; }
    }
}
