using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Apartment_Receipt
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserCreatedId { get; set; }
        public DateTime DatePaid { get; set; }
        public string ApartmentId { get; set; }
        public string ResidentId { get; set; }
        public string PayerName { get; set; }
        public string PayerMobile { get; set; }
        public string UserId { get; set; }
        public decimal Money { get; set; }
        public int PayType { get; set; }
        public int Type { get; set; }
        public int Note { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class BM_Apartment_ReceiptView
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserCreatedId { get; set; }
        public DateTime DatePaid { get; set; }
        public string ApartmentId { get; set; }
        public string ResidentId { get; set; }
        public string PayerName { get; set; }
        public string PayerMobile { get; set; }
        public string UserId { get; set; }
        public decimal Money { get; set; }
        public int PayType { get; set; }
        public int Type { get; set; }
        public int Note { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string ApartmentName { get; set; }
        public string BuildingName { get; set; }
        public string BuildingId { get; set; }
        public string FloorId { get; set; }
        public string FloorName { get; set; }
    }
}