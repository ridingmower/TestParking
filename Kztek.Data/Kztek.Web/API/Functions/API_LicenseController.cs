using Kztek.Model.Models.API;
using Kztek.Security;
using Kztek.Service.Admin;
using Kztek.Web.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API.Functions
{
    [RoutePrefix("api/license")]
    public class API_LicenseController : ApiController
    {
        private ItblSystemConfigService _tblSystemConfigService;
        public API_LicenseController(ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [Route("getlicense")]
        [HttpGet]
        public async Task<string> GetLicense()
        {
            string result = "";
            try
            {
                using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"uploads\lic.dat", FileMode.Open, FileAccess.ReadWrite))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        var feename = _tblSystemConfigService.GetDefault().FeeName;

                        var fileContent = sr.ReadToEnd();
                        var decryptedData = CryptoProvider.SimpleDecryptWithPassword(fileContent, SecurityModel.License_Key);
                        var licData = JsonConvert.DeserializeObject<List<MN_License>>(decryptedData);
                        var currentLic = licData.Where(p => p.ProjectName == feename).FirstOrDefault();
                        if(currentLic != null)
                        {
                            var expireDate = DateTime.Now;
                            DateTime.TryParseExact(currentLic.ExpireDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expireDate);

                            result = $"{currentLic.ProjectName};{currentLic.IsExpire};{expireDate.ToString("yyyy/MM/dd HH:mm")}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return await Task.FromResult(result);
        }
    }
}