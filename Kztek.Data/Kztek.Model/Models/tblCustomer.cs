using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblCustomer
    {
        [Key]
        public System.Guid CustomerID { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string IDNumber { get; set; }

        public string Mobile { get; set; }

        public string CustomerGroupID { get; set; }

        public string Description { get; set; }

        public bool EnableAccount { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public string CompartmentId { get; set; }

        public string AccessLevelID { get; set; }

        public string Finger1 { get; set; }

        public string Finger2 { get; set; }

        public int UserIDofFinger { get; set; }

        public System.DateTime AccessExpireDate { get; set; }

        public string DevPass { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractEndDate { get; set; }

        public string AddressUnsign { get; set; }
    }

    public class tblCustomerSubmit
    {
        public string CustomerID { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string Address { get; set; } = "";

        public string IDNumber { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string CustomerGroupID { get; set; } = "";

        public string Description { get; set; } = "";

        public bool EnableAccount { get; set; }

        public string Account { get; set; } = "";

        public string Password { get; set; } = "";

        public string Avatar { get; set; } = "";

        public bool Inactive { get; set; }

        public int SortOrder { get; set; }

        public string CompartmentId { get; set; } = "";

        public string AccessLevelID { get; set; } = "";

        public string Finger1 { get; set; } = "";

        public string Finger2 { get; set; } = "";

        public int UserIDofFinger { get; set; }

        //public System.DateTime AccessExpireDate { get; set; }

        public string DevPass { get; set; } = "";

        public System.DateTime AccessExpireDate { get; set; }
    }

    public class tblCustomerExtend
    {
        public string CustomerID { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string Address { get; set; } = "";

        public string IDNumber { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string CustomerGroupID { get; set; } = "";

        public string CustomerGroupName { get; set; } = "";

        public int SortOrder { get; set; }

        public string AccessLevelID { get; set; } = "";

        public string AccessLevelName { get; set; } = "";

        public string Finger1 { get; set; } = "";

        public string Finger2 { get; set; } = "";

        public int UserIDofFinger { get; set; }

        public string Password { get; set; } = "";

        public System.DateTime AccessExpireDate { get; set; }

    }

    public class tblCustomer_API3rd
    {
        public string CustomerID { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string Address { get; set; } = "";

        public string IDNumber { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string CustomerGroupID { get; set; } = "";

        public string CustomerGroupName { get; set; } = "";
    }

    public class Result_tblCustomer_API3rd
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }

    }
    public class tblCustomer_GetByNameOrMobile
    {
        public string CustomerID { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string Mobile { get; set; } = "";
    }

    public class tblCustomer_API
    {
        public string CustomerID { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";
        public string CustomerGroupID { get; set; } = "";

    }
}
