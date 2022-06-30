using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kztek.ReportAPI
{
    public class ReportAPI
    {
        public string Host { get; set; }

        public int Port { get; set; } = 80;
        
        public ReportAPI(string host, int port = 80)
        {
            this.Host = host;
            this.Port = port;
        }

        private bool GetReport(string routename, Dictionary<string, string> keyValues, HttpMethod method)
        {
            bool result = false;

            using (var api = new API())
            {
                var uribuilder = new UriBuilder()
                {
                    Host = this.Host,
                    Port = this.Port,
                    Path = routename
                };

                HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };

                if (method == HttpMethod.Get)
                {
                    uribuilder.Query = api.BuildParameters(keyValues);
                    response = api.Get(uribuilder.ToString());
                }
                else if (method == HttpMethod.Post)
                {
                    response = api.Post(uribuilder.ToString(), keyValues);
                }

                if (response != null & response.StatusCode == HttpStatusCode.OK)
                {
                    var fileName = response.Content.Headers.ContentDisposition.FileName;
                    using (var sfd = new SaveFileDialog() { FileName = fileName, Filter = "Excel (.xlsx)|*.xlsx" })
                    {
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            var content = response.Content.ReadAsByteArrayAsync().Result;
                            File.WriteAllBytes(sfd.FileName, content);
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}
