using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kztek.Model.CustomModel
{
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}