using Kztek.Data;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Functions
{
    public class FunctionAppHelper
    {
        //public static WebConfig getWebConfigFromCache(string conn = "")
        //{
        //    var model = new WebConfig();

        //    using (var db = new KztekEntities(conn))
        //    {
        //        model = db.WebConfigs.FirstOrDefault();
        //    }

        //    return model;
        //}

        public static void GetDateTimeOutStatus(string timeOutInEvent, string timeOutInCommand, int timeAlert, ref string result, ref string status)
        {
            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = t.ToString();

                    status = "2";
                }
                else
                {
                    result = t.ToString();

                    status = "3";
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = newT.ToString();

                    status = "1";
                }
                else
                {
                    result = newT.ToString();

                    status = "3";
                }
            }
        }
    }
}
