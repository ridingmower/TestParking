using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Models
{
    public class ActionConfig
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string ExportExcel = "ExExcel";
        public const string PrintFile = "PrintFile";
        public const string Login = "Login";
    }

    public class ActionConfigO
    {
        public const string Create = "Thêm mới";
        public const string Update = "Sửa";
        public const string Delete = "Xóa";
        public const string ExportExcel = "ExExcel";
        public const string ImportExcel = "ImExcel";
        public const string PrintFile = "PrintFile";
        public const string Login = "Đăng nhập";
        public const string UpdateFeeActiveCard = "Sửa số tiền gia hạn thẻ";
        public const string Lock = "Khóa";
        public const string Unlock = "Mở khóa";
        public const string Restore = "Phục hồi mật khẩu";
        public const string Authorize = "Phân quyền";
    }
}
