using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iLocker
{
    public class UploadModel
    {
        public string TaskViewId { get; set; } //Mỗi lần bật view sẽ là task id riêng -> chia session.

        public List<tblLockerLine> tblLockerLines { get; set; } //Danh sách lines

        public List<tblLockerController> tblLockerControllers { get; set; } //Danh sách bộ điều khiển

        public DataTable CardGroupDT { get; set; }

        public DataTable CustomerGroupDT { get; set; }
    }

    public class LockerControllerModel
    {
        public List<tblLockerController> Data { get; set; }

        public List<Kztek.Model.Models.tblLockerController> Selected { get; set; }
    }

    public class LockerConfirm
    {
        public List<Kztek.Model.Models.tblLockerController> DataController { get; set; }

        public int CardCount { get; set; }

        public int LockerCount { get; set; }

        public string DataLocker { get; set; }

        public string Description { get; set; }

        public string ActionTake { get; set; }
    }

    public class LockerCard
    {
        public string CardNumber { get; set; }

        public string CardNo { get; set; }

        public string CardGroupName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }

        public List<tblLocker> Data { get; set; }

        public string TaskViewId { get; set; }
    }

    public class LockerOpenManual
    {
        public string TaskViewId { get; set; }

        public List<tblLockerController> DataController { get; set; }

    }

    public class LockerOpenModel
    {
        public List<tblLocker> Data { get; set; }

        public List<Kztek.Model.Models.tblLocker> Selected { get; set; }
    }
}
