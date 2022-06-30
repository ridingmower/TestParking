using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    [CheckSessionLogin]
    [CheckAuthorize]
    public class MultipleLanesMapController : Controller
    {
        private ItblPCService _tblPCService;
      
        private IMultipleLanesMapService _MultipleLanesMapService;
        public MultipleLanesMapController(ItblPCService _tblPCService , IMultipleLanesMapService _MultipleLanesMapService)
        {
            this._tblPCService = _tblPCService;
            this._MultipleLanesMapService = _MultipleLanesMapService;
        }
        // GET: Parking/MultipleLanesMap///////////////
        public ActionResult Index()
        {
            var lst = _MultipleLanesMapService.GetAll();         
            ViewBag.ListPC = _tblPCService.GetAllActive().ToList();
            ViewBag.ListSideIndex = FunctionHelper.SideIndex();
         
            ViewBag.ListCurrectionDirect = FunctionHelper.ListCurrentDirection();
            var model = new MultipleLanesMap(); 
            
            return View(lst);
        }
        public PartialViewResult ShowView(string vlue)
        {
            var result = new MessageReport(false, "có lỗi");
            var lst = _MultipleLanesMapService.GetAll();
            ViewBag.Count =  Convert.ToInt32( vlue);  /*vlue != null ? Convert.ToInt32(vlue) : Convert.ToInt32("0");*/
            ViewBag.ListPC = _tblPCService.GetAllActive().ToList();
            ViewBag.ListSideIndex = FunctionHelper.SideIndex();
            ViewBag.lst = lst;
            ViewBag.ListCurrectionDirect = FunctionHelper.ListCurrentDirection();
            var model = new MultipleLanesMap();
            return PartialView(lst);
        }

        //public JsonResult SavePcId(List<MultipleLanesMap> list)
        //{
        //    var result = new MessageReport(false, "Có lỗi");

        //    foreach (var item in list)
        //    {
        //        var ob = _MultipleLanesMapService.GetByViewOrder(item.ViewOrder);
        //        if (ob != null)
        //        {
        //            result = new MessageReport(false, "Đã tồn tại"); f
        //        }
        //        else
        //        {
        //            var obj = new MultipleLanesMap();
        //            obj.Id = Guid.NewGuid().ToString();
        //            obj.PCid = item.PCid;
        //            obj.ViewOrder = Convert.ToInt32(item.ViewOrder);
        //            obj.CurrentDirection = Convert.ToInt32(item.CurrentDirection);
        //            obj.SideIndex = Convert.ToInt32(item.SideIndex);
        //            result = _MultipleLanesMapService.Create(obj);
        //        }

        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult DeleteAll()
        {
            var result = new MessageReport(false, "Có lỗi");

            result = _MultipleLanesMapService.DeleteAll();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult ModalCreateAndUpdatePC(string ViewOrder)
        {
           
            ViewBag.ListPC = _tblPCService.GetAllActive().ToList();
            ViewBag.ListSideIndex = FunctionHelper.SideIndex();
            ViewBag.ListCurrectionDirect = FunctionHelper.ListCurrentDirection();
            var model = _MultipleLanesMapService.GetByViewOrder(Convert.ToInt32(ViewOrder));

            ViewBag.Count = Convert.ToInt32(ViewOrder);
            var m1 = new MultipleLanesMap_Cus();
            if (model != null)
            {
               
                m1.Name = model.Name;
                m1.CurrentDirection = model.CurrentDirection.ToString();
                m1.SideIndex = model.SideIndex.ToString();
                m1.PCid = model.PCid;
                m1.Id = model.Id.ToString();
                

               
            }
            else
            {
                m1 = new MultipleLanesMap_Cus();
            }

            return PartialView(m1);
        }

        public JsonResult SaveOnlyPc(MultipleLanesMap_Cus obj)
        {
            var result = new MessageReport(false, "Có lỗi");
            
            var model = _MultipleLanesMapService.GetByViewOrder(Convert.ToInt32( obj.ViewOrder));
            if (model != null)
            {
                model.Name = obj.Name;
                model.PCid = obj.PCid;
                model.CurrentDirection =Convert.ToInt32( obj.CurrentDirection);
                model.SideIndex = Convert.ToInt32( obj.SideIndex);
                result = _MultipleLanesMapService.Update(model);
            }
            
            else
            {
                var model1 = new MultipleLanesMap();
                model1.CurrentDirection = Convert.ToInt32(obj.CurrentDirection);
                model1.Id = Guid.NewGuid().ToString();
                model1.Name = obj.Name;
                model1.PCid = obj.PCid.ToString();
                model1.SideIndex = Convert.ToInt32(obj.SideIndex);
                model1.ViewOrder = Convert.ToInt32( obj.ViewOrder);
                result = _MultipleLanesMapService.Create(model1);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}