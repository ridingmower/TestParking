using Kztek.Data;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using Kztek.Web.Core.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Model.CustomModel;
using System.Text;
using Kztek.Data.SqlHelper;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Web.Configuration;
using Kztek.Web.Core.Functions;
using Kztek.Security;
using System.Drawing;

namespace Kztek.Web.Core.Functions
{
    public class FunctionHelper
    {
        /// <summary>
        /// Test
        /// </summary>
        public static string Cnn = ConfigurationManager.ConnectionStrings["KztekEntities"].ConnectionString;
        public static List<object> TemplateStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "Image", ItemText = "Hình ảnh"},
                                         new  { ItemValue = "Template", ItemText = "Template"}
                                    };
            return list;
        }

        //test122
        public static List<SelectListModel> TimePeriodType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Tháng" },
                                         new SelectListModel { ItemValue = "1", ItemText = "Quý" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Năm" },
                                         new SelectListModel { ItemValue = "3", ItemText = "Ngày" }
                                    };
            return list;
        }
        public static List<SelectListModel6> FunctionController()
        {
            var list = new List<SelectListModel6> {
                                         new SelectListModel6 { ItemValue = 0, ItemText = "Access" },
                                         new SelectListModel6 { ItemValue = 1, ItemText = "FireAlarm" },
                                         new SelectListModel6 { ItemValue = 2, ItemText = "Khác" }

                                    };
            return list;
        }


        public static List<SelectListModel> AllResolution()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "SDKs1");
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "720x480", ItemText = "D1 (720x480)"},
                new  SelectListModel{ ItemValue = "1280x720", ItemText = "720p HD (1280x720)"},
                new  SelectListModel{ ItemValue = "1280x960", ItemText = "960p HD (1280x960)"},
                new  SelectListModel{ ItemValue = "1280x1024", ItemText = "1.3 MP(1280x1024)"},
                new  SelectListModel{ ItemValue = "1600x1200", ItemText = "2 MP (1600x1200)"},
                new  SelectListModel{ ItemValue = "1920x1080", ItemText = "1080p HD (1920x1080)"},
                new  SelectListModel{ItemValue = "2048x1536", ItemText = "3 MP (2048x1536)"},

            };
        }
        public static List<SelectListModel> ActionUploadProcess()
        {
            var list = new List<SelectListModel> {
                  new SelectListModel { ItemValue = "", ItemText = "-- Tất cả --" },
                                         new SelectListModel { ItemValue = "UPLOAD", ItemText = "Nạp" },
                                         new SelectListModel { ItemValue = "DELETE", ItemText = "Hủy" },
                                         new SelectListModel { ItemValue = "EXTEND", ItemText = "Gia hạn" }

                                    };
            return list;
        }

        public static List<SelectListModel> EventType()
        {
            var list = new List<SelectListModel> {
                  new SelectListModel { ItemValue = "", ItemText = "-- Tất cả --" },
                                         new SelectListModel { ItemValue = "Card", ItemText = "Thẻ" },
                                         new SelectListModel { ItemValue = "Finger", ItemText = "Vân tay" }

                                    };
            return list;
        }

        public static List<SelectListModel> EventStatus()
        {
            var list = new List<SelectListModel> {
                  new SelectListModel { ItemValue = "", ItemText = "-- Tất cả --" },
                                         new SelectListModel { ItemValue = "OK", ItemText = "Thành công" },
                                         new SelectListModel { ItemValue = "Failed", ItemText = "Thất bại" }

                                    };
            return list;
        }

        public static List<object> PayStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "2", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }

        public static List<object> PayStatus1()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "true", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "false", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }

        public static List<SelectListModel> CommandStatus()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = "-- Lựa chọn --"},
                                         new SelectListModel { ItemValue = "0", ItemText = "Chưa lập lệnh"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Đã lập lệnh"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Chưa đủ điều kiện"},
                                         new  SelectListModel{ ItemValue = "101", ItemText = "Lập lệnh xe đã ra"}
                                    };
            return list;
        }

        public static List<SelectListModel> CommandStatus1()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Chưa lập lệnh"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Đã lập lệnh"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Chưa đủ điều kiện"}
                                    };
            return list;
        }
        public static List<SelectListModel> TypePay()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Chuyển khoản"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Tiền mặt"}

                                    };
            return list;
        }

        public static List<object> FinishStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn tiến độ --"},
                                         new  { ItemValue = "1", ItemText = "Đã hoàn thành"},
                                         new  { ItemValue = "2", ItemText = "Chưa hoàn thành"}
                                    };
            return list;
        }


        public static List<object> GetProductStatusList()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "hethang", ItemText = "Sản phẩm đang hết hàng"},
                                         new  { ItemValue = "dangnhap", ItemText = "Sản phẩm đang nhập hàng"},

                                    };
            return list;
        }

        public static List<object> ActiveContract()
        {
            var list = new List<object> {
                new { ItemValue = "True", ItemText = "Hiệu lực" },
                new { ItemValue = "False", ItemText = "Hết hiệu lực" }
            };
            return list;
        }
        public static List<object> ActiveStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn trạng thái --"},
                                         new  { ItemValue = "True", ItemText = "Kích hoạt"},
                                         new  { ItemValue = "False", ItemText = "Chưa kích hoạt"}
                                    };
            return list;
        }
        public static List<SelectListModel> ScheduleType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Hàng tháng"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Hàng năm"}

                                    };
            return list;
        }
        public static List<SelectListModel> StatusEventAccess()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "--Tất cả--"},
                                         new SelectListModel { ItemValue = "Access Granted", ItemText = "Access Granted"},
                                         new SelectListModel { ItemValue = "Access Denied", ItemText = "Access Denied"}

                                    };
            return list;
        }
        public static List<SelectListModel> CheckTypeEvent()
        {
            var list = new List<SelectListModel>
            {
                    new SelectListModel {ItemValue = "0" ,  ItemText= "--Tất cả --"},
                    new SelectListModel {ItemText = "Finger" , ItemValue = "Finger"},
                     new SelectListModel {ItemText = "Card" , ItemValue = " "},
            };
            return list;
        }

        public static List<SelectListModel> Purpose()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Ở"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Cho thuê"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Kinh doanh"}

                                    };
            return list;
        }
        public static List<SelectListModel> TypeElec()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Điện gia đình"},

                                         new SelectListModel { ItemValue = "1", ItemText = "Điện kinh doanh"}

                                    };
            return list;
        }
        public static List<SelectListModel> CardStatus()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CardStatus");
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = Dictionary["select"]},
                                         new SelectListModel { ItemValue = "0", ItemText = Dictionary["active"]},
                                         new SelectListModel { ItemValue = "1", ItemText = Dictionary["inactive"]},
                                    };
            return list;
        }

        public static List<object> MenuType()
        {
            var list = new List<object> {
                                        new  { ItemValue = "1", ItemText = "Menu"},
                                         new  { ItemValue = "2", ItemText = "Function"}
                                    };
            return list;
        }
        //public static List<object> PositionStatus()
        //{
        //    return new List<object>     {
        //                                    new  { ItemValue = "0", ItemText = "-- Lựa chọn vị trí --"},
        //                                    new  { ItemValue = "Top", ItemText = "Trên"},
        //                                    new  { ItemValue = "Right", ItemText = "Phải"},
        //                                    new  { ItemValue = "Bottom", ItemText = "Dưới"},
        //                                    new  { ItemValue = "Left", ItemText = "Trái"},
        //                                    new  { ItemValue = "Center", ItemText = "Giữa"},
        //                                    new  { ItemValue = "LeftBody", ItemText = "Lề trái"},
        //                                    new  { ItemValue = "RightBody", ItemText = "Lề phải"},
        //                                    new  { ItemValue = "Slide", ItemText = "Slide"},
        //                                    new  { ItemValue = "Popup", ItemText = "PopUp"},
        //                                    new  { ItemValue = "Library", ItemText = "Thư viện"},
        //                                    new  { ItemValue = "Partner", ItemText = "Đối tác"}
        //                                };
        //}

        //public static List<object> PositionView()
        //{
        //    return new List<object>     {
        //                                    new  { ItemValue = "0", ItemText = "-- Lựa chọn vị trí --"},
        //                                    new  { ItemValue = "Home", ItemText = "Trang chủ"},
        //                                    new  { ItemValue = "Solution", ItemText = "Trang Giải pháp"},
        //                                    new  { ItemValue = "Product", ItemText = "Trang Sản phẩm"},
        //                                    new  { ItemValue = "Service", ItemText = "Trang Báo giá"},
        //                                    new  { ItemValue = "News", ItemText = "Tin tức"},
        //                                    new  { ItemValue = "Contact", ItemText = "Trang Liên hệ"}
        //                                };
        //}

        public static List<object> BannerStatus()
        {
            return new List<object>     {
                                            new  { ItemValue = "0", ItemText = "-- Lựa chọn hành động --"},
                                            new  { ItemValue = "run", ItemText = "Hoạt động"},
                                            new  { ItemValue = "pause", ItemText = "Tạm dừng"},
                                            new  { ItemValue = "stop", ItemText = "Hết hạn"}
                                        };
        }

        public static List<object> StockStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn tình trạng --"},
                                         new  { ItemValue = "1", ItemText = "Còn hàng"},
                                         new  { ItemValue = "2", ItemText = "Hết hàng"},
                                         new  { ItemValue = "3", ItemText = "Đang về"}
                                    };
            return list;
        }

        public static List<object> DeleteStatus()
        {
            var list = new List<object> {
                                         new  { ItemValue = "False", ItemText = "Chưa xóa"},
                                         new  { ItemValue = "True", ItemText = "Đã xóa"}
                                    };
            return list;
        }

        public static List<object> GetProductOrder()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Giá tăng dần"},
                                         new  { ItemValue = "2", ItemText = "Giá giảm dần"},
                                         new  { ItemValue = "3", ItemText = "Tên từ A - Z"},
                                         new  { ItemValue = "4", ItemText = "Tên từ Z - A" },
                                    };
            return list;
        }

        public static List<object> Status()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Sử dụng"},
                                         new  { ItemValue = "2", ItemText = "Không sử dụng"},
                                    };
            return list;
        }

        public static List<object> LEDType()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn loại--"},
                                         new  { ItemValue = "1", ItemText = "DSP840"},
                                         new  { ItemValue = "2", ItemText = "FUTECH"},
                                         new  { ItemValue = "3", ItemText = "FINGERTECH"},

                                    };
            return list;
        }

        public static List<SelectListModel> DateType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Ngày"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Tháng"}

                                    };
            return list;
        }
        public static List<SelectListModel> TypeEventLocker()
        {
            var list = new List<SelectListModel> {
                                         //new SelectListModel { ItemValue = "", ItemText = "--Lựa chọn--"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Nạp cố định"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Thẻ tức thời"},
                                         new  SelectListModel{ ItemValue = "3", ItemText = "Nhận dạng khuôn mặt"}

                                    };
            return list;
        }

        public static List<SelectListModel> Action()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "ActionCard");
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = Dictionary["select"]},
                                         new  SelectListModel{ ItemValue = "ADD", ItemText = Dictionary["add"]},
                                         new SelectListModel { ItemValue = "RELEASE", ItemText = Dictionary["release"]},
                                         new  SelectListModel{ ItemValue = "RETURN", ItemText = Dictionary["return"]},
                                         new SelectListModel { ItemValue = "CHANGE", ItemText = Dictionary["change"]},
                                         new  SelectListModel{ ItemValue = "DELETE", ItemText = Dictionary["del"]},
                                         new  SelectListModel{ ItemValue = "LOCK", ItemText = Dictionary["lock"]},
                                         new SelectListModel { ItemValue = "UNLOCK", ItemText =Dictionary["unlock"]},
                                         new  SelectListModel{ ItemValue = "ACTIVE", ItemText = Dictionary["active"]},
                                         new SelectListModel { ItemValue = "CHANGEPLA",ItemText = "Đổi biển số"},
                                         new  SelectListModel{ ItemValue = "UPDATEINFO", ItemText = "Đổi nhóm thẻ"}

                                    };
            return list;
        }

        public static List<object> TicketType()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn loại vé --"},
                                         new  { ItemValue = "1", ItemText = "Vé bán"},
                                         new  { ItemValue = "2", ItemText = "Vé trả lại"},
                                          new  { ItemValue = "3", ItemText = "Vé hủy"}
                                    };
            return list;
        }

        public static List<object> HubList()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn loại--"},
                                         new  { ItemValue = "1", ItemText = "Trái"},
                                         new  { ItemValue = "2", ItemText = "Phải"},
                                    };
            return list;
        }
        public static List<object> ListPC()
        {
            var list = new List<object>
            {
                new {ID = "", ComputerName = "--Chọn máy tính--" }
            };
            return list;
        }
        public static List<object> SelectOptionFilterOrder()
        {
            return new List<object> {
                                         new  { ItemValue = "", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "0", ItemText = "Tất cả"},
                                         new  { ItemValue = "today", ItemText = "Hôm nay"},
                                         new  { ItemValue = "yesterday", ItemText = "Hôm qua"},
                                         new  { ItemValue = "Lastweek", ItemText = "7 ngày trước"},
                                         new  { ItemValue = "LastMonth", ItemText = "Tháng này"},
                                    };
        }
        public static List<object> FillterTime()
        {
            return new List<object> {
                                         new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Hôm nay"},
                                         new  { ItemValue = "2", ItemText = "Hôm qua"},
                                         new  { ItemValue = "3", ItemText = "7 ngày trước"},
                                         new  { ItemValue = "4", ItemText = "Tháng này"},
                                    };
        }

        public static List<object> StatusCommand()
        {
            return new List<object> {
                                         new  { ItemValue = "-1", ItemText = "-- Tất cả --"},
                                         new  { ItemValue = "0", ItemText = "Chờ lệnh"},
                                         new  { ItemValue = "1", ItemText = "Đủ điều kiện"},
                                         new  { ItemValue = "2", ItemText = "Không đủ điều kiện"}
                                    };
        }
        public static List<SelectListModel> StatusCommandModel()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "0", ItemText = "Chờ lệnh"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Đủ điều kiện"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Không đủ điều kiện"},
                                         new  SelectListModel{ ItemValue = "101", ItemText = "Lập lệnh xe đã ra"}
                                    };
        }

        public static List<SelectListModel> ServiceType()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Tính theo ghế nằm"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Tính theo ghế ngồi"},
                                         new  SelectListModel{ ItemValue = "3", ItemText = "Tính theo lượt"},
                                         new  SelectListModel{ ItemValue = "4", ItemText = "Tính theo tháng"}
                                    };
        }

        public static List<SelectListModel> AlertTimeOut()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Ra sớm"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Ra muộn"},
                                         new  SelectListModel{ ItemValue = "3", ItemText = "Đúng giờ"}
                                    };
        }

        public static List<SelectListModel> PageSize()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "10", ItemText = "10"},
                                         new  SelectListModel{ ItemValue = "20", ItemText = "20"},
                                         new  SelectListModel{ ItemValue = "50", ItemText = "50"},
                                         new  SelectListModel{ ItemValue = "100", ItemText = "100"},
                                         new  SelectListModel{ ItemValue = "200", ItemText = "200"},
                                         new  SelectListModel{ ItemValue = "500", ItemText = "500"},
                                         new  SelectListModel{ ItemValue = "1000", ItemText = "1000"},
                                         new  SelectListModel{ ItemValue = "1500", ItemText = "1500"},
                                         new  SelectListModel{ ItemValue = "2000", ItemText = "2000"},
                                    };
        }

        public static List<SelectListModel> RealCommandStatus()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Ra sớm so với dk"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Ra muộn so với dk"},
                                         new  SelectListModel{ ItemValue = "3", ItemText = "Bình thường"}
                                    };
        }

        public static List<SelectListModel> ListNoteLock()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "mat the", ItemText = "Mất thẻ"},
                                         new  SelectListModel{ ItemValue = "hong the", ItemText = "Hỏng thẻ"},
                                         new  SelectListModel{ ItemValue = "chua dong phi", ItemText = "Chưa đóng phí"},
                                         new  SelectListModel{ ItemValue = "doi the", ItemText = "Đổi thẻ"}
                                    };
        }
        public static List<SelectListModel> ListNoteUnLock()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "tim thay the", ItemText = "Tìm thấy thẻ"},
                                         new  SelectListModel{ ItemValue = "da dong phi", ItemText = "Đã đóng phí"}

                                    };
        }

        public static List<object> Communication()
        {
            return new List<object>
            {
                new {ItemValue = "0", ItemText = "RS232/485/422"},
                new {ItemValue = "1", ItemText = "TCP/IP"}
            };
        }


        public static List<object> ReaderTypes()
        {
            return new List<object>
            {
                new {ItemValue = "0", ItemText = "1"},
                new {ItemValue = "1", ItemText = "2"}
            };
        }

        //public static List<statusOrderModel> StatusOrder()
        //{
        //    return new List<statusOrderModel> {
        //                                 new  statusOrderModel{ ItemValue = "0", ItemText = "-- Lựa chọn trạng thái hóa đơn --"},
        //                                 new  statusOrderModel{ ItemValue = "1", ItemText = "Chưa xác nhận"},
        //                                 new  statusOrderModel{ ItemValue = "2", ItemText = "Đã xác nhận"},
        //                                 new  statusOrderModel{ ItemValue = "3", ItemText = "Hủy đơn"},
        //                                 new  statusOrderModel{ ItemValue = "4", ItemText = "Hoãn đơn"},
        //                                 new  statusOrderModel{ ItemValue = "5", ItemText = "Hoàn thành"}
        //                            };
        //}
        public static string GetNameByLedType(int? LeadType)
        {
            string Name = "";
            switch (LeadType)
            {
                case 1:
                    Name = "DSP840";
                    break;
                case 2:
                    Name = "Futech";
                    break;
                case 3:
                    Name = "FINGERTECH";
                    break;
                default:
                    Name = "";
                    break;

            }
            return Name;
        }

        public static string GetNameTicketType(int? TicketType)
        {
            string Name = "";
            switch (TicketType)
            {
                case 1:
                    Name = "Vé bán";
                    break;
                case 2:
                    Name = "Vé trả lại";
                    break;
                case 3:
                    Name = "Vé hủy";
                    break;
                default:
                    Name = "";
                    break;

            }
            return Name;
        }
        public static string GetNameByStatus(string Name)
        {
            string str = "";
            switch (Name)
            {
                case "Home":
                    str = "Trang chủ";
                    break;

                case "Category":
                    str = "Trang Danh mục";
                    break;

                case "ProductDetail":
                    str = "Trang chi tiết";
                    break;

                case "Promotion":
                    str = "Trang khuyến mại";
                    break;

                case "Search":
                    str = "Trang tìm kiếm";
                    break;

                case "Provider":
                    str = "Trang nhà cung cấp";
                    break;

                case "Top":
                    str = "Trên";
                    break;

                case "Left":
                    str = "Trái";
                    break;

                case "Right":
                    str = "Phải";
                    break;

                case "Bottom":
                    str = "Dưới";
                    break;

                case "Center":
                    str = "Giữa";
                    break;

                case "Slide":
                    str = "Slide";
                    break;

                case "run":
                    str = "Hoạt động";
                    break;

                case "pause":
                    str = "Tạm dừng";
                    break;

                case "stop":
                    str = "Hết hạn";
                    break;

                case "choduyet":
                    str = "Chờ duyệt";
                    break;

                case "kichhoat":
                    str = "Kích hoạt";
                    break;

                case "color":
                    str = "mầu sắc";
                    break;

                case "size":
                    str = "Kích thước";
                    break;

                case "xuly":
                    str = "Đang xử lý";
                    break;

                case "finish":
                    str = "Đã xong";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

        public static string GenTextStatus(DateTime beginDate, DateTime endDate)
        {
            string str = string.Empty;

            if (DateTime.Now.Date >= beginDate.Date && DateTime.Now <= endDate.Date)
            {
                str = "Hoạt động";
            }
            else
            {
                str = "<span class='textStatusRed'>Hết thời gian</span>";
            }
            return str;
        }

        public static string SplitContent(string value, int num)
        {
            string result = string.Empty;
            int start = 0;
            if (value.Length > num)
            {
                result = value.Substring(0, num);
                start = result.LastIndexOf(' ') - 1;
                result = result.Substring(0, start + 1);
            }
            else
            {
                result = value;
            }
            return result;
        }

        public static List<object> TimeApp()
        {
            var list = new List<object> {
                                         new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "7", ItemText = "7 ngày"},
                                         new  { ItemValue = "14", ItemText = "14 ngày"},
                                         new  { ItemValue = "30", ItemText = "30 ngày"},
                                         new  { ItemValue = "60", ItemText = "60 ngày"},
                                         new  { ItemValue = "90", ItemText = "90 ngày"},
                                         new  { ItemValue = "120", ItemText = "120 ngày"},
                                         new  { ItemValue = "150", ItemText = "150 ngày"}
                                    };
            return list;
        }

        public static List<object> MenuOptions()
        {
            return new List<object> {
                                         new  { ItemValue = "0", ItemText = "Menu"},
                                         new  { ItemValue = "1", ItemText = "Function"},
                                    };
        }

        public static List<object> CameraTypes()
        {
            return new List<object> {
                                         new  { ItemValue = "", ItemText = "-- Lựa chọn loại"},
                                         new  { ItemValue = "Geovision", ItemText = "Geovision"},
                                         new  { ItemValue = "Panasonic i-Pro", ItemText = "Panasonic i-Pro"},
                                         new  { ItemValue = "Axis", ItemText = "Axis"},
                                         new  { ItemValue = "Secus", ItemText = "Secus"},
                                         new  { ItemValue = "Shany-Stream1", ItemText = "Shany-Stream1"},
                                         new  { ItemValue = "Shany-Stream21", ItemText = "Shany-Stream2"},
                                         new  { ItemValue = "Vivotek", ItemText = "Vivotek"},
                                         new  { ItemValue = "Lilin", ItemText = "Lilin"},
                                         new  { ItemValue = "Messoa", ItemText = "Messoa"},
                                         new  { ItemValue = "Entrovision", ItemText = "Entrovision"},
                                         new  { ItemValue = "Sony", ItemText = "Sony"},
                                         new  { ItemValue = "Bosch", ItemText = "Bosch"},
                                         new  { ItemValue = "Vantech", ItemText = "Vantech"},
                                         new  { ItemValue = "SC330", ItemText = "SC330"},
                                         new  { ItemValue = "SecusFFMPEG", ItemText = "SecusFFMPEG"},
                                         new  { ItemValue = "CNB", ItemText = "CNB"},//"CNB", "HIK", "Enster", "Afidus", "Dahua", "ITX"
                                         new  { ItemValue = "HIK", ItemText = "HIK"},
                                         new  { ItemValue = "Enster", ItemText = "Enster"},
                                         new  { ItemValue = "Afidus", ItemText = "Afidus"},
                                         new  { ItemValue = "Dahua", ItemText = "Dahua"},
                                         new  { ItemValue = "ITX", ItemText = "ITX"},
                                         new  { ItemValue = "Hanse", ItemText = "Hanse"}

                                    };
        }

        public static List<object> StreamTypes()
        {
            return new List<object> {
                                         new  { ItemValue = "", ItemText = "-- Chọn loại"},
                                         new  { ItemValue = "MJPEG", ItemText = "MJPEG"},
                                         new  { ItemValue = "PlayFile", ItemText = "PlayFile"},
                                         new  { ItemValue = "Local Video Capture Device", ItemText = "Local Video Capture Device"},
                                         new  { ItemValue = "JPEG", ItemText = "JPEG"},
                                         new  { ItemValue = "MPEG4", ItemText = "MPEG4"},
                                         new  { ItemValue = "H264", ItemText = "H264"},
                                         new  { ItemValue = "Onvif", ItemText = "Onvif"}
                                    };
        }

        public static List<object> SDKs()
        {
            return new List<object>
            {
                new  { ItemValue = "", ItemText = "-- Chọn loại"},
                new  { ItemValue = "AForgeSDK", ItemText = "AForgeSDK"},
                new  { ItemValue = "AxisSDK", ItemText = "AxisSDK"},
                new  { ItemValue = "GeoSDK", ItemText = "GeoSDK"},
                new  { ItemValue = "ScSDK", ItemText = "ScSDK"},
                new  { ItemValue = "FFMPEG", ItemText = "FFMPEG"},
            };
        }

        public static List<object> LineTypes()
        {
            return new List<object>
            {
                new  { ItemValue = "", ItemText = "-- Chọn loại"},
                new  { ItemValue = "0", ItemText = "IDTECK"},
                new  { ItemValue = "1", ItemText = "Honeywell SY-MSA30/60L"},
                new  { ItemValue = "2", ItemText = "Honeywell Nstar"},
                new  { ItemValue = "3", ItemText = "Pegasus PP-3760"},
                new  { ItemValue = "4", ItemText = "Pegasus PP-6750"},
                new  { ItemValue = "5", ItemText = "Pegasus PFP-3700"},
                new  { ItemValue = "6", ItemText = "FINGERTEC"},
                new  { ItemValue = "7", ItemText = "DS3000"},
                new  { ItemValue = "8", ItemText = "CS3000"},
                new  { ItemValue = "9", ItemText = "RCP4000"},
                new  { ItemValue = "10", ItemText = "PEGASUS PB7/PT3"},
                new  { ItemValue = "11", ItemText = "PEGASUS PB5"},
                new  { ItemValue = "12", ItemText = "IDTECK (006)"},
                new  { ItemValue = "13", ItemText = "IDTECK (iTDC)"},
                new  { ItemValue = "14", ItemText = "IDTECK (iMDC)"},
                new  { ItemValue = "15", ItemText = "IDTECK (Elevator384)"},
                new  { ItemValue = "16", ItemText = "Promax - FAT810W Kanteen"},
                new  { ItemValue = "17", ItemText = "Promax - AC908"},
                new  { ItemValue = "18", ItemText = "HAEIN S&amp;S"},
                new  { ItemValue = "19", ItemText = "Promax - PCR310U"},
                new  { ItemValue = "20", ItemText = "NetPOS Client MDB"},
                new  { ItemValue = "21", ItemText = "NetPOS Client SERVER"},
                new  { ItemValue = "22", ItemText = "Promax - FAT810W Parking"},
                new  { ItemValue = "23", ItemText = "Promax - FAT810W Vending Machine"},
                new  { ItemValue = "24", ItemText = "Pegasus - PP-110/PP-5210/PUA-310"},
                new  { ItemValue = "25", ItemText = "Futech SC100"},
                new  { ItemValue = "26", ItemText = "Honeywell HSR900"},
                new  { ItemValue = "27", ItemText = "AC9xxPCR"},
                new  { ItemValue = "28", ItemText = "E02.NET"},
                new  { ItemValue = "29", ItemText = "Futech SC101"},
                new  { ItemValue = "30", ItemText = "Futech SC100FPT"},
                new  { ItemValue = "31", ItemText = "Futech SC100LANCASTER"},
                new  { ItemValue = "32", ItemText = "Futech FUCM100"},
                new  { ItemValue = "33", ItemText = "Futech SC101"},
                new  { ItemValue = "34", ItemText = "E01 RS485"},
                new  { ItemValue = "35", ItemText = "E02.NET Card Int"},
                new  { ItemValue = "36", ItemText = "FUPC100"},
                new  { ItemValue = "37", ItemText = "E02.NET Mifare"},
                new  { ItemValue = "38", ItemText = "SOYAL"},
                new  { ItemValue = "39", ItemText = "E02.NET Mifare SR30"},
            };
        }

        public static List<object> LaneTypes()
        {
            return new List<object>
            {
                new  { ItemValue = "", ItemText = "-- Chọn loại"},
                new  { ItemValue = "0", ItemText = "0. Vào"},
                new  { ItemValue = "1", ItemText = "1. Ra"}
                //new  { ItemValue = "2", ItemText = "2. Vào-Ra"},
                //new  { ItemValue = "3", ItemText = "3. Vào-Vào"},
                //new  { ItemValue = "4", ItemText = "4. Ra-Ra"},
                //new  { ItemValue = "5", ItemText = "5. Vào-Ra 2"},
            };
        }

        public static List<object> CheckPlateLevelOuts()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CheckPlateLevelOuts");
            return new List<object>
            {
                new  { ItemValue = "1", ItemText = Dictionary["4char"]},
                new  { ItemValue = "2", ItemText = Dictionary["all"]},
                new  { ItemValue = "0", ItemText = Dictionary["noCheck"]}
            };
        }

        public static List<SelectListModel> VehicleGroup()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "2", ItemText = "Xe 2 chỗ"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Xe 4 chỗ"},
                                         new SelectListModel { ItemValue = "7", ItemText = "Xe 7 chỗ"},
                                         new SelectListModel { ItemValue = "9", ItemText = "Xe 9 chỗ"},
                                         new SelectListModel { ItemValue = "16", ItemText = "Xe 16 chỗ"},
                                         new SelectListModel { ItemValue = "24", ItemText = "Xe 24 chỗ"},
                                         new SelectListModel { ItemValue = "30", ItemText = "Xe 30 chỗ"},
                                         new SelectListModel { ItemValue = "36", ItemText = "Xe 36 chỗ"},
                                         new SelectListModel { ItemValue = "45", ItemText = "Xe 45 chỗ"},
                                    };
            return list;
        }

        /// <summary>
        /// Cấm sửa List này.
        /// </summary>
        /// <returns></returns>
        public static List<SelectListModel> VehicleTypes()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Xe hợp đồng"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe thay thế"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Xe tăng cường"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Xe đình tài"},
                                         new SelectListModel { ItemValue = "5", ItemText = "Xe đình nốt"},
                                         new SelectListModel { ItemValue = "6", ItemText = "Xe bus vào bến, không hợp đồng"},
                                         new SelectListModel { ItemValue = "7", ItemText = "Xe khách vào bến, không hợp đồng"},
                                         new SelectListModel { ItemValue = "8", ItemText = "Xe taxi vào bến, không hợp đồng"}
                                    };
            return list;
        }

        public static List<SelectListModel> VehicleTypes1()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = "-- Lựa chọn"},
                                         new SelectListModel { ItemValue = "-1", ItemText = "Xe chưa đăng ký"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Xe hợp đồng"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe thay thế"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Xe tăng cường"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Xe đình tài"},
                                         new SelectListModel { ItemValue = "5", ItemText = "Xe đình nốt"},
                                         new SelectListModel { ItemValue = "6", ItemText = "Xe bus vào bến, không hợp đồng"},
                                         new SelectListModel { ItemValue = "7", ItemText = "Xe khách vào bến, không hợp đồng"},
                                         new SelectListModel { ItemValue = "8", ItemText = "Xe taxi vào bến, không hợp đồng"}
                                    };
            return list;
        }

        public static List<SelectListModel> VehicleContractType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Xe buýt"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe cố định"},
                                    };
            return list;
        }
        public static List<SelectListModel> EventCodeStstus()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "001", ItemText = "Sai quyền truy cập"},
                                         new SelectListModel { ItemValue = "002", ItemText = "Xe đã vào bãi"},
                                         new SelectListModel { ItemValue = "003", ItemText = "Xe chưa vào bãi"},
                                         new SelectListModel { ItemValue = "004", ItemText = "Mở barrie bằng máy tính"},
                                         new SelectListModel { ItemValue = "005", ItemText = "Mở barrie bằng nút ấn"},
                                         new SelectListModel { ItemValue = "006", ItemText = "Sự kiện escape vào"},
                                         new SelectListModel { ItemValue = "007", ItemText = "Sự kiện escape ra"},
                                         new SelectListModel { ItemValue = "008", ItemText = "Biển số không hợp lệ"},
                                         new SelectListModel { ItemValue = "009", ItemText = "Danh sách đen"},
                                         new SelectListModel { ItemValue = "010", ItemText = "Biển số bị khóa"},
                                         new SelectListModel { ItemValue = "010", ItemText = "Biển số hết hạn sử dụng"},
                                         new SelectListModel { ItemValue = "012", ItemText = "Biển số không khớp"},
                                    };
            return list;
        }
        public static List<SelectListModel> OptionAutoCapture()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Cập nhật danh sách thẻ đã tích chọn"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Cập nhật danh sách thẻ đã tìm kiếm"},
                                    };
            return list;
        }


        public static List<SelectListModel> EventCode()
        {
            var list = new List<SelectListModel> {
                                            new SelectListModel { ItemValue = "1", ItemText = "Xe vào"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe ra"}
                                    };
            return list;
        }
        public static List<SelectListModel> PayState()
        {
            var list = new List<SelectListModel> {
                 new SelectListModel { ItemValue = "", ItemText = "--Tất cả--" },
                                         new SelectListModel { ItemValue = "0", ItemText = "Chưa thanh toán" },
                                         new SelectListModel { ItemValue = "1", ItemText = "Đã thanh toán"}
                                    };
            return list;
        }
        public static List<SelectListModel> EventCode1()
        {
            var list = new List<SelectListModel> {
                new SelectListModel { ItemValue = "", ItemText = "-- Chọn sự kiện --"},
                                            new SelectListModel { ItemValue = "1", ItemText = "Xe vào"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe ra"}
                                    };
            return list;
        }
        public static List<SelectListModel> ActivityTypes()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Đủ điều kiện xuất bến"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Không đủ điều kiện xuất bến"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Xe tạm giữ"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Xe về đơn vị"},
                                         new SelectListModel { ItemValue = "5", ItemText = "Xe về bảo dưỡng"}
                                    };
            return list;
        }

        public static List<SelectListModel> DayOfWeek()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "Mon", ItemText = "Thứ 2"},
                                         new SelectListModel { ItemValue = "Tue", ItemText = "Thứ 3"},
                                         new SelectListModel { ItemValue = "Wed", ItemText = "Thứ 4"},
                                         new SelectListModel { ItemValue = "Thu", ItemText = "Thứ 5"},
                                         new SelectListModel { ItemValue = "Fri", ItemText = "Thứ 6"},
                                         new SelectListModel { ItemValue = "Sat", ItemText = "Thứ 7"},
                                         new SelectListModel { ItemValue = "Sun", ItemText = "Chủ nhật"},
                                    };
            return list;
        }

        public static List<SelectListModel> RunType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Theo lượt"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Theo tháng"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Theo quý"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Theo năm"}
                                    };
            return list;
        }

        public static List<object> PayStatus2()
        {
            var list = new List<object> {
                                         new  { ItemValue = "true", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "false", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }

        public static List<SelectListModel> VehicleType2()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Gia hạn ngày hoạt động"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Xe đình tài"},
                                         new SelectListModel { ItemValue = "5", ItemText = "Xe đình nốt"}
                                    };
            return list;
        }

        public static List<SelectListModel> CardChipTypes()
        {
            var list = new List<SelectListModel>
            {
                new SelectListModel{ItemValue = "", ItemText = "--Lựa chọn--" },
            };

            list.AddRange(CardChipTypes_CreateUpdate());

            return list;
        }

        public static List<SelectListModel> CardChipTypes_CreateUpdate()
        {
            var list = new List<SelectListModel>
            {
                new SelectListModel{ItemValue = "0", ItemText = "Proxi-C1" },
                new SelectListModel{ItemValue = "1", ItemText = "Proxi-C2" },
                new SelectListModel{ItemValue = "2", ItemText = "Mifare" },
            };

            return list;
        }


        /// <summary>
        /// Lay danh sach tinh thanh quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        //public static List<DistrictModel> GetProvinceList()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/Country.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  where int.Parse(region.Element("ParentID").Value) == 0
        //                  select new
        //                  {
        //                      TinhThanhID = region.Element("TinhThanhID").Value,
        //                      Name = region.Element("Name").Value,
        //                      ParentID = region.Element("ParentID").Value,
        //                  };

        //    var tinhThanhList = new List<DistrictModel>();

        //    foreach (var region in regions)
        //    {
        //        tinhThanhList.Add(new DistrictModel()
        //        {
        //            DistrictId = region.TinhThanhID,
        //            DistrictName = region.Name
        //        });
        //    }
        //    return tinhThanhList;
        //}

        /// <summary>
        /// Lay danh sach quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        //public static List<DistrictModel> GetDistrictList(int tinhThanhId)
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/Country.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  where int.Parse(region.Element("ParentID").Value) == tinhThanhId
        //                  select new
        //                  {
        //                      TinhThanhID = region.Element("TinhThanhID").Value,
        //                      Name = region.Element("Name").Value,
        //                      ParentID = region.Element("ParentID").Value,
        //                  };

        //    var quanHuyenList = new List<DistrictModel>();

        //    foreach (var region in regions)
        //    {
        //        quanHuyenList.Add(new DistrictModel()
        //        {
        //            DistrictId = region.TinhThanhID,
        //            DistrictName = region.Name
        //        });
        //    }
        //    return quanHuyenList;
        //}

        //public static List<CountryModel> GetCountry()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/country.xml"));

        //    var regions = from region in xmlDoc.Descendants("country")
        //                  select new
        //                  {
        //                      CountryId = region.Element("country_id").Value,
        //                      CountryName = region.Element("name").Value,
        //                      CountryCode = region.Element("iso_code").Value,
        //                  };

        //    var CountryList = new List<CountryModel>();

        //    foreach (var region in regions)
        //    {
        //        CountryList.Add(new CountryModel()
        //        {
        //            CountryId = region.CountryId,
        //            CountryName = region.CountryName,
        //            CountryCode = region.CountryCode
        //        });
        //    }
        //    return CountryList;
        //}

        //public static List<StateModel> GetStateByCountry(string countryId)
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/state.xml"));

        //    var regions = from region in xmlDoc.Descendants("state_province")
        //                  where Convert.ToString(region.Element("country_id").Value).Equals(countryId)
        //                  select new
        //                  {
        //                      StateAbbreviation = region.Element("abbreviation").Value,
        //                      StateName = region.Element("name").Value,
        //                      CountryId = region.Element("country_id").Value,
        //                  };

        //    var StateList = new List<StateModel>();

        //    foreach (var region in regions)
        //    {
        //        StateList.Add(new StateModel()
        //        {
        //            StateAbbreviation = region.StateAbbreviation,
        //            StateName = region.StateName,
        //            CountryId = region.CountryId
        //        });
        //    }
        //    return StateList;
        //}

        //public static List<StateModel> GetState()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/state.xml"));

        //    var regions = from region in xmlDoc.Descendants("state_province")
        //                  select new
        //                  {
        //                      StateAbbreviation = region.Element("abbreviation").Value,
        //                      StateName = region.Element("name").Value,
        //                      CountryId = region.Element("country_id").Value,
        //                  };

        //    var StateList = new List<StateModel>();

        //    foreach (var region in regions)
        //    {
        //        StateList.Add(new StateModel()
        //        {
        //            StateAbbreviation = region.StateAbbreviation,
        //            StateName = region.StateName,
        //            CountryId = region.CountryId
        //        });
        //    }
        //    return StateList;
        //}

        public static bool emailIsValid(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //public static bool SendMailRegisterSucccess(string cusName, string cusMail, ConfigSys info)
        //{
        //    try
        //    {
        //        var mailTo = cusMail;
        //        var mailCC = info.EmailCc; //System.Configuration.ConfigurationManager.AppSettings["CCOrderMail"] + ";" + System.Configuration.ConfigurationManager.AppSettings["ReceiveOrderMail"];
        //        var subject = "Kích hoạt tài khoản mới đăng ký " + HttpContext.Current.Request.Url.Host;

        //        //reading email template
        //        var content = ReadMailTemplate("~/Templates/Email/RegisterMailTemplate.xml");
        //        string emailContent = string.Format(content, cusName, cusMail);
        //        //SmtpEmailSender.SendSystem(mailFrom, mailTo, mailCC, subject, emailContent);
        //        return SmtpEmailSender.Send(mailTo, mailCC, subject, emailContent, info);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Doc file xml tra ve du lieu cua cot chi dinh
        /// </summary>
        /// <param name="path"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        //public static List<HtmlStatic> ReadXmlHtmlStatics()
        //{
        //    var xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Areas/Admin/htmlStatic.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  //where int.Parse(region.Element("ParentID").Value) == 0
        //                  select new HtmlStatic
        //                  {
        //                      Id = region.Element("Id").Value,
        //                      Title = region.Element("Title").Value,
        //                      Desc = region.Element("Desc").Value,
        //                      type = region.Element("Type").Value
        //                  };
        //    return regions.ToList();
        //}
        //public static HtmlStatic ReadXmlHtmlStaticsById(string id)
        //{
        //    var xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Areas/Admin/htmlStatic.xml"));

        //    var htmlStatics = from htmlStatic in xmlDoc.Descendants("Rows")
        //                      where htmlStatic.Element("Id").Value == id
        //                      orderby htmlStatic.Element("Type").Value ascending
        //                      select new HtmlStatic
        //                      {
        //                          Id = htmlStatic.Element("Id").Value,
        //                          Title = htmlStatic.Element("Title").Value,
        //                          Desc = htmlStatic.Element("Desc").Value,
        //                          type = htmlStatic.Element("Type").Value
        //                      };
        //    return htmlStatics.FirstOrDefault();
        //}

        /// <summary>
        /// Ham thay the tham so Url dung cho bo loc
        /// </summary>
        /// <param name="path">Url hien tai dang day du tham so</param>
        /// <param name="paramName">ten tham so</param>
        /// <param name="addOrRemove">them hay xoa tham so khoi Url hien tai - True: them moi, False: xoa di</param>
        /// <returns>do-so-sinh?price=</returns>
        public static string addUrlFilter(string path, string paramName, bool addOrRemove)
        {
            var _url = "";
            var arrUrl = path.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            // neu co tham so khac
            if (arrUrl.Length > 1)
            {
                //Replace('?', '&');
                if (arrUrl.Length > 2)
                    for (var i = 2; i <= arrUrl.Length - 1; i++)
                    {
                        arrUrl[1] += "&" + arrUrl[i];
                    }
                var urlParam = arrUrl[1].Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                if (urlParam.Length <= 1) // co duy nhat 1 tham so
                {
                    // neu tham so duy nhat nay giong voi tham so can thay the thi giu nguyen ten nhung bo di gia tri cua tham so
                    if (urlParam[0].Contains(paramName))
                    {
                        if (addOrRemove)
                            _url = arrUrl[0] + "?" + paramName + "=";
                        else
                            _url = arrUrl[0];
                    }
                    else
                    {
                        if (addOrRemove)
                            _url = arrUrl[0] + "?" + arrUrl[1] + "&" + paramName + "=";
                        else
                            _url = arrUrl[0] + "?" + arrUrl[1];
                    }
                }
                else // neu co nhieu hon 1 tham so
                {
                    _url = arrUrl[0] + "?";
                    // kiem tra xem tham so co chua
                    //_url = urlParam.Where(s => !s.Contains(paramName)).Aggregate(_url, (current, s) => current + (s + "&"));
                    foreach (var s in urlParam)
                    {
                        // neu tham so da co san
                        if (!s.Contains(paramName) && s.Contains("="))
                            _url += s + "&";
                    }
                    // them moi param hay xoa di
                    if (addOrRemove)
                        _url += paramName + "=";
                    else
                        _url = _url.Substring(0, _url.Length - 1);
                }
            }
            else // neu khong co tham so nao
            {
                if (addOrRemove)
                    _url = arrUrl[0] + "?" + paramName + "=";
                else
                    _url = arrUrl[0];
            }
            return _url;
        }

        //public static string CovertImgtoBase64(string imagepath)
        //{
        //    try
        //    {
        //        using (Image image = Image.FromFile(imagepath))
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                image.Save(m, image.RawFormat);
        //                byte[] imageBytes = m.ToArray();
        //                string base64String = Convert.ToBase64String(imageBytes);
        //                return base64String;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //        throw ex;
        //    }
        //}

        public static string ConvertByteToBase64(Byte[] bytes)
        {
            string ImageUrl = "";

            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            ImageUrl = "data:image/png;base64," + base64String;

            return ImageUrl;
        }

        public static string ConvertImgFileUploadToBase64(HttpPostedFileBase FilesUpload)
        {
            string ImageUrl = "";

            Byte[] bytes = CovertFileToByte(FilesUpload);

            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            ImageUrl = "data:image/png;base64," + base64String;

            return ImageUrl;
        }

        public static byte[] CovertFileToByte(HttpPostedFileBase FilesUpload)
        {
            BinaryReader br = new BinaryReader(FilesUpload.InputStream);
            byte[] buffer = br.ReadBytes(FilesUpload.ContentLength);

            return buffer;
        }

        public static string GetRegisterFromUrl(string url)
        {
            string strRegister = string.Empty;
            string[] strPath = url.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (strPath.Any())
            {
                strRegister = strPath[0].ToString();
            }
            return strRegister;
        }

        /// <summary>
        /// ex: Thứ hai, 6/2/2017 | 16:10
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public static string showDayVN(DateTime _dt)
        {
            var strRtn = "{0}, {1} | {2}";
            var thu = "";
            if (_dt != null || _dt != DateTime.MinValue)
            {
                //Friday = 5,
                //Monday = 1,
                //Saturday = 6,
                //Sunday = 0,
                //Thursday = 4,
                //Tuesday = 2,
                //Wednesday = 3
                switch (Convert.ToInt32(_dt.DayOfWeek))
                {
                    case 0:
                        thu = "Chủ nhật";
                        break;

                    case 1:
                        thu = "Thứ hai";
                        break;

                    case 2:
                        thu = "Thứ ba";
                        break;

                    case 3:
                        thu = "Thứ tư";
                        break;

                    case 4:
                        thu = "Thứ năm";
                        break;

                    case 5:
                        thu = "Thứ sáu";
                        break;

                    case 6:
                        thu = "Thứ bẩy";
                        break;

                    default:
                        thu = "Chủ nhật";
                        break;
                }
            }

            return string.Format(strRtn, thu, _dt.ToString("dd/MM/yyyy"), _dt.ToString("hh:mm"));
        }


        //public static WebConfig getWebConfig()
        //{
        //    var model = (WebConfig)HttpContext.Current.Session[SessionConfig.WebInfoSession];

        //    if (model == null)
        //    {
        //        using (var db = new KztekEntities())
        //        {
        //            model = db.WebConfigs.FirstOrDefault();
        //            if (model != null)
        //            {
        //                HttpContext.Current.Session[SessionConfig.WebInfoSession] = model;
        //            }
        //            else
        //            {
        //                model = new WebConfig();
        //            }
        //        }
        //    }
        //    return model;
        //}

        //public static WebConfig getWebConfigFromCache()
        //{
        //    var model = new WebConfig();

        //    if (CacheLayer.Exists(ConstField.WebConfigObj))
        //    {
        //        model = CacheLayer.Get<WebConfig>(ConstField.WebConfigObj);
        //    }
        //    else
        //    {
        //        using (var db = new KztekEntities(@"Data Source=FUTECH-PC;Initial Catalog=DBStation;User Id=sa;Password=123456;"))
        //        {
        //            model = db.WebConfigs.FirstOrDefault();
        //        }
        //        CacheLayer.Add(ConstField.WebConfigObj, model, ConstField.TimeCache);
        //    }

        //    return model;
        //}

        public static void ClearCache(string name)
        {
            CacheLayer.Clear(name);
        }

        public static void ClearCache(string constField, string objId)
        {
            var formatRole = string.Format("{0}_{1}", constField, objId);
            CacheLayer.Clear(formatRole);
        }

        public static string GetCgiByCameraType(string CameraType, string FrameRate, string Resolution, string SDK, string Username = "", string Password = "")
        {
            string Cgi = "";
            switch (CameraType)
            {
                case "Panasonic i-Pro":
                    Cgi = "/cgi-bin/mjpeg?framerate=" + FrameRate + "&resolution=" + Resolution;
                    break;
                case "Axis":
                    Cgi = "/axis-cgi/mjpg/video.cgi?resolution = " + Resolution;
                    break;
                case "Sony":
                    Cgi = "/image";
                    break;
                case "Shany-Stream1":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Shany";
                        break;
                    }
                    Cgi = "/live/stream1.cgi";
                    break;
                case "Shany-Stream2":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Shany";
                        break;
                    }
                    Cgi = "/live/stream2.cgi";
                    break;
                case "Secus":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/stream1";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "/cgi-bin/image/mjpeg.cgi";

                    break;
                case "Bosch":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/rtsp_tunnel";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "/rtsp_tunnel?resolution=" + Resolution;
                        break;
                    }

                    Cgi = "/snap.jpg?";
                    break;
                case "Vantech":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Vantech";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Vantech?resolution=" + Resolution;
                    }
                    break;
                case "SecusFFMPEG":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "Secus";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "Secus";
                    break;
                case "CNB":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "CNB";
                        break;
                    }
                    Cgi = "CNB";
                    break;
                case "HIK":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                        Cgi = "HIK";
                    Cgi = "/api/mjpegvideo.cgi?InputNumber=1&StreamNumber=1";
                    break;
                case "Enster":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                        Cgi = "Enster";
                    break;
                case "Afidus":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Afidus";
                        break;
                    }
                    Cgi = "/cgi-bin/jpg/image.cgi";
                    break;
                case "Dahua":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Dahua";
                        break;
                    }

                    break;
                case "ITX":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "ITX";
                        break;
                    }
                    break;

                case "Hanse":
                    if (SDK == "FFMPEG" || SDK == "KztekSDK")
                    {
                        Cgi = "/stream1";
                        break;
                    }
                    else if (SDK == "VLC")
                    {
                        Cgi = "Secus?resolution=" + Resolution;
                        break;
                    }
                    Cgi = "/cgi-bin/image/mjpeg.cgi?" + "id=" + Username + "&passwd=" + Password;

                    break;
                case "Samsung":
                    if (SDK == "FFMPEG" || SDK == "VLC" || SDK == "KztekSDK")
                    {
                        Cgi = "Samsung";
                        break;
                    }
                    break;
                case "Camtron":
                    if (SDK == "KztekSDK")
                    {
                        Cgi = "Camtron";
                        break;
                    }
                    break;

                default:

                    break;
            }
            return Cgi;
        }

        public static bool CheckInt(string input, ref int num)
        {
            int numArc;

            bool isInt = int.TryParse(input, out numArc);
            if (isInt)
            {
                num = numArc;
            }
            else
            {
                num = 0;
            }

            return isInt;
        }

        public static bool CheckFloat(string input, ref float num)
        {
            float numArc;

            bool isFloat = float.TryParse(input, out numArc);
            if (isFloat)
            {
                num = numArc;
            }
            else
            {
                num = 0;
            }

            return isFloat;
        }

        public static int GetTotalMonthsFrom(DateTime dt1, DateTime dt2)
        {
            DateTime earlyDate = (dt1 > dt2) ? dt2.Date : dt1.Date;
            DateTime lateDate = (dt1 > dt2) ? dt1.Date : dt2.Date;

            // Start with 1 month's difference and keep incrementing
            // until we overshoot the late date
            int monthsDiff = 1;
            while (earlyDate.AddMonths(monthsDiff) <= lateDate)
            {
                monthsDiff++;
            }

            return monthsDiff;
        }




        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }

        public static string GetDateTimeExpire(DateTime dateFrom, DateTime dateTo)
        {
            string result = "";

            var dateTimeNow = DateTime.Today;
            if (dateFrom <= dateTimeNow)
            {
                if (dateTo >= dateTimeNow)
                {
                    result = "<span class='label label-success label-white middle'><i class='ace-icon fa fa-check-circle-o'></i>  Còn hạn </span>";
                }
                else
                {
                    result = "<span class='label label-danger label-white middle'><i class='ace-icon fa fa-ban'></i>  Hết hạn </span>";
                }
            }
            else
            {
                result = "<span class='label label-warning label-white middle'><i class='ace-icon fa fa-exclamation-circle'></i>  Chưa đến ngày </span>";
            }

            return result;
        }

        public static string GetDateTimeOut(string timeOutInEvent, string timeOutInCommand, int timeAlert, string dateTimeOut)
        {
            string result = "";

            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;

            //t > 0 là xe ra muộn, < 0 là xe ra sớm
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = string.Format("<span style='color:orange'>{0}</span>", dateTimeOut);
                }
                else
                {
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = string.Format("<span style='color:red'>{0}</span>", dateTimeOut);
                }
                else
                {
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                }
            }

            return result;
        }

        public static string GetTimeOutStatus(string status = "", string dateTimeOut = "")
        {
            string result = "";

            // 0 = mặc định, 1 = ra sớm, 2 = ra muộn
            switch (status)
            {
                case "0":
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
                case "1":
                    result = string.Format("<span style='color:red'>{0}</span>", dateTimeOut);
                    break;
                case "2":
                    result = string.Format("<span style='color:orange'>{0}</span>", dateTimeOut);
                    break;
                case "3":
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
                default:
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
            }

            return result;
        }

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

        public void GetDateTimeOutStatus1(string timeOutInEvent, string timeOutInCommand, int timeAlert, ref string result, ref string status)
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

        /// <summary>
        /// 1 - sớm 30' 2- muộn 10' 3- đúng giờ
        /// </summary>
        /// <param name="realDate"></param>
        /// <param name="dateCommand"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static SelectListModel GetDateTimeCommandStatus(string realDate, string dateCommand, string timeOut)
        {
            var result = new SelectListModel();

            var real = Convert.ToDateTime(realDate);
            var command = Convert.ToDateTime(string.Format("{0} {1}", Convert.ToDateTime(dateCommand).ToString("dd/MM/yyyy"), timeOut));
            var timespan = (command - real).Minutes;

            result.ItemText = "3";
            result.ItemValue = timespan.ToString();

            if (timespan > 0 && timespan >= 30)
            {
                result.ItemText = "1";
            }
            else if (timespan < 0 && -timespan >= 10)
            {
                result.ItemText = "2";
            }

            return result;
        }

        public static string GetDateTimeExpire(DateTime dateTo)
        {
            string result = "";

            var dateTimeNow = DateTime.Today;
            if (dateTo >= dateTimeNow)
            {
                result = "<span class='label label-success label-white middle'><i class='ace-icon fa fa-check-circle-o'></i>  Còn hạn </span>";
            }
            else
            {
                result = "<span class='label label-danger label-white middle'><i class='ace-icon fa fa-ban'></i>  Hết hạn </span>";
            }

            return result;
        }

        //Hien thi mau theo ngay het han
        //public static string GetDateTimeExpireColor(DateTime dateTo)
        //{
        //    string result = "";
        //    var winfo = getWebConfig();
        //    var dateTimeNow = DateTime.Today;
        //    if (dateTo >= dateTimeNow)
        //    {
        //        if(dateTo > dateTimeNow.AddDays(winfo.NumberAlertDay))
        //        {
        //            result = "blue";
        //        }
        //        else
        //        {
        //            result = "orange";
        //        }

        //    }
        //    else
        //    {
        //        //het han: ten class
        //        result = "red strike";
        //    }

        //    return result;
        //}

        public static bool ExecuteChangeColumnType(string tablename, string columnname)
        {
            var str = new StringBuilder();
            str.AppendLine(string.Format("DECLARE @tableName varchar(55) SET @tableName ='{0}'", tablename));
            str.AppendLine(string.Format("DECLARE @colName varchar(55) SET @colName ='{0}'", columnname));
            str.AppendLine("DECLARE @query NVARCHAR(MAX);");
            str.AppendLine(string.Format("SET @query = 'ALTER TABLE {0} DROP CONSTRAINT '+(select name from sys.indexes where object_id = OBJECT_ID(@tableName))", tablename));
            str.AppendLine(string.Format("SET @query = @query + ' ALTER TABLE ' + @tableName + ' ALTER COLUMN {0} nvarchar(128) NOT NULL'", columnname));
            str.AppendLine("SET @query = @query + ' ALTER TABLE ' + @tableName + ' add primary key (' + @colName + ')'");
            str.AppendLine("EXECUTE(@query)");

            var t = ExcuteSQL.Execute(str.ToString());

            return t;
        }

        public static string DirectActionByGroup(string group, ref string groupname)
        {
            string direct = "";

            var objGroup = GroupMenuList().FirstOrDefault(n => n.ItemValue.Equals(group));
            if (objGroup != null)
            {
                //direct = objGroup.ItemCode;
                groupname = objGroup.ItemText;
            }
            else
            {
                direct = "";
                groupname = "Trang chủ";
            }

            return direct;
        }

        public static List<SelectListModel1> GroupMenuList()
        {
            var list = new List<SelectListModel1> {
                                        new SelectListModel1 { ItemValue = "67810176", ItemText = "Dịch vụ tòa nhà", ItemIndex = 1, ItemCode = "BMS_", Icon = "/Content/Image/sy-building-icon.png", Color = "infobox-green", AreaName = "BMS"},
                                         new SelectListModel1 { ItemValue = "98818976", ItemText = "Kiểm soát vào ra", ItemIndex = 2, ItemCode = "AC_", Icon = "/Content/Image/access-control-icon.png", Color = "infobox-grey", AreaName = "Access"},
                                         new SelectListModel1 { ItemValue = "12878956", ItemText = "Bãi xe thông minh", ItemIndex = 3, ItemCode = "PK_", Icon = "/Content/Image/sy-parking-icon.png", Color = "infobox-blue", AreaName = "Parking"},
                                          new SelectListModel1 { ItemValue = "61119719", ItemText = "Tủ đồ thông minh", ItemIndex = 3, ItemCode = "LK_", Icon = "/Content/Image/iconfinder_go_locker_143845.png", Color = "infobox-red", AreaName = "Locker"},
                                          new SelectListModel1 { ItemValue = "75675733", ItemText = "Cư dân", ItemIndex = 5, ItemCode = "RD_", Icon = "/Content/Image/sy-building-icon.png", Color = "infobox-purple", AreaName = "Resident"}
                                    };

            //Ngôn ngữ
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "GroupMenuList");
            foreach (var item in list)
            {
                string _text = item.ItemText;
                Dictionary.TryGetValue(item.ItemValue, out _text);
                item.ItemText = _text;
            }

            return list;
        }

        public static string getCurrentGroup(string group)
        {
            var name = "";

            switch (group)
            {
                case "67810176":
                    break;
                case "98818976":
                    name = ConstField.AccessControlCode;
                    break;
                case "12878956":
                    name = ConstField.ParkingCode;
                    break;
                case "61119719":
                    name = ConstField.LockerCode;
                    break;
                default:
                    break;
            }

            return name;
        }

        public static string ConverControllerInGroup(string controller)
        {
            string value = "";

            var objGroup = GroupMenuList().Where(n => controller.Contains(n.ItemCode)).FirstOrDefault();
            if (objGroup != null)
            {
                value = controller.Replace(objGroup.ItemCode, "");
            }
            else
            {
                value = controller;
            }

            return value;
        }

        public static string GetLayoutByGroup(string group)
        {
            var layout = "~/Views/Shared/_ParkingLayout.cshtml";
            switch (group)
            {
                //Tòa nhà
                case "67810176":
                    layout = "~/Views/Shared/_BMSLayout.cshtml";
                    break;
                //Vào ra
                case "98818976":
                    layout = "~/Views/Shared/_AccessLayout.cshtml";
                    break;
                //Bãi xe
                case "12878956":
                    layout = "~/Views/Shared/_ParkingLayout.cshtml";
                    break;

                //Tủ đồ
                case "61119719":
                    layout = "~/Views/Shared/_LockerLayout.cshtml";
                    break;
                //cư dân
                case "75675733":
                    layout = "~/Views/Shared/_ResidentLayout.cshtml";
                    break;
                default:
                    layout = "~/Views/Shared/_ParkingLayout.cshtml";
                    break;
            }
            return layout;
        }

        public static string DirectAreaByGroup(string group)
        {
            string direct = "";

            var objGroup = GroupMenuList().FirstOrDefault(n => n.ItemValue.Equals(group));
            if (objGroup != null)
            {
                direct = objGroup.AreaName;
            }
            else
            {
                direct = "";
            }

            return direct;
        }

        public static List<SelectListModel_Resolution> Resolution()
        {
            var list = new List<SelectListModel_Resolution> {
                                         new SelectListModel_Resolution { Text = "640x360" },
                                         new SelectListModel_Resolution { Text = "640x480" },
                                         new SelectListModel_Resolution { Text = "720x480" },
                                         new SelectListModel_Resolution { Text = "720x576" },
                                         new SelectListModel_Resolution { Text = "800x600" },
                                         new SelectListModel_Resolution { Text = "1280x720" },
                                         new SelectListModel_Resolution { Text = "1280x960" },
                                         new SelectListModel_Resolution { Text = "1280x1080" },
                                         new SelectListModel_Resolution { Text = "1920x1080" },
                                         new SelectListModel_Resolution { Text = "2048x1536" },
                                    };
            return list;
        }

        public static List<SelectListModel> CameraTypes1()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CameraTypes1");
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "", ItemText = Dictionary["CamType"]},
                                         new  SelectListModel{ ItemValue = "Geovision", ItemText = "Geovision"},
                                         new  SelectListModel{ ItemValue = "Panasonic i-Pro", ItemText = "Panasonic i-Pro"},
                                         new  SelectListModel{ ItemValue = "Axis", ItemText = "Axis"},
                                         new  SelectListModel{ ItemValue = "Secus", ItemText = "Secus"},
                                         new  SelectListModel{ ItemValue = "Shany-Stream1", ItemText = "Shany-Stream1"},
                                         new  SelectListModel{ ItemValue = "Shany-Stream21", ItemText = "Shany-Stream2"},
                                         new  SelectListModel{ ItemValue = "Vivotek", ItemText = "Vivotek"},
                                         new  SelectListModel{ ItemValue = "Lilin", ItemText = "Lilin"},
                                         new  SelectListModel{ ItemValue = "Messoa", ItemText = "Messoa"},
                                         new  SelectListModel{ ItemValue = "Entrovision", ItemText = "Entrovision"},
                                         new  SelectListModel{ ItemValue = "Sony", ItemText = "Sony"},
                                         new  SelectListModel{ ItemValue = "Bosch", ItemText = "Bosch"},
                                         new  SelectListModel{ ItemValue = "Vantech", ItemText = "Vantech"},
                                         new  SelectListModel{ ItemValue = "SC330", ItemText = "SC330"},
                                         new  SelectListModel{ ItemValue = "SecusFFMPEG", ItemText = "SecusFFMPEG"},
                                         new  SelectListModel{ ItemValue = "CNB", ItemText = "CNB"},//"CNB", "HIK", "Enster", "Afidus", "Dahua", "ITX"
                                         new  SelectListModel{ ItemValue = "HIK", ItemText = "HIK"},
                                         new  SelectListModel{ ItemValue = "Enster", ItemText = "Enster"},
                                         new  SelectListModel{ ItemValue = "Afidus", ItemText = "Afidus"},
                                         new  SelectListModel{ ItemValue = "Dahua", ItemText = "Dahua"},
                                         new  SelectListModel{ ItemValue = "ITX", ItemText = "ITX"},
                                         new  SelectListModel{ ItemValue = "Hanse", ItemText = "Hanse"},
                                          new  SelectListModel{ ItemValue = "Samsung", ItemText = "Samsung"},
                                          new  SelectListModel{ ItemValue = "Tiandy", ItemText = "Tiandy"},
                                          new  SelectListModel{ ItemValue = "Camtron", ItemText = "Camtron"},
                                          new  SelectListModel{ ItemValue = "HIVIZ", ItemText = "HIVIZ"},
                                          new  SelectListModel{ ItemValue = "DMAX", ItemText = "DMAX"}
                                    };
        }

        public static List<SelectListModel> StreamTypes1()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "StreamType");
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "H264", ItemText = "H264"},
                                         new  SelectListModel{ ItemValue = "MJPEG", ItemText = "MJPEG"},
                                         new  SelectListModel{ ItemValue = "PlayFile", ItemText = "PlayFile"},
                                         new  SelectListModel{ ItemValue = "Local Video Capture Device", ItemText = "Local Video Capture Device"},
                                         new  SelectListModel{ ItemValue = "JPEG", ItemText = "JPEG"},
                                         new  SelectListModel{ ItemValue = "MPEG4", ItemText = "MPEG4"},

                                         new  SelectListModel{ ItemValue = "Onvif", ItemText = "Onvif"}
                                    };
        }

        public static List<SelectListModel> DiscountModes()
        {

            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "0", ItemText = "Phần trăm"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Giờ"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Tiền"},

                                    };
        }

        public static List<SelectListModel> SDKs1()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "SDKs1");
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "KztekSDK2", ItemText = "KztekSDK2"},
                new  SelectListModel{ ItemValue = "AForgeSDK", ItemText = "AForgeSDK"},
                new  SelectListModel{ ItemValue = "AxisSDK", ItemText = "AxisSDK"},
                new  SelectListModel{ ItemValue = "GeoSDK", ItemText = "GeoSDK"},
                new  SelectListModel{ ItemValue = "ScSDK", ItemText = "ScSDK"},
                new  SelectListModel{ ItemValue = "FFMPEG", ItemText = "FFMPEG"},
                new  SelectListModel{ItemValue = "VLC", ItemText = "VLC"},
                new  SelectListModel{ItemValue = "KztekSDK", ItemText = "KztekSDK"},

                new  SelectListModel{ItemValue = "HIKSDK", ItemText = "HIKSDK"},
                new  SelectListModel{ItemValue = "TiandySDK", ItemText = "TiandySDK"},
                new  SelectListModel{ItemValue = "DahuaSDK", ItemText = "DahuaSDK"},
            };
        }

        public static List<SelectListModel6> Communication1()
        {
            return new List<SelectListModel6>
            {
                new SelectListModel6 {ItemValue = 1, ItemText = "TCP/IP"},
                new SelectListModel6 {ItemValue = 0, ItemText = "RS232/485/422"}
            };
        }

        //dung cho iparking
        public static List<SelectListModel> LineTypes1()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "ControllerType");
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = Dictionary["chooseControllerType"]},
                new  SelectListModel{ ItemValue = "0", ItemText = "IDTECK"},
                new  SelectListModel{ ItemValue = "1", ItemText = "Honeywell SY-MSA30/60L"},
                new  SelectListModel{ ItemValue = "2", ItemText = "Honeywell Nstar"},
                new  SelectListModel{ ItemValue = "3", ItemText = "Pegasus PP-3760"},
                new  SelectListModel{ ItemValue = "4", ItemText = "Pegasus PP-6750"},
                new  SelectListModel{ ItemValue = "5", ItemText = "Pegasus PFP-3700"},
                new  SelectListModel{ ItemValue = "6", ItemText = "FINGERTEC"},
                new  SelectListModel{ ItemValue = "7", ItemText = "DS3000"},
                new  SelectListModel{ ItemValue = "8", ItemText = "CS3000"},
                new  SelectListModel{ ItemValue = "9", ItemText = "RCP4000"},
                new  SelectListModel{ ItemValue = "10", ItemText = "PEGASUS PB7/PT3"},
                new  SelectListModel{ ItemValue = "11", ItemText = "PEGASUS PB5"},
                new  SelectListModel{ ItemValue = "12", ItemText = "IDTECK (006)"},
                new  SelectListModel{ ItemValue = "13", ItemText = "IDTECK (iTDC)"},
                new  SelectListModel{ ItemValue = "14", ItemText = "IDTECK (iMDC)"},
                new  SelectListModel{ ItemValue = "15", ItemText = "IDTECK (Elevator384)"},
                new  SelectListModel{ ItemValue = "16", ItemText = "Promax - FAT810W Kanteen"},
                new  SelectListModel{ ItemValue = "17", ItemText = "Promax - AC908"},
                new  SelectListModel{ ItemValue = "18", ItemText = "HAEIN S&amp;S"},
                new  SelectListModel{ ItemValue = "19", ItemText = "Promax - PCR310U"},
                new  SelectListModel{ ItemValue = "20", ItemText = "NetPOS Client MDB"},
                new  SelectListModel{ ItemValue = "21", ItemText = "NetPOS Client SERVER"},
                new  SelectListModel{ ItemValue = "22", ItemText = "Promax - FAT810W Parking"},
                new  SelectListModel{ ItemValue = "23", ItemText = "Promax - FAT810W Vending Machine"},
                new  SelectListModel{ ItemValue = "24", ItemText = "Pegasus - PP-110/PP-5210/PUA-310"},
                new  SelectListModel{ ItemValue = "25", ItemText = "Futech SC100"},
                new  SelectListModel{ ItemValue = "26", ItemText = "Honeywell HSR900"},
                new  SelectListModel{ ItemValue = "27", ItemText = "AC9xxPCR"},
                new  SelectListModel{ ItemValue = "28", ItemText = "E02.NET"},
                new  SelectListModel{ ItemValue = "29", ItemText = "Futech SC101"},
                new  SelectListModel{ ItemValue = "30", ItemText = "Futech SC100FPT"},
                new  SelectListModel{ ItemValue = "31", ItemText = "Futech SC100LANCASTER"},
                new  SelectListModel{ ItemValue = "32", ItemText = "Futech FUCM100"},
                new  SelectListModel{ ItemValue = "33", ItemText = "IDTECK 8 Number"},
                new  SelectListModel{ ItemValue = "34", ItemText = "E01 RS485"},
                new  SelectListModel{ ItemValue = "35", ItemText = "E02.NET Card Int"},
                new  SelectListModel{ ItemValue = "36", ItemText = "FUPC100"},
                new  SelectListModel{ ItemValue = "37", ItemText = "E02.NET Mifare"},
                new  SelectListModel{ ItemValue = "38", ItemText = "SOYAL"},
                new  SelectListModel{ ItemValue = "39", ItemText = "E02.NET Mifare SR30"},

                new  SelectListModel{ ItemValue = "40", ItemText = "Ingressus"},
                new  SelectListModel{ ItemValue = "41", ItemText = "E01 RS485 V2"},
                new  SelectListModel{ ItemValue = "42", ItemText = "Ingressus Mifare"},
                new  SelectListModel{ ItemValue = "43", ItemText = "FAT810WDispenser"},
                new  SelectListModel{ ItemValue = "44", ItemText = "FUCMHID100"},
                new  SelectListModel{ ItemValue = "45", ItemText = "USB Mifare"},
                new  SelectListModel{ ItemValue = "46", ItemText = "USB Proximity"},

                new  SelectListModel{ ItemValue = "47", ItemText = "IDTECKSR30"},
                new  SelectListModel{ ItemValue = "48", ItemText = "E02QRCode"},
                new  SelectListModel{ ItemValue = "49", ItemText = "E04.NET"},
                new  SelectListModel{ ItemValue = "50", ItemText = "E04.NET Mifare"},
                new  SelectListModel{ ItemValue = "51", ItemText = "E05.NET"},
                new  SelectListModel{ ItemValue = "52", ItemText = "KZ-MFC01.NET"},
                new  SelectListModel{ ItemValue = "53", ItemText = "E02_FPT"},
                new  SelectListModel{ ItemValue = "54", ItemText = "E05.NET Mifare"},
                new  SelectListModel{ ItemValue = "55", ItemText = "IDTECK Mifare"},
                new  SelectListModel{ ItemValue = "56", ItemText = "FaceMQTT"},
                new  SelectListModel{ ItemValue = "57", ItemText = "E02Mifare_BTNMT"},
                new  SelectListModel{ ItemValue = "58", ItemText = "FaceMQTT_V2"},
                new  SelectListModel{ ItemValue = "59", ItemText = "E02_FirstSeri10"},
                new  SelectListModel{ ItemValue = "60", ItemText = "Ingress_FirstSeri10"}  ,
                new  SelectListModel{ ItemValue = "61", ItemText = "SupremaCS40"},
                new  SelectListModel{ ItemValue = "62", ItemText = "KZE02NET"},
                new  SelectListModel{ ItemValue = "63", ItemText = "E05v2"},
                new  SelectListModel{ ItemValue = "64", ItemText = "SC200"},
                new  SelectListModel{ ItemValue = "65", ItemText = "KZE05NET"},
                new  SelectListModel{ ItemValue = "66", ItemText = "FAT810_Proximity_Dispenser"},
                new  SelectListModel{ ItemValue = "67", ItemText = "MT166_CardDispenser"},
                 new  SelectListModel{ ItemValue = "68", ItemText = "Soyal-AR725"}

            };
        }

        //dung cho he thong access, locker
        public static List<SelectListModel> LineTypes2()
        {
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = "-- Chọn loại"},
                new  SelectListModel{ ItemValue = "0", ItemText = "IDTECK"},
                new  SelectListModel{ ItemValue = "1", ItemText = "Honeywell SY-MSA30/60L"},
                new  SelectListModel{ ItemValue = "2", ItemText = "Honeywell Nstar"},
                new  SelectListModel{ ItemValue = "3", ItemText = "Pegasus PP-3760"},
                new  SelectListModel{ ItemValue = "4", ItemText = "Pegasus PP-6750"},
                new  SelectListModel{ ItemValue = "5", ItemText = "Pegasus PFP-3700"},
                new  SelectListModel{ ItemValue = "6", ItemText = "FINGERTEC"},
                new  SelectListModel{ ItemValue = "7", ItemText = "DS3000"},
                new  SelectListModel{ ItemValue = "8", ItemText = "CS3000"},
                new  SelectListModel{ ItemValue = "9", ItemText = "RCP4000"},
                new  SelectListModel{ ItemValue = "10", ItemText = "PEGASUS PB7/PT3"},
                new  SelectListModel{ ItemValue = "11", ItemText = "PEGASUS PB5"},
                new  SelectListModel{ ItemValue = "12", ItemText = "IDTECK (006)"},
                new  SelectListModel{ ItemValue = "13", ItemText = "IDTECK (iTDC)"},
                new  SelectListModel{ ItemValue = "14", ItemText = "IDTECK (iMDC)"},
                new  SelectListModel{ ItemValue = "15", ItemText = "IDTECK (Elevator384)"},
                new  SelectListModel{ ItemValue = "16", ItemText = "Promax - FAT810W Kanteen"},
                new  SelectListModel{ ItemValue = "17", ItemText = "Promax - AC908"},
                new  SelectListModel{ ItemValue = "18", ItemText = "HAEIN S&amp;S"},
                new  SelectListModel{ ItemValue = "19", ItemText = "Promax - PCR310U"},
                new  SelectListModel{ ItemValue = "20", ItemText = "NetPOS Client MDB"},
                new  SelectListModel{ ItemValue = "21", ItemText = "NetPOS Client SERVER"},
                new  SelectListModel{ ItemValue = "22", ItemText = "Promax - FAT810W Parking"},
                new  SelectListModel{ ItemValue = "23", ItemText = "Promax - FAT810W Vending Machine"},
                new  SelectListModel{ ItemValue = "24", ItemText = "Pegasus - PP-110/PP-5210/PUA-310"},
                new  SelectListModel{ ItemValue = "25", ItemText = "Futech SC100"},
                new  SelectListModel{ ItemValue = "26", ItemText = "Honeywell HSR900"},
                new  SelectListModel{ ItemValue = "27", ItemText = "AC9xxPCR"},
                new  SelectListModel{ ItemValue = "28", ItemText = "E02.NET"},
                new  SelectListModel{ ItemValue = "29", ItemText = "Futech SC101"},
                new  SelectListModel{ ItemValue = "30", ItemText = "Futech SC100FPT"},
                new  SelectListModel{ ItemValue = "31", ItemText = "Futech SC100LANCASTER"},
                new  SelectListModel{ ItemValue = "32", ItemText = "Futech FUCM100"},
                new  SelectListModel{ ItemValue = "33", ItemText = "IDTECK 8 Number"},
                new  SelectListModel{ ItemValue = "34", ItemText = "E01 RS485"},
                new  SelectListModel{ ItemValue = "35", ItemText = "E02.NET Card Int"},
                new  SelectListModel{ ItemValue = "36", ItemText = "FUPC100"},
                new  SelectListModel{ ItemValue = "37", ItemText = "E02.NET Mifar9e"},
                new  SelectListModel{ ItemValue = "38", ItemText = "SOYAL"},
                new  SelectListModel{ ItemValue = "39", ItemText = "E02.NET Mifare SR30"},
                new  SelectListModel{ ItemValue = "40", ItemText = "Ingressus"},
                new  SelectListModel{ ItemValue = "41", ItemText = "E01 RS485 V2"},
                new  SelectListModel{ ItemValue = "42", ItemText = "Ingressus Mifare"},
                new  SelectListModel{ ItemValue = "43", ItemText = "FAT810WDispenser"},
                new  SelectListModel{ ItemValue = "44", ItemText = "FUCMHID100"},
                new  SelectListModel{ ItemValue = "45", ItemText = "USB Mifare"},
                new  SelectListModel{ ItemValue = "46", ItemText = "USB Proximity"},
                new  SelectListModel{ ItemValue = "47", ItemText = "IDTECKSR30"},
                new  SelectListModel{ ItemValue = "48", ItemText = "E02QRCode"},
                new  SelectListModel{ ItemValue = "49", ItemText = "E04.NET"},
                new  SelectListModel{ ItemValue = "50", ItemText = "E04.NET Mifare"},
                new  SelectListModel{ ItemValue = "51", ItemText = "E05.NET"},
                new  SelectListModel{ ItemValue = "52", ItemText = "KZ-MFC01.NET"},
                new  SelectListModel{ ItemValue = "53", ItemText = "E02_FPT"},
                new  SelectListModel{ ItemValue = "54", ItemText = "E05.NET Mifare"},
                new  SelectListModel{ ItemValue = "55", ItemText = "IDTECK Mifare"},
                new  SelectListModel{ ItemValue = "56", ItemText = "FaceMQTT"},
                new  SelectListModel{ ItemValue = "57", ItemText = "E02Mifare_BTNMT"},
                new  SelectListModel{ ItemValue = "58", ItemText = "FaceMQTT_V2"},
                new  SelectListModel{ ItemValue = "59", ItemText = "E02_FirstSeri10"},
                new  SelectListModel{ ItemValue = "60", ItemText = "Ingress_FirstSeri10"},
                new  SelectListModel{ ItemValue = "61", ItemText = "SupremaCS40"},
                new  SelectListModel{ ItemValue = "62", ItemText = "KZE02NET"},
                new  SelectListModel{ ItemValue = "63", ItemText = "E05v2"},
                new  SelectListModel{ ItemValue = "64", ItemText = "SC200"},
                new  SelectListModel{ ItemValue = "65", ItemText = "KZE05NET"},
                new  SelectListModel{ ItemValue = "66", ItemText = "FAT810_Proximity_Dispenser"},
                new  SelectListModel{ ItemValue = "67", ItemText = "MT166_CardDispenser"},
                new  SelectListModel{ ItemValue = "68", ItemText = "ZKTECO_PULL"},
                new  SelectListModel{ ItemValue = "69", ItemText = "KZE32NET_V3_6"},
                new  SelectListModel{ ItemValue = "70", ItemText = "KZ_ELV_485_V1.0"},
                new  SelectListModel{ ItemValue = "71", ItemText = "FingerTecQ2i"}
            };
        }

        public static List<SelectListModel> ReaderTypes1()
        {
            return new List<SelectListModel>
            {
                new SelectListModel{ItemValue = "0", ItemText = "1"},
                new SelectListModel{ItemValue = "1", ItemText = "2"}
            };
        }

        public static List<SelectListModel> LaneTypes1()
        {
            //var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "SelectList");
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "LaneTypes1");
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = Dictionary["chooseAction"]},
                new  SelectListModel{ ItemValue = "0", ItemText = "0." + Dictionary["in"]},
                new  SelectListModel{ ItemValue = "1", ItemText = "1." + Dictionary["out"]},
                new  SelectListModel{ ItemValue = "2", ItemText = "2." + Dictionary["in"] +"-" + Dictionary["out"]},
                new  SelectListModel{ ItemValue = "3", ItemText = "3."+ Dictionary["in"] +"-" + Dictionary["in"]},
                new  SelectListModel{ ItemValue = "4", ItemText = "4."+ Dictionary["out"] +"-" + Dictionary["out"]},
                new  SelectListModel{ ItemValue = "5", ItemText = "5."+ Dictionary["in"] +"-" + Dictionary["out"] +"2"},
                 new  SelectListModel{ ItemValue = "6", ItemText = "6. #"},
            };
        }

        /// <summary>
        /// option kiểm tra biển số vào ra
        /// </summary>
        /// <returns></returns>
        public static List<SelectListModel6> CheckBS()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CheckPlateLevelOuts");

            var str = " (" + Dictionary["CardMonth"] + ")";

            var list = new List<SelectListModel6> {
                                        new SelectListModel6 { ItemValue = 1, ItemText = Dictionary["4char"] + str, Type = "IN"},
                                        new SelectListModel6 { ItemValue = 2, ItemText = Dictionary["all"] + str , Type = "IN"},
                                        new SelectListModel6 { ItemValue = 0, ItemText = Dictionary["noCheck"] + str , Type = "IN"},
                                        new SelectListModel6 { ItemValue = 1, ItemText = Dictionary["4char"], Type = "OUT"},
                                        new SelectListModel6 { ItemValue = 2, ItemText = Dictionary["all"] , Type = "OUT"},
                                        new SelectListModel6 { ItemValue = 0, ItemText = Dictionary["noCheck"], Type = "OUT"},
                                        new SelectListModel6 { ItemValue = 3, ItemText = Dictionary["CheckAllPlate"] , Type = "OUT"},
                                        new SelectListModel6 { ItemValue = 4, ItemText = Dictionary["CheckPlateInOut"] , Type = "OUT"},
                                         new SelectListModel6 { ItemValue = 5, ItemText = Dictionary["CheckPlateOutAndRegistration"], Type = "OUT"}
                                    };
            return list;
        }

        /// <summary>
        /// Thứ tự trên giao diện
        /// </summary>
        /// <returns></returns>
        public static List<SelectListModel> SideIndex()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Giao diện trái"},
                                        new SelectListModel { ItemValue = "2", ItemText = "Giao diện phải"},

                                    };
            return list;
        }

        /// <summary>
        /// Thứ tự trên giao diện
        /// </summary>
        /// <returns></returns>
        public static List<SelectListModel> ListCurrentDirection()
        {



            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Chiều vào"},
                                        new SelectListModel { ItemValue = "2", ItemText = "Chiều ra"},

                                    };
            return list;
        }
        public static List<SelectListModel6> OpenBarrie()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "OpenBarrie");



            var list = new List<SelectListModel6> {
                                        new SelectListModel6 { ItemValue = 0, ItemText = Dictionary["noOpen"]},
                                        new SelectListModel6 { ItemValue = 1, ItemText = Dictionary["noOpenAndnoCheck"]},
                                        new SelectListModel6 { ItemValue = 2, ItemText = Dictionary["OpenAllTicket"]},
                                        new SelectListModel6 { ItemValue = 3, ItemText = Dictionary["OnlyCardMonthAndAllType"]},
                                        new SelectListModel6 { ItemValue = 4, ItemText = Dictionary["AutoInOutSame"]},
                                         new SelectListModel6 { ItemValue = 5, ItemText = Dictionary["AutoInOutDifferent"]},
                                         new SelectListModel6 { ItemValue = 6, ItemText = Dictionary["OnlyCardMonth"]},
                                         new SelectListModel6 { ItemValue = 7, ItemText = Dictionary["OnlyTurnMonth"]},
                                         new SelectListModel6 { ItemValue = 8, ItemText = Dictionary["OnlyMonthCard"]},
                                    };
            return list;
        }


        public static List<SelectListModel> HubList1()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "HubList1");
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = Dictionary["select"]},
                                         new SelectListModel { ItemValue = "0", ItemText = Dictionary["left"]},
                                         new SelectListModel { ItemValue = "1", ItemText =  Dictionary["right"]},
                                         new SelectListModel { ItemValue = "2", ItemText =  Dictionary["all"]},
                                    };
            return list;
        }

        public static List<SelectListModel> LEDType1()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = FunctionHelper.GetLocalizeDictionary("SelectList", "HubList1")["select"]},
                                         new SelectListModel { ItemValue = "1", ItemText = "DSP840"},
                                         new SelectListModel { ItemValue = "2", ItemText = "FUTECH"},
                                         new SelectListModel { ItemValue = "3", ItemText = "FAT810"},
                                         new SelectListModel { ItemValue = "4", ItemText = "FUTECH2"},
                                         new SelectListModel { ItemValue = "5", ItemText = "FUTECH2LINE"},
                                         new SelectListModel { ItemValue = "6", ItemText = "PGS_LED"},
                                         new SelectListModel { ItemValue = "7", ItemText = "ATPRO"},
                                          new SelectListModel { ItemValue = "8", ItemText = "P10_RED"},
                                         new SelectListModel { ItemValue = "9", ItemText = "P10_Fullcolor"},
                                         new SelectListModel { ItemValue = "10", ItemText = "P7_62_RGY"}
                                    };
            return list;
        }

        public static List<SelectListModel> FormulationList()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "FormulationList");
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = Dictionary["turn"]},
                                         new SelectListModel { ItemValue = "1", ItemText = Dictionary["block"]},
                                         new SelectListModel { ItemValue = "2", ItemText = Dictionary["periodTime"]}
                                    };
            return list;
        }

        public static List<SelectListModel> CardTypes()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CardTypes");

            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = Dictionary["monthlyTicket"]},
                                         new SelectListModel { ItemValue = "1", ItemText = Dictionary["ticketEachTime"]},
                                         new SelectListModel { ItemValue = "2", ItemText = Dictionary["freeTicket"]},
                                          new SelectListModel { ItemValue = "3", ItemText ="Thẻ VIP"}
                                    };
            return list;
        }

        public static List<SelectListModel> VehicleType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = "Ô tô"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Xe máy"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe đạp"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Xe đạp điện"}
                                    };
            return list;
        }

        public static List<SelectListModel> AlarmCodes()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "AlarmCodes");
            var list = new List<SelectListModel> {
                      // dự án hòa phát mandarin garden 15/7/2019 dungdt
                      //Sự kiện cảnh báo chưa có trường thông tin: "Không tồn tại trên hệ thống" => đổi tên 001 unknownCard <-> Not_exist_on_the_system
                                         new SelectListModel { ItemValue = "001", ItemText = Dictionary["Not_exist_on_the_system"] },
                                         new SelectListModel { ItemValue = "002", ItemText = Dictionary["lock"]},
                                         new SelectListModel { ItemValue = "003", ItemText = Dictionary["permissions"]},
                                         new SelectListModel { ItemValue = "004", ItemText = Dictionary["inParking"]},
                                         new SelectListModel { ItemValue = "005", ItemText = Dictionary["notInParking"]},
                                         new SelectListModel { ItemValue = "006", ItemText = Dictionary["openBarrieByComputer"]},
                                         new SelectListModel { ItemValue = "007", ItemText = Dictionary["openBarrieByButton"]},
                                         new SelectListModel { ItemValue = "008", ItemText = Dictionary["eventEscapeIn"]},
                                         new SelectListModel { ItemValue = "009", ItemText = Dictionary["eventEscapeOut"]},
                                         new SelectListModel { ItemValue = "010", ItemText = Dictionary["Card_expired"]},
                                         new SelectListModel { ItemValue = "011", ItemText = Dictionary["Invalid_license_plates"]},
                                         new SelectListModel { ItemValue = "012", ItemText = Dictionary["Black_list"]},
                                         new SelectListModel { ItemValue = "013", ItemText = Dictionary["lock_License_plate"]},
                                         new SelectListModel { ItemValue = "014", ItemText = Dictionary["License_plate_expired"]},
                                         new SelectListModel { ItemValue = "015", ItemText = Dictionary["Locked_tag_group"]},
                                         new SelectListModel { ItemValue = "016", ItemText = Dictionary["License_plate_do_not_match"]},
                                         new SelectListModel { ItemValue = "017", ItemText = Dictionary["Vehicles_beyond_the_magnetic_ring"]},
                                                new SelectListModel { ItemValue = "018", ItemText = "Mở barrie bằng nút bấm (hợp lệ)"},
                                         new SelectListModel { ItemValue = "019", ItemText = "Mở barrie bằng bàn phím (hợp lệ)"},
                                         new SelectListModel { ItemValue = "020", ItemText = "Ghi vé vào"},
                                         new SelectListModel { ItemValue = "021", ItemText = "Ghi vé ra"},
                                    };
            return list;
        }

        public static Stream WriteToExcel(Stream stream = null, DataTable listData = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string sheetname = "", string comments = "")
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "FutechJSC";

                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "";

                // thêm comments
                excelPackage.Workbook.Properties.Comments = comments;

                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(sheetname);

                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];

                workSheet.Cells[1, 1, 1, 13].Merge = true;
                workSheet.Cells[2, 1, 2, 13].Merge = true;
                workSheet.Cells[3, 1, 3, 13].Merge = true;
                workSheet.Cells[4, 1, 4, 13].Merge = true;
                workSheet.Cells[5, 1, 5, 13].Merge = true;
                workSheet.Cells[6, 1, 6, 13].Merge = true;

                workSheet.Cells.AutoFitColumns();
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                // Đổ data vào Excel file
                var count = 0;

                //tao header cho file
                var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //if (listData != null && listData.Rows.Count > 0)
                //{
                //Load danh sách phần header
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        workSheet.Cells[i + 1, 1].Value = dr["header"].ToString();
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                    }
                }

                //Load phần tiêu đề của từng côtj
                foreach (var item in listTitle)
                {
                    count++;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Value = item.ItemText;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.Font.Bold = true;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }
                //}

                //workSheet.Cells[dtHeader.Rows.Count + 1, 1, listData.Rows.Count + 1, listTitle.Count()].AutoFitColumns();

                workSheet.Cells.AutoFitColumns();

                //Dòng bắt đầu của dữ liệu
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[dtHeader.Rows.Count + 2, 1].LoadFromDataTable(listData, false);
                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells.Style.Font.Size = 12;
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells.AutoFitColumns();

                //Save lại
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static Stream WriteToExcel_ByCardGroup_LOTTE(Stream stream = null, IEnumerable<tblVehicleGroup> vehicleGroups = null, List<TotalByCardGroup> listData = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string sheetname = "", string comments = "")
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "FutechJSC";

                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "";

                // thêm comments
                excelPackage.Workbook.Properties.Comments = comments;

                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(sheetname);

                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];

                workSheet.Cells[1, 1, 1, 13].Merge = true;
                workSheet.Cells[2, 1, 2, 13].Merge = true;
                workSheet.Cells[3, 1, 3, 13].Merge = true;
                workSheet.Cells[4, 1, 4, 13].Merge = true;
                workSheet.Cells[5, 1, 5, 13].Merge = true;
                workSheet.Cells[6, 1, 6, 13].Merge = true;

                workSheet.Cells.AutoFitColumns();
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                // Đổ data vào Excel file
                var count = 0;

                //tao header cho file
                var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //if (listData != null && listData.Rows.Count > 0)
                //{
                //Load danh sách phần header
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        workSheet.Cells[i + 1, 1].Value = dr["header"].ToString();
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                    }
                }

                //Load phần tiêu đề của từng côtj
                foreach (var item in listTitle)
                {
                    count++;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Value = item.ItemText;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.Font.Bold = true;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                //}

                //workSheet.Cells[dtHeader.Rows.Count + 1, 1, listData.Rows.Count + 1, listTitle.Count()].AutoFitColumns();

                workSheet.Cells.AutoFitColumns();

                //Dòng bắt đầu của dữ liệu
                //Số dòng hiển thị dữ liệu
                int countR = 7;

                //STT dữ liệu
                int countD = 0;

                foreach (var item in vehicleGroups)
                {
                    var cardGroups = listData.Where(n => n.VehicleGroupID == item.VehicleGroupID.ToString());

                    var totalQuan = 0;

                    decimal totalMoneys = 0;

                    if (cardGroups != null && cardGroups.Count() > 0)
                    {
                        foreach (var itemC in cardGroups)
                        {
                            countR++;

                            countD++;

                            totalQuan += itemC.Quantity;

                            totalMoneys += itemC.Total;

                            for (int i = 1; i <= 4; i++)
                            {
                                workSheet.Cells[countR, i].Value = i == 1 ? countD.ToString() : (i == 2 ? itemC.CardGroupID : (i == 3 ? itemC.Quantity.ToString() : (itemC.Total > 0 ? itemC.Total.ToString("###,###") : "0")));

                                if (i == 3 || i == 4)
                                {
                                    workSheet.Cells[countR, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }


                                workSheet.Cells[countR, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                workSheet.Cells[countR, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                workSheet.Cells.Style.Font.Name = "Times New Roman";

                                workSheet.Cells.Style.Font.Size = 12;
                            }
                        }

                        #region Tổng từng nhóm xe
                        countR++;

                        for (int i = 1; i <= 4; i++)
                        {
                            workSheet.Cells[countR, i].Value = i == 1 ? "" : (i == 2 ? "Tổng" : (i == 3 ? totalQuan.ToString() : (totalMoneys > 0 ? totalMoneys.ToString("###,###") : "0")));

                            if (i == 3 || i == 4)
                            {
                                workSheet.Cells[countR, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }


                            workSheet.Cells[countR, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            workSheet.Cells[countR, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            workSheet.Cells[countR, i].Style.Font.Bold = true;

                            workSheet.Cells.Style.Font.Name = "Times New Roman";

                            workSheet.Cells.Style.Font.Size = 12;
                        }
                        #endregion
                    }
                }

                workSheet.Cells.AutoFitColumns();

                //Save lại
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static Stream BAOVIET_WriteToExcel(Stream stream = null, DataTable listData = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string sheetname = "", string comments = "", string logopath = "")
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {

                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "FutechJSC";

                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "";

                // thêm comments
                excelPackage.Workbook.Properties.Comments = comments;

                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(sheetname);

                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];

                workSheet.Cells[1, 1, 1, 13].Merge = true;
                workSheet.Cells[2, 1, 2, 13].Merge = true;
                workSheet.Cells[3, 1, 3, 13].Merge = true;
                workSheet.Cells[4, 1, 4, 13].Merge = true;
                workSheet.Cells[5, 1, 5, 13].Merge = true;
                workSheet.Cells[6, 1, 6, 13].Merge = true;

                workSheet.Cells.AutoFitColumns();
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                // Đổ data vào Excel file
                var count = 0;

                //tao header cho file
                var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                if (!string.IsNullOrEmpty(logopath))
                {
                    workSheet.Row(1).Height = 100;

                    Image logo = Image.FromFile(logopath);

                    var excelImage = workSheet.Drawings.AddPicture("My Logo", logo);

                    //add the image to row 20, column E
                    excelImage.SetPosition(0, 0, 0, 0);

                }

                //if (listData != null && listData.Rows.Count > 0)
                //{
                //Load danh sách phần header
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        workSheet.Cells[i + 2, 1].Value = dr["header"].ToString();
                        workSheet.Cells[i + 2, 1].Style.Font.Bold = true;
                        workSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    }
                }

                //Load phần tiêu đề của từng côtj
                foreach (var item in listTitle)
                {
                    count++;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Value = item.ItemText;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.Font.Bold = true;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }
                //}

                //workSheet.Cells[dtHeader.Rows.Count + 1, 1, listData.Rows.Count + 1, listTitle.Count()].AutoFitColumns();

                workSheet.Cells.AutoFitColumns();

                //Dòng bắt đầu của dữ liệu
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[dtHeader.Rows.Count + 3, 1].LoadFromDataTable(listData, false);
                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells.Style.Font.Size = 12;
                workSheet.Cells[2, 1].Style.Font.Size = 15;
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells.AutoFitColumns();

                //Save lại
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static Stream BVDK_WriteToExcel(Stream stream = null, DataTable listData = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string sheetname = "", string comments = "")
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "FutechJSC";

                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "";

                // thêm comments
                excelPackage.Workbook.Properties.Comments = comments;

                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(sheetname);

                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];

                var countcolumn = listData.Columns.Count;

                workSheet.Cells[1, 1, 1, 12].Merge = true;
                workSheet.Cells[2, 1, 2, 12].Merge = true;
                workSheet.Cells[3, 1, 3, 12].Merge = true;
                workSheet.Cells[4, 1, 4, 12].Merge = true;
                workSheet.Cells[5, 1, 5, 12].Merge = true;
                workSheet.Cells[6, 1, 6, 12].Merge = true;

                workSheet.Cells.AutoFitColumns();
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                // Đổ data vào Excel file
                var count = 0;

                //tao header cho file
                var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //Load danh sách phần header
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        workSheet.Cells[i + 1, 1].Value = dr["header"].ToString();
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;

                        if (i == 3)
                        {
                            workSheet.Cells[i + 1, 1].Style.Font.Size = 18;
                            workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                            workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        if (i == 4)
                        {
                            workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                    }
                }

                //Load phần tiêu đề của từng côtj
                foreach (var item in listTitle)
                {
                    count++;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Value = item.ItemText;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.Font.Bold = true;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                }
                //}

                //workSheet.Cells[dtHeader.Rows.Count + 1, 1, listData.Rows.Count + 1, listTitle.Count()].AutoFitColumns();

                workSheet.Cells.AutoFitColumns();

                //Dòng bắt đầu của dữ liệu

                //workSheet.Cells[dtHeader.Rows.Count + 3, 1].LoadFromDataTable(listData, false);
                var rowStart = dtHeader.Rows.Count + 3;
                for (int i = 0; i < listData.Rows.Count; i++)
                {
                    DataRow item = listData.Rows[i];
                    for (int j = 0; j < listData.Columns.Count; j++)
                    {
                        var _fromRow = rowStart + (i);
                        var _fromCol = j;

                        workSheet.Cells[_fromRow, _fromCol + 1].Value = item[j].ToString();

                        workSheet.Cells[_fromRow, _fromCol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        workSheet.Cells[_fromRow, _fromCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        workSheet.Cells[rowStart + listData.Rows.Count - 1, _fromCol + 1].Style.Font.Bold = true;

                    }
                }
                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells.Style.Font.Size = 12;
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells.AutoFitColumns();

                var rowcount = listData.Rows.Count + dtHeader.Rows.Count + 4;

                workSheet.Cells[rowcount, 12].Value = "NGƯỜI LẬP";
                workSheet.Cells[rowcount, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                workSheet.Cells[rowcount, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[rowcount, 12].Style.Font.Bold = true;

                //Save lại
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static Stream WriteToExcelBVDK_ReportTotalMoneyByCardGroup(Stream stream = null, DataTable listData = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string sheetname = "", string comments = "")
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "FutechJSC";

                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "";

                // thêm comments
                excelPackage.Workbook.Properties.Comments = comments;

                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(sheetname);

                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];

                workSheet.Cells[1, 1, 1, 10].Merge = true;
                workSheet.Cells[2, 1, 2, 10].Merge = true;
                workSheet.Cells[3, 1, 3, 10].Merge = true;
                workSheet.Cells[4, 1, 4, 10].Merge = true;
                workSheet.Cells[5, 1, 5, 10].Merge = true;
                workSheet.Cells[6, 1, 6, 10].Merge = true;

                workSheet.Cells.AutoFitColumns();
                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                // Đổ data vào Excel file
                var count = 0;

                //tao header cho file
                var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


                //Load danh sách phần header
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        workSheet.Cells[i + 1, 1].Value = dr["header"].ToString();
                        workSheet.Cells[i + 1, 1].Style.Font.Bold = true;

                        if (i == 3)
                        {
                            workSheet.Cells[i + 1, 1].Style.Font.Size = 16;
                            workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
                            workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        if (i == 4)
                        {
                            workSheet.Cells[i + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                    }
                }

                //Load phần tiêu đề của từng côtj
                // tạo header cho danh sách              
                foreach (var item in listTitle)
                {
                    count++;
                    var a = "B7:F7";
                    workSheet.Cells[a].Merge = true;
                    workSheet.Cells[a].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var b = "G7:J7";
                    workSheet.Cells[b].Merge = true;
                    workSheet.Cells[b].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    if (count == 3)
                    {
                        workSheet.Cells[dtHeader.Rows.Count + 2, count + 4].Value = item.ItemText;
                        workSheet.Cells[dtHeader.Rows.Count + 2, count + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[dtHeader.Rows.Count + 2, count + 4].Style.Font.Bold = true;
                    }
                    else
                    {
                        workSheet.Cells[dtHeader.Rows.Count + 2, count].Value = item.ItemText;
                    }

                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.Font.Bold = true;
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[dtHeader.Rows.Count + 2, count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                }

                workSheet.Cells.AutoFitColumns();

                //Dòng bắt đầu của dữ liệu
                var rowStart = dtHeader.Rows.Count + 3;
                // Đỗ dữ liệu từ list vào 
                for (int i = 0; i < listData.Rows.Count; i++)
                {
                    var countmerge = (8 + i).ToString();
                    var a = "B" + countmerge + ":" + "F" + countmerge;
                    workSheet.Cells[a].Merge = true;
                    workSheet.Cells[a].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var b = "G" + countmerge + ":" + "J" + countmerge;
                    workSheet.Cells[b].Merge = true;
                    workSheet.Cells[b].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    DataRow item = listData.Rows[i];
                    for (int j = 0; j < listData.Columns.Count; j++)
                    {
                        var _fromRow = rowStart + (i);
                        var _fromCol = j;

                        workSheet.Cells[_fromRow, _fromCol + 1].Style.Numberformat.Format = "@";


                        if (_fromCol == 1)
                        {
                            workSheet.Cells[_fromRow, _fromCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            workSheet.Cells[_fromRow, _fromCol + 1].Value = item[j].ToString();

                        }
                        else if (_fromCol == 2)
                        {
                            workSheet.Cells[_fromRow, _fromCol + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet.Cells[_fromRow, _fromCol + 5].Value = item[j].ToString();

                            workSheet.Cells[rowStart + listData.Rows.Count - 1, _fromCol + 5].Style.Font.Bold = true;
                        }
                        else
                        {
                            workSheet.Cells[_fromRow, _fromCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[_fromRow, _fromCol + 1].Value = item[j].ToString();
                        }


                        workSheet.Cells[_fromRow, _fromCol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        workSheet.Cells[_fromRow, _fromCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        workSheet.Cells[rowStart + listData.Rows.Count - 1, _fromCol + 1].Style.Font.Bold = true;

                    }
                }


                workSheet.Cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells.AutoFitColumns();

                var rowcount = listData.Rows.Count + dtHeader.Rows.Count + 4;

                workSheet.Cells[rowcount, listData.Columns.Count + 6].Value = "NGƯỜI LẬP";
                workSheet.Cells[rowcount, listData.Columns.Count + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                workSheet.Cells[rowcount, listData.Columns.Count + 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[rowcount, listData.Columns.Count + 6].Style.Font.Bold = true;


                //Save lại
                excelPackage.Save();

                return excelPackage.Stream;
            }
        }

        public static string ReplaceStringUrl(string url)
        {
            //
            var host = HttpContext.Current.Request.Url.Host;

            //Khai báo
            string str = "";

            if (url.Contains("PICRA"))
            {
                str = "http:" + url.Replace(@"\SERVERIPARK\PICRA", @"\192.168.1.80:100").Replace(@"/SERVERIPARK/PICRA", @"\192.168.1.80:100").Replace(@"\HH2-2A\PICRA", @"\192.168.1.80:100").Replace(@"\SERVERPARKING\PICRA", @"\192.168.1.80:100").Replace(@"/SERVERPARKING/PICRA", @"\192.168.1.80:100");
            }
            else
            {
                str = "http:" + url.Replace(@"\SERVERIPARK\PIC", @"\192.168.1.80:99").Replace(@"/SERVERIPARK/PIC", @"\192.168.1.80:99").Replace(@"\HH2-2A\PIC", @"\192.168.1.80:99").Replace(@"\SERVERPARKING\PIC", @"\192.168.1.80:99").Replace(@"/SERVERPARKING/PIC", @"/192.168.1.80:99");
            }

            return str;
        }

        public static List<SelectListModel> CustomerStatusType()
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("SelectList", "CustomerStatusType");
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = Dictionary["active"]},
                                         new SelectListModel { ItemValue = "1", ItemText = Dictionary["inactive"]}
                                    };
            return list;
        }

        public static string GetStatusDateByDay(DateTime date)
        {
            var time = DateTime.Now;
            var status = "";


            if (time < date)
            {
                var newDate = date.AddDays(-7);
                if (newDate > time)
                {
                    status = string.Format("<span>{0}</span>", date.ToString("dd/MM/yyyy"));
                }
                else
                {
                    status = string.Format("<span class='label label-warning label-white middle'>{0}</span>", date.ToString("dd/MM/yyyy"));
                }
            }
            else
            {
                status = string.Format("<span class='label label-danger label-white middle'>{0}</span>", date.ToString("dd/MM/yyyy"));
            }

            return status;
        }

        public static DataTable ReadFromExcelCardCustomer(string path, ref string errorText)
        {
            try
            {
                // Khởi tạo data table
                var dt = new DataTable();
                // Load file excel và các setting ban đầu
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        // Đọc tất cả các header
                        foreach (var firstRowCell in workSheet.Cells[7, 1, 7, workSheet.Dimension.End.Column])
                        {
                            if (!string.IsNullOrWhiteSpace(firstRowCell.Text))
                            {
                                dt.Columns.Add(firstRowCell.Text.Trim());
                            }
                        }
                        // Đọc tất cả data bắt đầu từ row thứ n
                        for (var rowNumber = 8; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                        {
                            var rowCell1 = workSheet.Cells[rowNumber, 1];

                            if (!string.IsNullOrWhiteSpace((rowCell1.Text)))
                            {
                                // Lấy 1 row trong excel để truy vấn
                                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                // tạo 1 row trong data table
                                var newRow = dt.NewRow();
                                foreach (var cell in row)
                                {
                                    //if (!string.IsNullOrWhiteSpace(cell.Text))
                                    //{
                                    //    newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                    //}
                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                    {
                                        if (cell.Address.Contains("E"))
                                        {
                                            if (cell.Text.Length < 8)
                                            {
                                                var a = Convert.ToDateTime(cell.Value).Date;
                                                newRow[cell.Start.Column - 1] = a.Year + "/" + a.Day + "/" + a.Month;
                                            }
                                            else
                                            {
                                                DateTime d = DateTime.ParseExact(!string.IsNullOrEmpty(cell.Text) ? cell.Text : DateTime.Now.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                                newRow[cell.Start.Column - 1] = d.ToString("yyyy/MM/dd");
                                            }

                                        }
                                        else
                                        {
                                            newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                        }
                                    }
                                }
                                dt.Rows.Add(newRow);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// Hòa Phát Dung Quất
        /// </summary>
        /// <param name="path"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        public static DataTable ReadFromExcelCardCustomer_HPDQ(string path, ref string errorText)
        {
            try
            {
                // Khởi tạo data table
                var dt = new DataTable();
                // Load file excel và các setting ban đầu
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        // Đọc tất cả các header
                        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                        {
                            if (!string.IsNullOrWhiteSpace(firstRowCell.Text))
                            {
                                dt.Columns.Add(firstRowCell.Text.Trim());
                            }
                        }
                        // Đọc tất cả data bắt đầu từ row thứ n
                        for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                        {
                            var rowCell1 = workSheet.Cells[rowNumber, 1];

                            if (!string.IsNullOrWhiteSpace((rowCell1.Text)))
                            {
                                // Lấy 1 row trong excel để truy vấn
                                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                // tạo 1 row trong data table
                                var newRow = dt.NewRow();
                                foreach (var cell in row)
                                {
                                    //if (!string.IsNullOrWhiteSpace(cell.Text))
                                    //{
                                    //    newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                    //}
                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                    {
                                        if (cell.Address.Contains("Z"))
                                        {
                                            if (cell.Text.Length < 8)
                                            {
                                                var a = Convert.ToDateTime(cell.Value).Date;
                                                newRow[cell.Start.Column - 1] = a.Day + "/" + a.Month + "/" + a.Year;
                                            }
                                            else
                                            {
                                                //DateTime d = DateTime.ParseExact(!string.IsNullOrEmpty(cell.Text) ? cell.Text : DateTime.Now.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                                //newRow[cell.Start.Column - 1] = d.ToString("yyyy/MM/dd");
                                                newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                            }

                                        }
                                        else
                                        {
                                            newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                        }

                                    }
                                }
                                dt.Rows.Add(newRow);
                            }
                        }
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return null;
        }

        public static object[,] ReadFromExcel_HPDQ_CSV(string path, ref string errorText)
        {
            try
            {
                // Khởi tạo data table
                var dt = new DataTable();

                ExcelTextFormat format = new ExcelTextFormat();
                format.Delimiter = ';';
                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                format.Encoding = new UTF8Encoding();

                //read the CSV file from disk
                FileInfo file = new FileInfo(path);

                //or if you use asp.net, get the relative path
                // FileInfo file = new FileInfo(Server.MapPath("CSVDemo.csv"));

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    //create a WorkSheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                    //load the CSV data into cell A1
                    worksheet.Cells["A1"].LoadFromText(file, format);

                    if (worksheet.Cells.Value != null)
                    {
                        var dataList = (object[,])worksheet.Cells.Value;

                        return dataList;
                    }
                }



            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return null;
        }
        /// <summary>
        /// đọc file excel cho khách hàng
        /// </summary>
        /// <param name="path"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        public static DataTable ReadExcelCustomer(string path, ref string errorText)
        {
            try
            {
                // Khởi tạo data table
                var dt = new DataTable();
                // Load file excel và các setting ban đầu
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    if (package.Workbook.Worksheets.Count >= 1)
                    {
                        // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        // Đọc tất cả các header
                        foreach (var firstRowCell in workSheet.Cells[7, 1, 7, workSheet.Dimension.End.Column])
                        {
                            if (!string.IsNullOrWhiteSpace(firstRowCell.Text))
                            {
                                dt.Columns.Add(firstRowCell.Text.Trim());
                            }
                        }
                        // Đọc tất cả data bắt đầu từ row thứ n
                        for (var rowNumber = 8; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                        {
                            var rowCell1 = workSheet.Cells[rowNumber, 1];

                            if (!string.IsNullOrWhiteSpace((rowCell1.Text)))
                            {
                                // Lấy 1 row trong excel để truy vấn
                                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                                // tạo 1 row trong data table
                                var newRow = dt.NewRow();
                                foreach (var cell in row)
                                {
                                    if (!string.IsNullOrWhiteSpace(cell.Text))
                                    {
                                        newRow[cell.Start.Column - 1] = cell.Text.Trim();
                                    }

                                }
                                dt.Rows.Add(newRow);
                            }
                        }
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            return null;
        }

        public async static Task<string> FtpImage(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                string[] fileitem = filename.Split(Convert.ToChar(@"\"));
                if (fileitem != null && fileitem.Length == 6)
                {
                    string _fileonftp = "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5];

                    return await Task.FromResult(_fileonftp);
                }
                else if (fileitem != null && fileitem.Length == 7)
                {
                    string _fileonftp = "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5] + "/" + fileitem[6];

                    return await Task.FromResult(_fileonftp);
                }
            }
            else
            {
                //
                string username = ConfigurationManager.AppSettings["FTPUserName"].ToString();
                string pass = ConfigurationManager.AppSettings["FTPPassword"].ToString();

                string[] fileitem = filename.Split(Convert.ToChar(@"\"));

                if (fileitem != null && fileitem.Length == 6)
                {
                    string _fileonftp = "ftp://" + username + ":" + pass + "@" + fileitem[2] + "/" + fileitem[3] + "/" + fileitem[4] + "/" + fileitem[5];

                    if (await CheckIfFileExistsOnServer(_fileonftp) == true)
                    {
                        WebClient web = new WebClient();
                        var t = web.DownloadData(_fileonftp);
                        if (t != null)
                        {
                            var k = ConvertByteToBase64(t);

                            return await Task.FromResult(k);
                        }
                    }
                }
            }

            string[] file = filename.Split(Convert.ToChar(@"\"));

            string str = file != null && file.Length == 6 ? "/" + file[3] + "/" + file[4] + "/" + file[5] : "";

            return await Task.FromResult(str);
        }

        public async static Task<string> FtpImage_V5(string filename)
        {
            string[] fileitem = filename.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (fileitem != null)
            {
                var sb = new StringBuilder();
                for (int i = 1; i < fileitem.Count(); i++)
                {
                    sb.Append($"/{fileitem[i]}");
                }
                string _fileonftp = sb.ToString();

                return await Task.FromResult(_fileonftp);
            }

            return await Task.FromResult("");
        }

        private async static Task<bool> CheckIfFileExistsOnServer(string fileName)
        {
            bool isSuccess = false;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileName);
            string username = ConfigurationManager.AppSettings["FTPUserName"].ToString();
            string pass = ConfigurationManager.AppSettings["FTPPassword"].ToString();
            request.Credentials = new NetworkCredential(username, pass);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {

                Thread thre = new Thread(() =>
                {
                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    //Thread.Sleep(1000);
                });

                thre.Start();
                //thre.Join();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;

                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)

                    isSuccess = false;
            }

            return await Task.FromResult(isSuccess);
        }

        public static void Ping(object filename)
        {

        }

        public static string CheckConnectController(string url, string controllerid)
        {
            var result = false;

            try
            {
                using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(5000);
                    Task<String> response = httpClient.GetStringAsync("http://" + url + ":8081/api/register/getcontrollerstate?controllerid=" + controllerid);
                    result = JsonConvert.DeserializeObject<bool>(response.Result);
                }
            }
            catch (Exception ex)
            {

            }

            if (result)
            {
                return "<span class='label label-success'>Connect</span>";
            }
            else
            {
                return "<span class='label label-danger'>Disconnect</span>";
            }
        }

        public static string CheckConnectControllerDesktop(string url, string controllerid)
        {
            var result = false;

            try
            {
                using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(5000);
                    Task<String> response = httpClient.GetStringAsync("http://" + url + ":8081/api/register/getcontrollerstate?controllerid=" + controllerid);
                    result = JsonConvert.DeserializeObject<bool>(response.Result);
                }
            }
            catch (Exception ex)
            {

            }

            if (result)
            {
                return "Connect";
            }
            else
            {
                return "Disconnect";
            }
        }

        public static string GetFinger(string url, string controllerid, string userid, string finger)
        {
            var result = "";

            try
            {
                var fullurl = url + ":8081/api/register/getfinger?ControllerID=" + controllerid + "&userid=" + userid + "&fingerid=" + finger;

                var uri = new UriBuilder(fullurl);

                using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
                    Task<String> response = httpClient.GetStringAsync(uri.ToString());
                    result = JsonConvert.DeserializeObject<string>(response.Result);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static string MonthName(string fromdate, string todate)
        {
            string monthname = "";
            List<int> listmonth = new List<int>();

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                var monthfdate = Convert.ToDateTime(fromdate).Month;
                var monthedate = Convert.ToDateTime(todate).Month;

                string fName = new DateTimeFormatInfo().GetMonthName(monthfdate).Substring(0, 3);
                string eName = new DateTimeFormatInfo().GetMonthName(monthedate).Substring(0, 3);
                monthname = fName + ",";

                for (int i = 1; i < 12; i++)
                {
                    var month = Convert.ToDateTime(fromdate).AddMonths(i);

                    if (month < Convert.ToDateTime(todate))
                    {
                        if (monthedate != month.Month)
                        {
                            string name = new DateTimeFormatInfo().GetMonthName(month.Month).Substring(0, 3);
                            monthname += name + ",";
                        }

                    }

                }
                if (monthfdate == monthedate)
                {
                    monthname = monthname.Substring(0, 3);
                }
                else
                {
                    monthname += eName;
                }

            }

            return monthname;
        }

        public static Dictionary<string, string> GetLocalizeDictionary(string controllerName, string actionName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();

            string langFile = WebConfigurationManager.AppSettings["LangFile"];

            var actualPath = System.Web.Hosting.HostingEnvironment.MapPath($"~/Content/Language/{langFile}");

            if (!File.Exists(actualPath))
            {
                actualPath = System.Web.Hosting.HostingEnvironment.MapPath($"~/Content/Language/language-vi.xml");
            }

            xml.Load(actualPath);

            foreach (System.Xml.XmlNode node in xml.SelectNodes($"/root/{controllerName}/{actionName}/Text"))
            {
                result.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
            }

            return result;
        }

        #region Looker
        public static List<SelectListModel> ActionLockerProcess()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Hủy" },
                                         new SelectListModel { ItemValue = "1", ItemText = "Nạp"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Mở tủ thủ công"},
                                    };
            return list;
        }
        public static List<SelectListModel> TypeLockerProcess()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Cố định" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Tức thời"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Nhận dạng"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Mở tủ thủ công"},
                                    };
            return list;
        }

        public static List<SelectListModel> LockerEventCode()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Check in" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Check out"},
                                    };
            return list;
        }
        public static List<SelectListModel> LockerEventType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Nạp cố định" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Thẻ tức thời"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Nhận dạng khuôn mặt"},
                                    };
            return list;
        }
        public static List<SelectListModel> LockerAlarmCode()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "1", ItemText = "Thẻ không tồn tại" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Thẻ chưa đăng ký"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Chưa gửi đồ"},
                                    };
            return list;
        }
        #endregion

        /// <summary>
        /// Chia list to thành nhiều list con
        /// </summary>
        /// <param name="list">List to</param>
        /// <param name="count">Số lượng phần tử trong mỗi list con</param>
        /// <returns></returns>
        public static List<string> GetSubListFromList(List<string> list, int count = 10)
        {
            var lst = new List<string>();
            //var arr = "'1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21'";
            var listCount = list.Count; //đếm số lượng phần tử trong list
            if (listCount > count) // 10 là số lượng phần tử có trong list con
            {
                var b = listCount / count; //kiểm tra xem chia thành mấy list con
                for (int i = 0; i <= b; i++)
                {
                    var number = count * i; // số lượng phần tử đã chia ra list con

                    var newitem = "";

                    if (i == 0)  //list con thứ nhất
                    {
                        newitem = string.Join(",", list.Take(count));
                    }
                    else
                    {
                        if (listCount - number <= count) // list con cuối cùng
                        {
                            newitem = string.Join(",", list.Skip(number));
                        }
                        else //các list con kế tiếp
                        {
                            newitem = string.Join(",", list.Skip(number).Take(count));
                        }

                    }

                    lst.Add(newitem);
                }

            }

            return lst;
        }

        /// <summary>
        /// Lấy License Data từ API WORK MANAGEMENT
        /// </summary>
        /// <param name="feename">Tên biểu phí, để trống lấy all</param>
        /// <returns></returns>
        public static async Task<MessageReport> FetchLicenseData(string feename = "")
        {
            var message = new MessageReport();
            var webUrl = ConfigurationManager.AppSettings["KztekAPI"];

            try
            {
                var reqUri = new Uri(webUrl + "api/config/license/" + feename);

                var response = await ApiHelper.HttpGet(reqUri.ToString(), "", "");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"uploads\lic.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (var sr = new StreamWriter(fs))
                        {
                            var licData = Security.CryptoProvider.SimpleEncryptWithPassword(content, SecurityModel.License_Key);

                            sr.Write(licData);
                            sr.Flush();
                        }
                    }

                    message.isSuccess = true;
                    message.Message = "Cập nhật thành công";
                }
                else
                {
                    message.isSuccess = false;
                    message.Message = "Cập nhật thất bại";
                }
            }
            catch (Exception ex)
            {
                message.isSuccess = false;
                message.Message = ex.Message;
            }

            return message;
        }



        /// <summary>
        /// VNPAY
        /// </summary>
        /// <returns></returns>
        public static List<SelectListModel> TypesOfGoods()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "topup", ItemText = "Nạp tiền điện thoại"},
                                         new SelectListModel { ItemValue = "billpayment", ItemText = "Thanh toán hóa đơn"},
                                         new SelectListModel { ItemValue = "fashion", ItemText = "Thời trang"},
                                         new SelectListModel { ItemValue = "other", ItemText = "Thanh toán trực tuyến"}
                                    };
            return list;
        }

        public static List<SelectListModel> cboLanguage()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "vn", ItemText = "Tiếng Việt"},
                                         new SelectListModel { ItemValue = "en", ItemText = "English"},
                                    };
            return list;
        }
        public static List<SelectListModel> TypeInvoices()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "I", ItemText = "Cá nhân"},
                                         new SelectListModel { ItemValue = "O", ItemText = "Công ty"},
                                    };
            return list;
        }
        public static List<SelectListModel> Bank()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = "Không chọn"},
                                         new SelectListModel { ItemValue = "VNPAYQR", ItemText = "VNPAYQR"},
                                         new SelectListModel { ItemValue = "VNBANK", ItemText = "LOCAL BANK"},
                                         new SelectListModel { ItemValue = "IB", ItemText = "INTERNET BANKING"},
                                         new SelectListModel { ItemValue = "INTCARD", ItemText = "INTERNATIONAL CARD"},
                                         new SelectListModel { ItemValue = "VISA", ItemText = "VISA"},
                                         new SelectListModel { ItemValue = "MASTERCARD", ItemText= "MASTERCARD" },
                                         new SelectListModel { ItemValue = "JCB", ItemText = "JCB" },
                                         new SelectListModel { ItemValue = "UPI", ItemText = "UPI" },
                                         new SelectListModel { ItemValue = "NCB", ItemText = "Ngan hang NCB" },
                                         new SelectListModel { ItemValue = "SACOMBANK", ItemText = "Ngan hang SacomBank" },
                                         new SelectListModel { ItemValue = "EXIMBANK", ItemText = "Ngan hang EximBank" },
                                         new SelectListModel { ItemValue = "MSBANK", ItemText = "Ngan hang MSBANK" },
                                         new SelectListModel { ItemValue = "NAMABANK", ItemText = "Ngan hang NamABank " },
                                         new SelectListModel { ItemValue = "VNMART", ItemText = "Vi dien tu VnMart" },
                                         new SelectListModel { ItemValue = "VIETINBANK", ItemText = "Ngan hang Vietinbank" },
                                         new SelectListModel { ItemValue = "VIETCOMBANK", ItemText = "Ngan hang VCB" },
                                         new SelectListModel { ItemValue = "HDBANK", ItemText = "Ngan hang HDBank" },
                                         new SelectListModel { ItemValue = "DONGABANK", ItemText = "Ngan hang Dong A" },
                                         new SelectListModel { ItemValue = "TPBANK", ItemText = "Ngân hàng TPBank" },
                                         new SelectListModel { ItemValue = "OJB", ItemText = "Ngân hàng OceanBank" },
                                         new SelectListModel { ItemValue = "BIDV", ItemText = "Ngân hàng BIDV" },
                                         new SelectListModel { ItemValue = "TECHCOMBANK", ItemText = "Ngân hàng Techcombank" },
                                         new SelectListModel { ItemValue = "VPBANK", ItemText = "Ngan hang VPBank" },
                                         new SelectListModel { ItemValue = "AGRIBANK", ItemText = "Ngan hang Agribank" },
                                         new SelectListModel { ItemValue = "ACB", ItemText = "Ngan hang ACB" },
                                         new SelectListModel { ItemValue = "OCB", ItemText = "Ngan hang Phuong Dong" },
                                         new SelectListModel { ItemValue = "SCB", ItemText = "Ngan hang SCB" },
                                    };
            return list;
        }
    }
}