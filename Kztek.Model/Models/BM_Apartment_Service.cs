using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Apartment_Service
    {
        [Key]
        public string Id { get; set; }
        public string ApartmentId { get; set; }
        public string ServiceId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SchedulePay { get; set; }
        public int ScheduleType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
