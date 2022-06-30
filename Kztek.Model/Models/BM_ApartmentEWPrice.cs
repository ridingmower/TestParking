using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_ApartmentEWPrice
    {
        [Key]
        public string Id { get; set; }
        public decimal Electricity_Price { get; set; }
        public decimal Water_Price { get; set; }
        public DateTime Date_Price { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
