using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblVehicleGroup
    {
        [Key]
        public System.Guid VehicleGroupID { get; set; }

        public string VehicleGroupCode { get; set; }

        public string VehicleGroupName { get; set; }

        public Nullable<int> VehicleType { get; set; }

        public Nullable<int> LimitNumber { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }
    }

    public class tblVehicleGroupAPI
    {
        public System.Guid VehicleGroupID { get; set; }

        public string VehicleGroupName { get; set; }
    }
    public class Result_tblVehicleGroupAPI
    {
        public string VehicleGroupID { get; set; }

        public string VehicleGroupName { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }

    }
}
