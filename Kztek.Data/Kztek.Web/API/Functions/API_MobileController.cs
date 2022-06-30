using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using Kztek.Model.Models.Event;
using System.Windows.Forms;
using Kztek.Repository.API;
using Kztek.Data.Event.SqlHelper;
using Kztek.Web.Core.Helpers;
using Newtonsoft.Json;

namespace Kztek.Web.API.Functions
{
    [RoutePrefix("api/mobile")]
    public partial class API_MobileController : ApiController
    {
        private IAPI_MobileService _API_MobileService;
        private ItblSystemConfigService _tblSystemConfigService;
        public API_MobileController(IAPI_MobileService _API_MobileService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._API_MobileService = _API_MobileService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [Route("convertcard")]
        [HttpPost]
        public async Task<string> ConvertCard(API_CardConvert cardconvert)
        {
            var result = "";

            if (cardconvert.CardNumber.Length == 8)
            {
                result = ConvertCard(cardconvert.CardNumber);
            }

            return await Task.FromResult(result);
        }

        private string ConvertCard(string cardnumber)
        {
            var part1 = cardnumber.Substring(0, 2);
            var part2 = cardnumber.Substring(2, 2);
            var part3 = cardnumber.Substring(4, 2);
            var part4 = cardnumber.Substring(6, 2);

            var result = part4 + part3 + part2 + part1;

            return result;
        }

        [Route("getlaneslist")]
        [HttpGet]
        public async Task<IEnumerable<API_Lane>> GetLanesList()
        {
            var listLane = new List<API_Lane>()
            {
                new API_Lane() { Id = "" , Name = "-- Lựa chọn --"}
            };

            var _lanes = _API_MobileService.GetLanesList().Select(p => new API_Lane()
            {
                Id = $"{p.LaneID.ToString()}#{p.LaneName}",
                Name = $"{p.LaneName}#{0}"
            });

            listLane.AddRange(_lanes);

            return await Task.FromResult(listLane);
        }

        [Route("getbyid/{id}")]
        [HttpGet]
        public async Task<User> GetById(string id)
        {
            return await Task.FromResult(_API_MobileService.GetUserById(id));
        }

        [Route("checkin")]
        [HttpPost]
        public async Task<MessageReport> CheckIn()
        {
            var report = new MessageReport() { isSuccess = false };
            tblCardEvent postbackEvent = null;
            string msg = "";

            try
            {
                var _API_EventInOUt = new API_EventInOut()
                {
                    CardNumber = HttpContext.Current.Request.Params["CardNumber"],
                    UserId = HttpContext.Current.Request.Params["UserId"],
                    LaneId = HttpContext.Current.Request.Params["LaneId"],
                };

                _API_EventInOUt.CardNumber = ConvertCard(_API_EventInOUt.CardNumber);

                Image imageData = null;
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                imageData = Image.FromStream(httpPostedFile.InputStream);

                //using (var ms = new MemoryStream())
                //{
                //    httpPostedFile.InputStream.CopyTo(ms);
                //    imageData = new Bitmap(ms);
                //}

                ProcessCardEventIn(_API_EventInOUt, imageData, ref postbackEvent, ref msg);
                if (postbackEvent != null)
                {
                    if(!string.IsNullOrWhiteSpace(postbackEvent.PicDirIn))
                        postbackEvent.PicDirIn = postbackEvent.PicDirIn.Replace($@"\\{Environment.MachineName}", "").Replace(@"\", @"/");

                    report.isSuccess = true;
                    report.Message = JsonConvert.SerializeObject(postbackEvent, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                }
                else
                {
                    report.Message = msg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(report);
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<MessageReport> CheckOut()
        {
            var report = new MessageReport() { isSuccess = false };
            tblCardEvent postbackEvent = null;
            string msg = "";

            try
            {
                var _API_EventInOUt = new API_EventInOut()
                {
                    CardNumber = HttpContext.Current.Request.Params["CardNumber"],
                    UserId = HttpContext.Current.Request.Params["UserId"],
                    LaneId = HttpContext.Current.Request.Params["LaneId"],
                };

                Image imageData = null;
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                imageData = Image.FromStream(httpPostedFile.InputStream);
                //using (var ms = new MemoryStream())
                //{
                //    httpPostedFile.InputStream.CopyTo(ms);
                //    imageData = new Bitmap(ms);
                //}

                _API_EventInOUt.CardNumber = ConvertCard(_API_EventInOUt.CardNumber);

                ProcessCardEventOut(_API_EventInOUt, imageData, ref postbackEvent, ref msg);
                if (postbackEvent != null)
                {
                    if (!string.IsNullOrWhiteSpace(postbackEvent.PicDirIn))
                        postbackEvent.PicDirIn = postbackEvent.PicDirIn.Replace($@"\\{Environment.MachineName}", "").Replace(@"\", @"/");

                    if (!string.IsNullOrWhiteSpace(postbackEvent.PicDirOut))
                        postbackEvent.PicDirOut = postbackEvent.PicDirOut.Replace($@"\\{Environment.MachineName}", "").Replace(@"\", @"/");

                    report.isSuccess = true;
                    report.Message = JsonConvert.SerializeObject(postbackEvent, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                }
                else
                {
                    report.Message = msg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return await Task.FromResult(report);
        }
    }
}