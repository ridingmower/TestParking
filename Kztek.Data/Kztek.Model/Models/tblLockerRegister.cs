using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLockerRegister
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string LockerID { get; set; }

        public string LockerIndex { get; set; }

        public string ControllerID { get; set; }

        public string Description { get; set; }

        public int Status { get; set; } = 0; // 1 - Đang sử dụng, 2 - Hoàn thành

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
    }
}
