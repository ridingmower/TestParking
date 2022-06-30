using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class OrderActiveCard
    {
        [Key]
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
    }

    public class OrderActiveCard_Custom
    {
      
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public string Plate { get; set; }
        public string CardNumber { get; set; }
        public string CardNo { get; set; }
    }
}
