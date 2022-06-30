using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Building_Service
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Day { get; set; }
        public string SchedulePay { get; set; }
        public int ScheduleType { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BM_Building_ServiceCustom
    {
        public Int64 RowNumber { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Day { get; set; }
        public string SchedulePay { get; set; }
        public int ScheduleType { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string PriceValue { get; set; }
    }
}
