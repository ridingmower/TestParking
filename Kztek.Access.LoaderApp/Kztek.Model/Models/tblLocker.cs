using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLocker
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int ReaderIndex { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string ControllerID { get; set; }

        public DateTime DateCreated { get; set; }

        public string LockerType { get; set; } // 0 - Chưa sử dụng, 1 - Cố định, 2 - Tức thời, 3 - Nhận dạng
    }

    public class LockerHome
    {
       public IQueryable<tblLockerController> ListController { get; set; }
       public IQueryable<tblLocker> ListLocker { get; set; }
    }
}
