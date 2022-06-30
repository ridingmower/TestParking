using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kztek.Model.CustomModel
{
    public class AdminLoginModel
    {
        //[Required(ErrorMessage ="Vui lòng nhập email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string UserName { get; set; }

        public bool Remember { get; set; }
    }
}