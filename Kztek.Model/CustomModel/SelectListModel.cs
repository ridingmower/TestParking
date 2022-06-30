using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class SelectListModel
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
    }

    public class SelectListModel2
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        public string ItemOtherValue { get; set; }

        public int ItemSecondValue { get; set; }
    }

    public class SelectListModel3
    {
        public string ItemValue1 { get; set; }
        public string ItemValue2 { get; set; }
    }

    public class SelectListModel4
    {
        public string ControllerID { get; set; }

        public string DoorIndexes { get; set; }

        public string TimezoneID { get; set; }
    }

    public class SelectListModel5
    {
        public string ItemValue { get; set; }

        public string ItemText { get; set; }

        public string ItemSecondValue { get; set; }
    }

    public class SelectListModel1
    {
        public string ItemValue { get; set; }

        public string ItemText { get; set; }

        public int ItemIndex { get; set; }

        public string ItemCode { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        public string AreaName { get; set; }
    }

    public class SelectListModel_Resolution
    {
        public string Text { get; set; }
    }

    public class SelectListModel6
    {
        public int ItemValue { get; set; }

        public string ItemText { get; set; }
        public string Type { get; set; }
    }


    public class SelectListModelUpload
    {
        public DataTable dtComputer { get; set; }
        public DataTable dtLine { get; set; }
        public DataTable dtCardGroup { get; set; }

        public DataTable dtCustomerGroup { get; set; }

        public DataTable dtAccessLevel { get; set; }
    }

    public class SelectListModelCardUpload
    {
        public string key { get; set; } = "";

        public string cardgroupids { get; set; } = "";

        public string customergroupid { get; set; } = "";

        public string accesslevelids { get; set; } = "";

        public bool isall { get; set; }

        public string newdateexpire { get; set; } = "";

        public bool isusenewdate { get; set; } = false;

        public int pageSize { get; set; } = 10;

        public int pageIndex { get; set; } = 1;

      public  string action { get; set; }

      public  string type { get; set; }
    }

    public class SelectListModelCardUploadReturn
    {
        public List<tblCardExtend> ListCard { get; set; }

        public List<tblCustomerExtend> ListCustomer { get; set; }

        public List<Employee> ListEmployee { get; set; }

        public List<tblAccessController> ListController { get; set; }

        public List<SelfHostConfig> ListSelfHost { get; set; }

        public bool IsUseNewDate { get; set; } = false;
    }

    public class SelectListModelCardUploadReturn2
    {
        public List<Employee> ListEmployee { get; set; }

        public List<SelfHostConfig> ListSelfHost { get; set; }
    }

    public class SelectListModelCardUploadReturn3
    {
        public int totalPage { get; set; }

        public int pageSize { get; set; }

        public int totalItem { get; set; }
    }

    public class SelectListModelUploadSubmit
    {
        public Employee objE { get; set; } = new Employee();

        public string actionV { get; set; } = "";

        public string controllerid { get; set; } = "";

        public string eventtype { get; set; } = "";

        public string desc { get; set; } = "";

        public bool isusenewdate { get; set; } = false;
    }

    public class SelectListModelCardUploadStatus
    {
        public List<tblAccessController> ListController { get; set; }

        public List<tblAccessUploadDetail> ListUploadDetail { get; set; }

        public string Status { get; set; }
    }

    public class SelectListModel_FileUpload
    {
        public string FileUploadName { get; set; }

        public string BoxRenderId { get; set; }

        public string Base64String { get; set; }

        public string FilePath { get; set; }

        public string CustomerId { get; set; }
    }

    public class SelectListModelAutocomplete
    {
        //Id
        public string id { get; set; }

        //Tên tìm thêm
        public string name { get; set; }

        //Tên hiển thị
        public string value { get; set; }
    }
}
