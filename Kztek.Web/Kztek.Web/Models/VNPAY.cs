using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kztek.Web.Models
{
    public class VNPAY
    {
        public string vnp_Version { get; set; }
        public string vnp_Command { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_CreateDate { get; set; }
        public string vnp_CurrCode { get; set; }
        public string vnp_IpAddr { get; set; }
        public string vnp_Locale { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_OrderType { get; set; }
        public string vnp_ReturnUrl { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_ExpireDate { get; set; }
        public string vnp_Bill_Mobile { get; set; }
        public string vnp_Bill_FirstName { get; set; }
        public string vnp_Bill_LastName { get; set; }
        public string vnp_Bill_Email { get; set; }
        public string vnp_Bill_Address { get; set; }
        public string vnp_Bill_City { get; set; }
        public string vnp_Bill_Country { get; set; }
        public string vnp_Bill_State { get; set; }
        public string vnp_Inv_Phone { get; set; }
        public string vnp_Inv_Email { get; set; }
        public string vnp_Inv_Customer { get; set; }
        public string vnp_Inv_Address { get; set; }
        public string vnp_Inv_Company { get; set; }
        public string vnp_Inv_Taxcode { get; set; }
        public string vnp_Inv_Type { get; set; }
    }

    public class VNPAY_Custom
    {
        public string vnp_Version { get; set; }
        public string vnp_Command { get; set; }
        public string vnp_TmnCode { get; set; }
        public decimal vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public DateTime vnp_CreateDate { get; set; }
        public string vnp_CurrCode { get; set; }
        public string vnp_IpAddr { get; set; }
        public string vnp_Locale { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_OrderType { get; set; }
        public string vnp_ReturnUrl { get; set; }
        public string vnp_TxnRef { get; set; }
        public DateTime vnp_ExpireDate { get; set; }
        public string vnp_Bill_Mobile { get; set; }
        public string vnp_Bill_FirstName { get; set; }
        public string vnp_Bill_LastName { get; set; }
        public string vnp_Bill_Email { get; set; }
        public string vnp_Bill_Address { get; set; }
        public string vnp_Bill_City { get; set; }
        public string vnp_Bill_Country { get; set; }
        public string vnp_Bill_State { get; set; }
        public string vnp_Inv_Phone { get; set; }
        public string vnp_Inv_Email { get; set; }
        public string vnp_Inv_Customer { get; set; }
        public string vnp_Inv_Address { get; set; }
        public string vnp_Inv_Company { get; set; }
        public string vnp_Inv_Taxcode { get; set; }
        public string vnp_Inv_Type { get; set; }
    }
}