using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Resident
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public string ResidentGroupId { get; set; }     
        public string Note { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BM_ResidentCustom
    {
        public Int64 RowNumber { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public string ResidentGroupId { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupName { get; set; }
        public string RoleId { get; set; }
    }
}
