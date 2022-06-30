using Kztek.Model.Models;
using Kztek.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kztek.Web.API.Functions
{
    [Route("api/systemconfig")]
    public class API_SystemConfigController : ApiController
    {
        private ItblSystemConfigService _tblSystemConfigService;

        public API_SystemConfigController(ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [HttpGet]
        public tblSystemConfig Get()
        {
            return _tblSystemConfigService.GetDefault();
        }
    }
}
