using System.Web;
using System.Web.Optimization;

namespace Kztek.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Fonts
            bundles.Add(new StyleBundle("~/Content/AdminLayout/AdminFont").Include(
                      "~/Content/Fonts/font_global.css",
                      "~/Content/Fonts/font_vn.css"
                ));

            //ACE css
            bundles.Add(new StyleBundle("~/Content/AdminLayout/Admincss").Include(
                      "~/Content/AdminLayout/css/bootstrap.css",
                      "~/Content/AdminLayout/css/colorbox.css",
                      "~/Content/AdminLayout/css/ace.css",
                      "~/Content/AdminLayout/css/ace-rtl.css",                   
                      "~/Content/AdminLayout/css/daterangepicker.css",
                      "~/Content/AdminLayout/css/chosen.css",
                      "~/Content/AdminLayout/css/jquery-ui.css",
                      "~/Content/AdminLayout/css/jquery-ui.custom.css",
                      "~/Content/AdminLayout/css/bootstrap-datetimepicker.css",
                      "~/Content/AdminLayout/css/bootstrap-timepicker.css",
                      "~/Content/AdminLayout/css/bootstrap-multiselect.css"
                ));

            bundles.Add(new StyleBundle("~/Content/AdminLayout/AdminExtcss").Include(
                    "~/Content/NewConfigStyle.css",
                    "~/Content/ToastrJquery/toastr.css",
                    "~/Content/Zoom/jquery.spzoom.css"
                ));

            //ACE js
            bundles.Add(new ScriptBundle("~/Content/AdminLayout/Adminjs").Include(
                      "~/Content/AdminLayout/js/bootstrap.js",
                      "~/Content/AdminLayout/js/jquery.colorbox.js",
                      "~/Content/AdminLayout/js/ace-elements.js",
                      "~/Content/AdminLayout/js/ace.js",
                      "~/Content/AdminLayout/js/moment.js",
                      "~/Content/AdminLayout/js/daterangepicker.js",
                      "~/Content/AdminLayout/js/chosen.jquery.js",
                      "~/Content/AdminLayout/js/bootstrap-tag.js",
                      "~/Content/AdminLayout/js/bootbox.js",
                      "~/Content/AdminLayout/js/jquery-ui.js",
                      "~/Content/AdminLayout/js/jquery-ui.custom.js",
                      "~/Content/AdminLayout/js/spinbox.js",
                      "~/Content/AdminLayout/js/bootstrap-datetimepicker.js",
                      "~/Content/AdminLayout/js/bootstrap-timepicker.js",
                      "~/Content/AdminLayout/js/bootstrap-multiselect.js",
                      "~/Content/AdminLayout/js/bootbox.js",
                      "~/Content/AdminLayout/js/jquery.easypiechart.js",
                      "~/Content/AdminLayout/js/ace-extra.js"
                ));

            bundles.Add(new ScriptBundle("~/Content/AdminLayout/AdminExtjs").Include(
                      "~/Scripts/Ext/Common.js",
                      //"~/Scripts/function.Ext.js",
                      "~/Scripts/ToastrJquery/toastr.js",
                //"~/Scripts/jquery.signalR-2.2.2.js",
                //"~/Scripts/AjaxPaging/jquery.twbsPagination.js"
                "~/Scripts/jquery.mask.js",
                "~/Content/Zoom/jquery.spzoom.js",
                "~/Scripts/Ext/FunctionHelperController.js",
                "~/Scripts/canvasjs.min.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            //"~/Scripts/InputMask/inputmask.js",
            //"~/Scripts/InputMask/jquery.inputmask.js"
            ////"~/Scripts/jInputMask/inputmask.extensions.js",
            ////"~/Scripts/InputMask/inputmask.date.extensions.js",
            //////and other extensions you want to include
            ////"~/Scripts/InputMask/inputmask.numeric.extensions.js"
            //));

            BundleTable.EnableOptimizations = false;
        }
    }
}
