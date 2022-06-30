using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblAccessUploadProcess
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(100)]
        public string CardNumber { get; set; }

        [StringLength(100)]
        public string UserIDofFinger { get; set; }

        [StringLength(100)]
        public string Actions { get; set; }

        [StringLength(50)]
        public string CardGroupID { get; set; }

        [StringLength(100)]
        public string UserID { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string AccessLevelID { get; set; }

        [StringLength(50)]
        public string CustomerID { get; set; }

        [StringLength(50)]
        public string CustomerGroupID { get; set; }

        [StringLength(250)]
        public string SuccessControllerIDs { get; set; }

        [StringLength(250)]
        public string TotalControllerIDs { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        public DateTime? AccessDateExpire { get; set; }
    }

    public class ReporttblAccessUploadProcess
    {
        
        public int Id { get; set; }

        public DateTime? Date { get; set; }
     
        public string CardNumber { get; set; }
    
        public string UserIDofFinger { get; set; }

        public string Actions { get; set; }

        public string CardGroupID { get; set; }

        public string UserID { get; set; }

        public string Description { get; set; }

        public string AccessLevelID { get; set; }

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }

        public string CustomerGroupID { get; set; }

        public string SuccessControllerIDs { get; set; }

        public string TotalControllerIDs { get; set; }

        public string EventType { get; set; }

        public DateTime? AccessDateExpire { get; set; }
    }
}
