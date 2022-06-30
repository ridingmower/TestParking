using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class ExtendCard
    {
        [Key]
        public string Id { get; set; }

        public string Code { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string Plate { get; set; }

        public Nullable<System.DateTime> OldExpireDate { get; set; }

        public int Days { get; set; }

        public Nullable<System.DateTime> NewExpireDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string CardGroupID { get; set; }

        public string CustomerGroupID { get; set; }

        public string CustomerID { get; set; }

        public string UserID { get; set; }

        public int FeeLevel { get; set; }

        public bool IsDelete { get; set; }

        public bool IsTransferPayment { get; set; }
        public string SubId { get; set; }
    }

}
