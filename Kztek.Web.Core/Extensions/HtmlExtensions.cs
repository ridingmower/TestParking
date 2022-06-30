using System;
using System.Text;
using System.Linq;
using System.Web.Mvc;
namespace Kztek.Web.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString GeneratePagingFooterForAjax(this HtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<div class='t-grid-pager {0}'>", cssClass);
            //sb.Append("<div class='t-status'><a class='t-icon t-refresh' href='javascript:void(0);' rel='nofollow'>Refresh</a></div>");
            sb.Append("<div class='t-pager t-reset'>");

            if (currentPage == 1)
            {
                sb.Append("<a class='t-link t-state-disabled w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-first'>first</span></a>");
                sb.Append("<a class='t-link t-state-disabled w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-prev'>prev</span></a>");
            }
            else
            {
                sb.Append("<a class='t-link w-16'><span class='t-icon t-arrow-first'>first</span></a>");
                sb.Append("<a class='t-link w-16'><span class='t-icon t-arrow-prev'>prev</span></a>");
            }

            var totalHold = totalPage/itemsPerPageingFooter + 1;
            var currentHold = currentPage / itemsPerPageingFooter >= 1 && currentPage % itemsPerPageingFooter >= 1 ?
                currentPage / itemsPerPageingFooter + 1 : currentPage / itemsPerPageingFooter;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / itemsPerPageingFooter >= 1 && currentPage % itemsPerPageingFooter >= 1)
                pointStart = currentPage / itemsPerPageingFooter * itemsPerPageingFooter + 1;
            else if (currentPage / itemsPerPageingFooter > 0)
                pointStart = (currentPage / itemsPerPageingFooter - 1) * itemsPerPageingFooter + 1;

            if (currentHold == 1)
            {
                sb.Append("<div class='t-numeric'>");
                for (var i = pointStart; i <= ((totalPage < itemsPerPageingFooter) ? totalPage : pointStart + itemsPerPageingFooter - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link'>{0}</a>", i);
                    }
                }
                if (totalPage > itemsPerPageingFooter)
                    sb.Append("<a class='t-link t-next'>...</a>");
                sb.Append("</div>");
            }
            else if(currentHold == totalHold)
            {
                sb.Append("<div class='t-numeric'>");
                sb.Append("<a class='t-link t-prev'>...</a>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link'>{0}</a>", i);
                    }
                }
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div class='t-numeric'>");
                sb.Append("<a class='t-link t-prev'>...</a>");
                for (var i = pointStart; i <= pointStart + itemsPerPageingFooter - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link'>{0}</a>", i);
                    }
                }
                sb.Append("<a class='t-link t-next'>...</a>");
                sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<a class='t-link t-state-disabled  w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-next'>next</span></a>");
                sb.Append("<a class='t-link t-state-disabled  w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-last'>last</span></a>");
                sb.Append("<div class='clear'></div>");
            }
            else
            {
                sb.Append("<a class='t-link w-16'><span class='t-icon t-arrow-next'>next</span></a>");
                sb.Append("<a class='t-link w-16'><span class='t-icon t-arrow-last'>last</span></a>");
            }

            sb.Append("</div>");
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }

      
        //public static MvcHtmlString GeneratePagingFooter(this HtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendFormat("<div class='t-grid-pager {0}'>", cssClass);
        //    sb.Append("<div class='t-status'><a class='t-icon t-refresh' href='javascript:void(0);' rel='nofollow'>Refresh</a></div>");
        //    sb.Append("<div class='t-pager t-reset'>");           
        //    if (currentPage == 1)
        //    {
        //        sb.Append("<a class='t-link t-state-disabled w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-first'>first</span></a>");
        //        sb.Append("<a class='t-link t-state-disabled w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-prev'>prev</span></a>");
        //    }
        //    else
        //    {
        //        sb.Append("<a class='t-link w-16' href='" + pageUrl(1) + "'><span class='t-icon t-arrow-first'>first</span></a>");
        //        sb.Append("<a class='t-link w-16' href='" + pageUrl(currentPage - 1) + "'><span class='t-icon t-arrow-prev'>prev</span></a>");
        //    }

        //    const int pageHold = 10;
        //    var totalHold = totalPage / pageHold + 1;
        //    var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
        //        currentPage / pageHold + 1 : currentPage / pageHold;
        //    currentHold = currentHold == 0 ? 1 : currentHold;

        //    var pointStart = 1;

        //    if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
        //        pointStart = currentPage / pageHold * pageHold + 1;
        //    else if (currentPage / pageHold > 0)
        //        pointStart = (currentPage / pageHold - 1) * pageHold + 1;

        //    if (currentHold == 1)
        //    {
        //        sb.Append("<div class='t-numeric'>");
        //        for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
        //        {
        //            if (i == currentPage)
        //            {
        //                sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
        //            }
        //            else
        //            {
        //                sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a>", i);
        //            }
        //        }
        //        if (totalHold > 1)
        //            sb.Append("<a class='t-link t-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a>");
        //        sb.Append("</div>");
        //    }
        //    else if (currentHold == totalHold)
        //    {
        //        sb.Append("<div class='t-numeric'>");
        //        sb.Append("<a class='t-link t-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a>");
        //        for (var i = pointStart; i <= totalPage; i++)
        //        {
        //            if (i == currentPage)
        //            {
        //                sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
        //            }
        //            else
        //            {
        //                sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a>", i);
        //            }
        //        }
        //        sb.Append("</div>");
        //    }
        //    else
        //    {
        //        sb.Append("<div class='t-numeric'>");
        //        sb.Append("<a class='t-link t-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a>");
        //        for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
        //        {
        //            if (i == currentPage)
        //            {
        //                sb.AppendFormat("<span class='t-state-active'>{0}</span>", i);
        //            }
        //            else
        //            {
        //                sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a>", i);
        //            }
        //        }
        //        sb.Append("<a class='t-link t-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a>");
        //        sb.Append("</div>");
        //    }

        //    if (currentPage == totalPage)
        //    {
        //        sb.Append("<a class='t-link t-state-disabled  w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-next'>next</span></a>");
        //        sb.Append("<a class='t-link t-state-disabled  w-16' href='javascript:void(0);' rel='nofollow'><span class='t-icon t-arrow-last'>last</span></a>");
        //        sb.Append("<div class='clear'></div>");
        //    }
        //    else
        //    {
        //        sb.Append("<a class='t-link w-16' href='" + pageUrl(currentPage + 1) + "'><span class='t-icon t-arrow-next'>next</span></a>");
        //        sb.Append("<a class='t-link w-16' href='" + pageUrl(totalPage) + "'><span class='t-icon t-arrow-last'>last</span></a>");
        //    }

        //    sb.Append("</div>");
        //    sb.Append("<div class='t-pager-size'><div class='t-pager-size-chosen'>10</div><ul>" +
        //              "<li>10</li>" +
        //              "<li>15</li>" +
        //              "<li>20</li>" +
        //              "<li>50</li>" +
        //              "<li>100</li>" +
        //              "</ul>" +
        //              "<div class='sprite t-icon-arrow-bottom'></div></div>");
        //    sb.Append("</div>");
        //    return new MvcHtmlString(sb.ToString());
        //}
        public static MvcHtmlString GeneratePagingFooter(this HtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class='pagination' style='float:right'>");
            if (currentPage == 1)
            {
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='javascript:void(0);' rel='nofollow'>First</a></li>");
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='javascript:void(0);' rel='nofollow'>Previous</a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='" + pageUrl(1) + "'>First</a></li>");
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='" + pageUrl(currentPage - 1) + "'>Previous</a></li>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                //sb.Append("<div class='t-numeric'>");
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                if (totalHold > 1)
                    sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='javascript:void(0);' rel='nofollow'>Next</a></li>");
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='javascript:void(0);' rel='nofollow'>Last</a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='" + pageUrl(currentPage + 1) + "'>Next</a></li>");
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='" + pageUrl(totalPage) + "'>Last</a></li");
            }

            sb.Append("</ul>");
            //sb.Append("</div>");
            //sb.Append("<div class='t-pager-size'><div class='t-pager-size-chosen'>10</div><ul>" +
            //          "<li>10</li>" +
            //          "<li>15</li>" +
            //          "<li>20</li>" +
            //          "<li>50</li>" +
            //          "<li>100</li>" +
            //          "</ul>" +
            //          "<div class='sprite t-icon-arrow-bottom'></div></div>");
            //sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }
       
        public static MvcHtmlString GeneratePagingHeader(this HtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<div class='t-grid-pager {0}'>", cssClass);
            if (currentPage == 1)
            {
                sb.Append("<a class='btn-pre' href='javascript:void(0);'>Trang đầu</a>");
            }
            else
            {
                sb.Append("<a class='btn-pre' href='" + pageUrl(1) + "'>Trang đầu</a>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<a class='t-link active'>{0}</a>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a> ", i);
                    }
                }
                if (totalHold > 1)
                    sb.Append("<a class='t-link t-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<a class='t-link t-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<a class='t-linkactive'>{0}</a>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<a class='t-link t-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<a class='t-link active'>{0}</a>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a class='t-link' href='" + pageUrl(i) + "'>{0}</a>", i);
                    }
                }
                sb.Append("<a class='t-link t-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<a class='btn-next' href='javascript:void(0);'>Trang cuối</a>");
            }
            else
            {
                sb.Append("<a class='btn-next' href='" + pageUrl(totalPage) + "'>Trang cuối</a>");
            }

            sb.Append("</div>");
       
            return new MvcHtmlString(sb.ToString());
        }
        public static MvcHtmlString GeneratePagingHome(this HtmlHelper htmlHelper, int totalPage, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            //<ul class="ea-nls-pager clearfix">
            //            <li>Trang</li>
            //            <li><a href="" title="">1</a></li>
            //            <li><a href="" title="">2</a></li>
            //            <li><a href="" title="">3</a></li>
            //            <li>...</li>
            //            <li><a href="" title="">20</a></li>
            //        </ul>
            var sb = new StringBuilder();
            sb.AppendFormat("<ul class='ea-nls-pager clearfix {0}'><li>Trang</li>", cssClass);
            if (currentPage == 1)
            {
                sb.Append("<li><a href='javascript:void(0);'>Đầu</a></li>");
            }
            else
            {
                sb.Append("<li><a href='" + pageUrl(1) + "'>Đầu</a></li>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li><a class='active' href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                if (totalHold > 1)
                    sb.Append("<li><a class='p-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li><a class='p-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li><a class='active'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li><a class='p-prev' href='" + pageUrl(pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li><a class='active'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a href='" + pageUrl(i) + "'>{0}</a></li>", i);
                    }
                }
                sb.Append("<li><a class='p-next' href='" + pageUrl(pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<li><a href='javascript:void(0);'>Cuối</a></li>");
            }
            else
            {
                sb.Append("<li><a href='" + pageUrl(totalPage) + "'>Cuối</a></li>");
            }

            sb.Append("</ul>");

            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// Xac dinh dia chi Url duy nhat tren website
        /// </summary>
        /// <param name="html"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MvcHtmlString CanonicalUrl(this HtmlHelper html, string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                var rawUrl = html.ViewContext.RequestContext.HttpContext.Request.Url;
                path = String.Format("{0}://{1}{2}", rawUrl.Scheme, rawUrl.Host, rawUrl.AbsolutePath);
            }

            path = path.ToLower();

            if (path.Count(c => c == '/') > 3)
            {
                path = path.TrimEnd('/');
            }

            if (path.EndsWith("/index"))
            {
                path = path.Substring(0, path.Length - 6);
            }

            var canonical = new TagBuilder("link");
            canonical.MergeAttribute("rel", "canonical");
            canonical.MergeAttribute("href", path);
            return new MvcHtmlString(canonical.ToString(TagRenderMode.SelfClosing));
        }
        public static MvcHtmlString PageLinksOnProductListCatePage(this HtmlHelper html, Kztek.Model.CustomModel.PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var arr = GenerateArrIntForPaging(pageInfo.CurrentPage, pageInfo.TotalPages, 5);
            if (pageInfo.TotalPages > 0)
            {
                var result = new StringBuilder();
                //Tạo link về đầu
                var FirstTag = new TagBuilder("a");
                FirstTag.MergeAttribute("href", pageUrl(1));
                FirstTag.MergeAttribute("class", "btn-pre");
                result.AppendLine(""+FirstTag+"");

                for (int i = 0; i < arr.Length; i++)
                {
                    var tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(arr[i]));
                    tag.InnerHtml = arr[i].ToString();
                    tag.AddCssClass("category_pager_item");
                    if (arr[i] == pageInfo.CurrentPage)
                        tag.AddCssClass("active");
                    result.AppendLine("" + tag + "");
                }


                //Tạo link về cuối
                TagBuilder LastTag = new TagBuilder("a");
                LastTag.MergeAttribute("href", pageUrl(pageInfo.TotalPages));
                LastTag.MergeAttribute("class", "last_paging");
                LastTag.AddCssClass("btn-next");
                result.AppendLine("" + LastTag + "");

                return MvcHtmlString.Create("" + result + "");
            }
            return MvcHtmlString.Create("");
        }
        public static MvcHtmlString CanonicalUrl(this HtmlHelper html)
        {
            var rawUrl = html.ViewContext.RequestContext.HttpContext.Request.Url;

            return CanonicalUrl(html, String.Format("{0}://{1}{2}", rawUrl.Scheme, rawUrl.Host, rawUrl.AbsolutePath));
        }
        public static int[] GenerateArrIntForPaging(int currentPage, int totalPages, int itemPerPage)
        {
            var arr = new int[itemPerPage];
            if ((currentPage <= 5) & (totalPages <= 5))
            {
                arr = new int[totalPages];
                for (var i = 0; i < totalPages; i++)
                {
                    arr[i] = i + 1;
                }
                return arr;
            }

            if ((currentPage <= 3) & (totalPages > 5))
            {
                for (var i = 0; i < itemPerPage; i++)
                {
                    arr[i] = i + 1;
                }
                return arr;
            }

            if ((currentPage >= 4) & (totalPages > 5))
            {
                arr[0] = currentPage - 2;
                arr[1] = currentPage - 1;
                arr[2] = currentPage;
                arr[3] = currentPage + 1;
                arr[4] = currentPage + 2;
            }

            // 2 so cuoi
            if (currentPage == (totalPages - 1))
            {
                arr[0] = currentPage - 3;
                arr[1] = currentPage - 2;
                arr[2] = currentPage - 1;
                arr[3] = currentPage;
                arr[4] = currentPage + 1;
            }

            if (currentPage == totalPages)
            {
                arr[0] = currentPage - 4;
                arr[1] = currentPage - 3;
                arr[2] = currentPage - 2;
                arr[3] = currentPage - 1;
                arr[4] = currentPage;
            }

            if (currentPage > totalPages)
            {
                arr[0] = totalPages - 4;
                arr[1] = totalPages - 3;
                arr[2] = totalPages - 2;
                arr[3] = totalPages - 1;
                arr[4] = totalPages;
            }

            return arr;
        }

        public static MvcHtmlString GeneratePagingFooterAjax(this HtmlHelper htmlHelper, int totalPage, int totalItem, int currentPage, int itemsPerPageingFooter, string cssClass, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class='pagination'>");
            if (currentPage == 1)
            {
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-step-backward'></i></a></li>");
                sb.Append("<li class='paginate_button previous disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-caret-left'></i></a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_first'><a href='" + pageUrl(1) + "' idata='1'><i class='fa fa-step-backward'></i></a></li>");
                sb.Append("<li class='paginate_button previous' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_previous'><a href='" + pageUrl(currentPage - 1) + "' idata='" + (currentPage - 1) + "'><i class='fa fa-caret-left'></i></a></li>");
            }

            const int pageHold = 10;
            var totalHold = totalPage / pageHold + 1;
            var currentHold = currentPage / pageHold >= 1 && currentPage % pageHold >= 1 ?
                currentPage / pageHold + 1 : currentPage / pageHold;
            currentHold = currentHold == 0 ? 1 : currentHold;

            var pointStart = 1;

            if (currentPage / pageHold >= 1 && currentPage % pageHold >= 1)
                pointStart = currentPage / pageHold * pageHold + 1;
            else if (currentPage / pageHold > 0)
                pointStart = (currentPage / pageHold - 1) * pageHold + 1;

            if (currentHold == 1)
            {
                //sb.Append("<div class='t-numeric'>");
                for (var i = pointStart; i <= ((totalPage < pageHold) ? totalPage : pointStart + pageHold - 1); i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                if (totalHold > 1)
                    sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "' idata='" + (pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }
            else if (currentHold == totalHold)
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "' idata='" + (pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= totalPage; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                //sb.Append("</div>");
            }
            else
            {
                //sb.Append("<div class='t-numeric'>");
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * (currentHold - 1)) + "' idata='" + (pageHold * (currentHold - 1)) + "'>...</a></li>");
                for (var i = pointStart; i <= pointStart + pageHold - 1; i++)
                {
                    if (i == currentPage)
                    {
                        sb.AppendFormat("<li class='paginate_button active' aria-controls='dynamic-table' tabindex='0'><a href='javascript:void(0);'>{0}</a></li>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(i) + "' idata='" + i + "'>{0}</a></li>", i);
                    }
                }
                sb.Append("<li class='paginate_button' aria-controls='dynamic-table' tabindex='0'><a href='" + pageUrl(pageHold * currentHold + 1) + "' idata='" + (pageHold * currentHold + 1) + "'>...</a></li>");
                //sb.Append("</div>");
            }

            if (currentPage == totalPage)
            {
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-caret-right'></i></a></li>");
                sb.Append("<li class='paginate_button next disabled' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='javascript:void(0);' rel='nofollow'><i class='fa fa-step-forward'></i></a></li>");
            }
            else
            {
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_next'><a href='" + pageUrl(currentPage + 1) + "' idata='" + (currentPage + 1) + "'><i class='fa fa-caret-right'></i></a></li>");
                sb.Append("<li class='paginate_button next' aria-controls='dynamic-table' tabindex='0' id='dynamic-table_last'><a href='" + pageUrl(totalPage) + "' idata='" + totalPage + "'><i class='fa fa-step-forward'></i></a></li>");
            }
            //Chọn số bản ghi cần hiển thị

            sb.Append("<li class=\"paginate_button next\" aria-controls=\"dynamic-table\" tabindex=\"0\" id=\"dynamic-table_last\">");
            //sb.Append("<select class=\"form-control cssPagingSelect\">");
            //sb.Append(string.Format("<option value=\"10\" {0}>10</option>", itemsPerPageingFooter == 10 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"20\" {0}>20</option>", itemsPerPageingFooter == 20 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"30\" {0}>30</option>", itemsPerPageingFooter == 30 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"50\" {0}>50</option>", itemsPerPageingFooter == 50 ? "selected='selected'" : ""));
            //sb.Append(string.Format("<option value=\"100\" {0}>100</option>", itemsPerPageingFooter == 100 ? "selected='selected'" : ""));
            //sb.Append("</select>");
            sb.Append("</li>");
            sb.Append("<li class=\"paginate_button next\" aria-controls=\"dynamic-table\" tabindex=\"0\" >");
            sb.Append("<p style=\"line-height:30px;margin-left: 10px;color: #777;\">");
            sb.Append("/ <b>" + totalItem + "</b> bản ghi");
            sb.Append("</p>");
            sb.Append("</li>");

            sb.Append("</ul>");
            //sb.Append("</div>");
            //sb.Append("<div class='t-pager-size'><div class='t-pager-size-chosen'>10</div><ul>" +
            //          "<li>10</li>" +
            //          "<li>15</li>" +
            //          "<li>20</li>" +
            //          "<li>50</li>" +
            //          "<li>100</li>" +
            //          "</ul>" +
            //          "<div class='sprite t-icon-arrow-bottom'></div></div>");
            //sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}
