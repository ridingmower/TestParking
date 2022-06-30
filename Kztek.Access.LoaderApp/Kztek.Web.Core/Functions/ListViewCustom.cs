using Kztek.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;
using Kztek.Model.CustomModel;

namespace Kztek.Web.Core.Functions
{
    public class ListViewCustom
    {
        public static List<IconModel> GetListIcon(string path)
        {
            XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath(path));

            var regions = from region in xmlDoc.Descendants("Rows")
                          select new
                          {
                              IconValue = region.Element("IconValue").Value,
                              IconName = region.Element("IconName").Value,
                          };

            var ListIcon = new List<IconModel>();

            foreach (var region in regions)
            {
                ListIcon.Add(new IconModel()
                {
                    IconVal = region.IconValue,
                    IconNam = region.IconName,
                    IconDis = String.Format("{0} {1}", region.IconValue, region.IconName)
                });
            }
            return ListIcon;
        }

        /// <summary>
        /// Lay danh sach tinh thanh quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        public static List<DistrictModel> GetProvinceList(string path)
        {
            XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath(path));

            var regions = from region in xmlDoc.Descendants("Rows")
                          where int.Parse(region.Element("ParentID").Value) == 0
                          select new
                          {
                              TinhThanhID = region.Element("TinhThanhID").Value,
                              Name = region.Element("Name").Value,
                              ParentID = region.Element("ParentID").Value,
                          };

            var tinhThanhList = new List<DistrictModel>();

            foreach (var region in regions)
            {
                tinhThanhList.Add(new DistrictModel()
                {
                    DistrictId = region.TinhThanhID,
                    DistrictName = region.Name
                });
            }
            return tinhThanhList;
        }

        /// <summary>
        /// Lay danh sach quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        public static List<DistrictModel> GetDistrictList(int tinhThanhId, string path)
        {
            XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath(path));

            var regions = from region in xmlDoc.Descendants("Rows")
                          where int.Parse(region.Element("ParentID").Value) == tinhThanhId
                          select new
                          {
                              TinhThanhID = region.Element("TinhThanhID").Value,
                              Name = region.Element("Name").Value,
                              ParentID = region.Element("ParentID").Value,
                          };

            var quanHuyenList = new List<DistrictModel>();

            foreach (var region in regions)
            {
                quanHuyenList.Add(new DistrictModel()
                {
                    DistrictId = region.TinhThanhID,
                    DistrictName = region.Name
                });
            }
            return quanHuyenList;
        }
    }
}
