using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.ADFS
{
    public class ADFS_User
    {
        public string Email { get; set; }
        public string Id_Token { get; set; }
        public string Refresh_Token { get; set; }
        public string Access_Token { get; set; }
    }
}
