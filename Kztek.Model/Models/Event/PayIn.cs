using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class PayIn
    {
        [Key]
        public int ID { get; set; }
        public string EventID { get; set; }
        public decimal Moneys { get; set; }
        public int PayState { get; set; }
    }
}
