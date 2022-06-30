using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class MenuFunction
    {
        public string Id { get; set; }

        public string MenuName { get; set; }

        [StringLength(150)]
        public string ControllerName { get; set; }

        [StringLength(10)]
        public string MenuType { get; set; }

        [StringLength(150)]
        public string ActionName { get; set; }

        [StringLength(1000)]
        public string Url { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [StringLength(100)]
        public string ParentId { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public Nullable<int> OrderNumber { get; set; }

        public string Breadcrumb { get; set; }

        public Nullable<int> Dept { get; set; }

        public bool isSystem { get; set; } = false;

        public string MenuGroupListId { get; set; } = "";
    }

    public class MenuFunctionSubmit
    {
        public string Id { get; set; }

        public string MenuName { get; set; }

        [StringLength(150)]
        public string ControllerName { get; set; }

        [StringLength(10)]
        public string MenuType { get; set; }

        [StringLength(150)]
        public string ActionName { get; set; }

        [StringLength(1000)]
        public string Url { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [StringLength(100)]
        public string ParentId { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public Nullable<int> OrderNumber { get; set; }

        public string Breadcrumb { get; set; }

        public Nullable<int> Dept { get; set; }

        public bool isSystem { get; set; } = false;

        public string MenuGroupListId { get; set; } = "";
    }
}
