using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class API_User
    {
        public string UserId { get; set; } = "";

        public string Username { get; set; } = "";

        public string Name { get; set; } = "";

        public string Avatar { get; set; } = "";
    }

    public class API_User_Login
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string KeyPass { get; set; } = "";
    }
}
