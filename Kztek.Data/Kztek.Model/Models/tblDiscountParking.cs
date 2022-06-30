using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
  public  class tblDiscountParking
    {
        [Key]
        public string Id { get; set; }
        public string DCTypeName { get; set; }
        public string DCTypeCode { get; set; }
        public int DiscountMode { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Note { get; set; }
        public Int16 Priority { get; set; }
    }
    public class tblDiscountParking_Submit
    {
       
        public string Id { get; set; }
        public string NameDiscountType { get; set; }
        public string CodeDiscountType { get; set; }
        public int DiscountMode { get; set; }
        public string AmountReduced { get; set; }
        public string Note { get; set; }
        public string Priority { get; set; }
    }
}
