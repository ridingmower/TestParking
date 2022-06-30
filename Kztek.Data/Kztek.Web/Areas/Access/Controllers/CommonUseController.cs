using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class CommonUseController : Controller
    {
        #region Khai báo services
        
        public CommonUseController()
        {
            
        }
        #endregion

        /// <summary>
        /// Dropdownlist multi select
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             08/07/2017      Tạo mới
        /// </modified>
        /// <param name="_dt">Danh sách dạng datatable</param>
        /// <param name="itemValue">Cột giá trị</param>
        /// <param name="itemText">Cột hiển thị</param>
        /// <param name="selectedValues">Giá trị đã lựa chọn</param>
        /// <param name="Modelname">Tên select đó</param>
        /// <returns></returns>
        public PartialViewResult DroplistMultiSelectTemplate(DataTable _dt, string itemValue, string itemText, string selectedValues, string Modelname)
        {
            //var list = _GateService.GetAllActive();
            ViewBag.Selected = selectedValues;
            ViewBag.ivalue = itemValue;
            ViewBag.itext = itemText;
            ViewBag.iName = Modelname;
            return PartialView(_dt);
        }

        /// <summary>
        /// Dropdownlist multi select với 2 tên hiển thị
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             08/07/2017      Tạo mới
        /// </modified>
        /// <param name="_dt">Danh sách dạng datatable</param>
        /// <param name="itemValue">Cột giá trị</param>
        /// <param name="itemText">Cột hiển thị thứ nhất</param>
        /// <param name="itemSecondText">Cột hiển thị thứ hai</param>
        /// <param name="selectedValues">Giá trị đã lựa chọn</param>
        /// <param name="Modelname">Tên select</param>
        /// <returns></returns>
        public PartialViewResult DroplistMultiSelectTemplate1(DataTable _dt, string itemValue, string itemText, string itemSecondText, string selectedValues, string Modelname)
        {
            //var list = _GateService.GetAllActive();
            ViewBag.Selected = selectedValues;
            ViewBag.ivalue = itemValue;
            ViewBag.itext = itemText;
            ViewBag.iName = Modelname;
            ViewBag.isecondtext = itemSecondText;

            return PartialView(_dt);
        }

        /// <summary>
        /// Dropdown đang có tìm kiếm
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             08/07/2017      Tạo mới
        /// </modified>
        /// <param name="_dt">Danh sách dạng datatable</param>
        /// <param name="itemValue">Giá trị </param>
        /// <param name="itemText">Cột hiển thị</param>
        /// <param name="selectedValue">Giá trị đã lựa chọn</param>
        /// <param name="Modelname">Tên select</param>
        /// <param name="labelname">Tên label</param>
        /// <returns></returns>
        public PartialViewResult DroplistChosenTemplate(DataTable _dt, string itemValue, string itemText, string selectedValue, string Modelname, string labelname)
        {
            ViewBag.Selected = selectedValue;
            ViewBag.ivalue = itemValue;
            ViewBag.itext = itemText;
            ViewBag.iName = Modelname;
            ViewBag.iLabel = labelname;
            return PartialView(_dt);
        }

        /// <summary>
        /// Dropdown dạng có tìm kiếm
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             08/07/2017      Tạo mới
        /// </modified>
        /// <param name="_dt">Danh sách dạng datatable</param>
        /// <param name="itemValue">Cột giá trị</param>
        /// <param name="itemText">Cột hiển thị thứ nhất</param>
        /// <param name="itemSecondText">Cột hiển thị thứ hai</param>
        /// <param name="selectedValue">Giá trị đã lựa chọn</param>
        /// <param name="Modelname">Tên select</param>
        /// <param name="labelname">Tên label</param>
        /// <returns></returns>
        public PartialViewResult DroplistChosenTemplate1(DataTable _dt, string itemValue, string itemText, string itemSecondText, string selectedValue, string Modelname, string labelname)
        {
            ViewBag.Selected = selectedValue;
            ViewBag.ivalue = itemValue;
            ViewBag.itext = itemText;
            ViewBag.isecondtext = itemSecondText;
            ViewBag.iName = Modelname;
            ViewBag.iLabel = labelname;
            return PartialView(_dt);
        }

        /// <summary>
        /// Dropdown dạng có tìm kiếm
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             08/07/2017      Tạo mới
        /// </modified>
        /// <param name="_dt">Danh sách dạng datatable</param>
        /// <param name="itemValue">Cột giá trị</param>
        /// <param name="itemText">Cột hiển thị thứ nhất</param>
        /// <param name="itemSecondText">Cột hiển thị thứ hai</param>
        /// <param name="selectedValue">Giá trị đã chọn</param>
        /// <param name="Modelname">Tên select</param>
        /// <param name="labelname">Tên label</param>
        /// <returns></returns>
        public PartialViewResult DroplistChosenTemplate2(DataTable _dt, string itemValue, string itemText, string itemSecondText, string selectedValue, string Modelname, string labelname)
        {
            ViewBag.Selected = selectedValue;
            ViewBag.ivalue = itemValue;
            ViewBag.itext = itemText;
            ViewBag.isecondtext = itemSecondText;
            ViewBag.iName = Modelname;
            ViewBag.iLabel = labelname;
            return PartialView(_dt);
        }
    }
}