using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblCustomerGroup
    {
        [Key]
        public System.Guid CustomerGroupID { get; set; }

        public string ParentID { get; set; }

        public string CustomerGroupCode { get; set; }

        public string CustomerGroupName { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }
        public int Ordering { get; set; }
        public string Tax { get; set; }
        public bool IsCompany { get; set; }
    }

    public class tblCustomerGroupSubmit
    {
        public string CustomerGroupID { get; set; }

        public string ParentID { get; set; }

        public string CustomerGroupCode { get; set; }

        public string CustomerGroupName { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        public int SortOrder { get; set; }
        public int Ordering { get; set; }
    }
    public class tblCustomerGroupExcel
    {
        public string STT { get; set; }
        public string CustomerGroupName { get; set; }

    }
    public class tblCustomerGroupAPI
    {
        public string CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public string ParentID { get; set; }
        public int Ordering { get; set; }

    }
    public class Result_tblCustomerGroup
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }

    }
}
