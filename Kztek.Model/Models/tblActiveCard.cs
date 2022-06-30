using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblActiveCard
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string Plate { get; set; }

        public Nullable<System.DateTime> OldExpireDate { get; set; }

        public int Days { get; set; }

        public Nullable<System.DateTime> NewExpireDate { get; set; }

        public string CardGroupID { get; set; }

        public string CustomerGroupID { get; set; }

        public string CustomerID { get; set; }

        public string UserID { get; set; }

        public int FeeLevel { get; set; }

        public bool IsDelete { get; set; }

        public string ContractCode { get; set; }

        public bool IsTransferPayment { get; set; }
        public string OrderId { get; set; }
        public string SubId { get; set; }
    }

    public class tblActiveCardCustomViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string Plate { get; set; }
        public string Address { get; set; }
        public string AddressUnsign { get; set; }
        public Nullable<System.DateTime> OldExpireDate { get; set; }

        public int Days { get; set; }

        public Nullable<System.DateTime> NewExpireDate { get; set; }

        public string CardGroupName { get; set; }

        public string CardGroupID { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerGroupID { get; set; }

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }

        public string UserID { get; set; }

        public int FeeLevel { get; set; }

        public bool IsDelete { get; set; }

        public string ContractCode { get; set; }

        public bool IsTransferPayment { get; set; }
        public string Tax { get; set; }
        public string SubId { get; set; }
        public string ExtendId { get; set; }
    }

    public class tblActiveCard_Excel
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string Plate { get; set; }

        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string CustomerGroupName { get; set; }

        public string OldDate { get; set; }

        public string NewDate { get; set; }

        public int Days { get; set; }

        public int Money { get; set; }

        public string DateCreated { get; set; }

        public string UserName { get; set; }

       

    }

    public class tblActiveCard_ExcelTRANSERCO
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

       

        public string CustomerName { get; set; }
        public string Plate { get; set; }
        public string ContractCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string Tax { get; set; }
        public string OldDate { get; set; }

        public string NewDate { get; set; }

        public int Days { get; set; }

        public int Money { get; set; }

        public string DateCreated { get; set; }

        public string UserName { get; set; }

        public bool IsTransferPayment { get; set; }
        public string TransferPaymentValue { get; set; }

    }

    public class tblActiveCard_ExcelPRIDE
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string Plate { get; set; }
       
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string Address { get; set; }
        public string OldDate { get; set; }

        public string NewDate { get; set; }

        public string Days { get; set; }

        public double Money { get; set; }
        public string DateCreated { get; set; }

        public string UserName { get; set; }



    }

    public class tblActiveCard_TC
    {
        public string CardNumber { get; set; }
        public int FeeLevel { get; set; }
    }
}
