using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class API_CardInfo
    {
        //tblCard
        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }

        public bool IsLock { get; set; }

        public string CardDescription { get; set; }

        public DateTime? DateActive { get; set; }

        //tblCardGroup
        public string CardGroupID { get; set; }

        public string CardGroupName { get; set; }

        public int CardType { get; set; }

        public bool IsHaveMoneyExpiredDate { get; set; }

        public bool CardGroupInactive { get; set; }

        //tblVehicleGroup
        public string VehicleGroupID { get; set; }

        public string VehicleGroupName { get; set; }

        public int? VehicleType { get; set; }

        //tblCustomer
        public string CustomerID { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string IDNumber { get; set; }

        public string Mobile { get; set; }

        public string CustomerDescription { get; set; }

        public string Avatar { get; set; }

        //tblCustomerGroup
        public string CustomerGroupID { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerGroupDescription { get; set; }
    }
}
