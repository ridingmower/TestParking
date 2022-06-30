using Kztek.Model.CustomModel;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.Web.Core.Functions
{
    public class GetCurrentSuperAdmin
    {
        public static bool CheckCurrentLogin()
        {
            var _user = GetUser();
            return _user == null ? false : true;
        }

        public static AdminLoginModel GetUser()
        {
            var host = HttpContext.Current.Request.Url.Host;
            var _user = (AdminLoginModel)HttpContext.Current.Session[string.Format("{0}_{1}", SessionConfig.SuperAdminSession, host)];
           
            //if (_user == null)
            //{
                
            //}

            return _user;
        }
    }
}
