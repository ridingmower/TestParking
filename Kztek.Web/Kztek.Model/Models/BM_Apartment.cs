using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Apartment
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public decimal Acreage { get; set; }
        public string ApartmentUseId { get; set; }
        public string BuildingId { get; set; }
        public string FloorId { get; set; }
        public int ElecticityType { get; set; }// Loại điện tiêu thụ	"1 - Điện gia đình. 2 - Điện kinh doanh"
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BM_ApartmentCustom
    {
        public Int64 RowNumber { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public decimal Acreage { get; set; }
        public string ApartmentUseId { get; set; }
        public string BuildingId { get; set; }
        public string FloorId { get; set; }
        public int ElecticityType { get; set; }// Loại điện tiêu thụ	"1 - Điện gia đình. 2 - Điện kinh doanh"
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string BuildingName { get; set; }
        public string FloorName { get; set; }
    }
}
