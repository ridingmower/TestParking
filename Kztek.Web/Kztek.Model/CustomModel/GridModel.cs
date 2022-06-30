using System.Collections.Generic;

namespace Kztek.Model.CustomModel
{
    public class GridModel<T>
    {
        public object Aggregates { get; set; }

        public IEnumerable<T> Data { get; set; }

        public int TotalPage { get; set; }

        public int TotalItem { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
