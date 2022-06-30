using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Apartment_Member
    {
        [Key]
        public string Id { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
        public string RoleId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BM_Apartment_MemberCustom
    {
        
        public string Id { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
        public string RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}