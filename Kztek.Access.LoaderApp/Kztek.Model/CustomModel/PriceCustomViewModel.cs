using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class PriceCustomViewModel
    {
        [StringLength(255)]
        public string Name { get; set; }

        public string Price { get; set; }
    }

    public class PriceChartCustomViewModel
    {
        [StringLength(255)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Price { get; set; }
    }
}
