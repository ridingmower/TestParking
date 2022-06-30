using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Model.CustomModel;
using PagedList;

namespace Kztek.Web.Core.Functions
{
    public class PageModelCustom<T> where T : class
    {
        public static PageModel<T> GetPage(IPagedList<T> list, int currentPage, int itemPerPage)
        {
            var PageModel = new PageModel<T>
            {
                Data = list,
                PageIndex = currentPage,
                PageSize = itemPerPage,
                TotalPage = list.PageCount,
                TotalItem = list.TotalItemCount,
            };
            return PageModel;
        }

        /// <summary>
        /// HNG: Hàm phân trang nhưng lấy theo tổng số bản ghi
        /// Dùng trong trường hợp truy vấn là ExcuteSQL
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemPerPage"></param>
        /// <param name="TotalItemCount"></param>
        /// <returns></returns>
        public static PageModel<T> GetPage(List<T> list, int currentPage, int itemPerPage, int TotalItemCount)
        {
            var PageCount = TotalItemCount > 0
                        ? (int)Math.Ceiling(TotalItemCount / (double)itemPerPage)
                        : 0;

            var PageModel = new PageModel<T>
            {
                Data = list,
                PageIndex = currentPage,
                PageSize = itemPerPage,
                TotalPage = PageCount,
                TotalItem = TotalItemCount,
            };
            return PageModel;
        }
    }
}
