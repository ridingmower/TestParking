using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class VNQPAYController : Controller
    {
        private IOrderInfoService _OrderInfoService;

        public VNQPAYController(IOrderInfoService _OrderInfoService)
        {
            this._OrderInfoService = _OrderInfoService;
        }

        // GET: Parking/VNQPAY
        public ActionResult Index()
        {
            ViewBag.TypesOfGoods = FunctionHelper.TypesOfGoods();
            ViewBag.Bank = FunctionHelper.Bank();
            ViewBag.cboLanguage = FunctionHelper.cboLanguage();
            ViewBag.TypeInvoices = FunctionHelper.TypeInvoices();
            return View();
        }

        [HttpPost]
        public ActionResult Index(VNPAY_Custom model)
        {
            var obj = new VNPAY();

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                //lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
                return View(model);
            }
            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks.ToString();
            order.Amount = model.vnp_Amount.ToString();
            order.OrderDesc = model.vnp_OrderInfo;
            order.CreatedDate = DateTime.Now;
            var returnOrder = _OrderInfoService.Create(order);
            string locale = model.vnp_Locale;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.1");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (Convert.ToInt64(order.Amount) * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", model.vnp_BankCode);
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }

            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDesc);
            vnpay.AddRequestData("vnp_OrderType", model.vnp_OrderType); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            //Add Params of 2.0.1 Version
            vnpay.AddRequestData("vnp_ExpireDate", model.vnp_ExpireDate.ToString("yyyyMMddHHmmss"));
            //vnpay.AddRequestData("vnp_ExpireDate", "20210726094237");
            //Billing
            vnpay.AddRequestData("vnp_Bill_Mobile", model.vnp_Bill_Mobile);
            vnpay.AddRequestData("vnp_Bill_Email", model.vnp_Bill_Email);

            vnpay.AddRequestData("vnp_Bill_FirstName", model.vnp_Bill_FirstName);
            vnpay.AddRequestData("vnp_Bill_LastName", model.vnp_Bill_LastName);
            vnpay.AddRequestData("vnp_Bill_Address", model.vnp_Bill_Address);
            vnpay.AddRequestData("vnp_Bill_City", model.vnp_Bill_City);
            vnpay.AddRequestData("vnp_Bill_Country", model.vnp_Bill_Country);
            vnpay.AddRequestData("vnp_Bill_State", "");
            // Invoice
            vnpay.AddRequestData("vnp_Inv_Phone", model.vnp_Inv_Phone);
            vnpay.AddRequestData("vnp_Inv_Email", model.vnp_Inv_Email);
            vnpay.AddRequestData("vnp_Inv_Customer", model.vnp_Inv_Customer);
            vnpay.AddRequestData("vnp_Inv_Address", model.vnp_Inv_Address);
            vnpay.AddRequestData("vnp_Inv_Company", model.vnp_Inv_Company);
            vnpay.AddRequestData("vnp_Inv_Taxcode", model.vnp_Inv_Taxcode);
            vnpay.AddRequestData("vnp_Inv_Type", model.vnp_Inv_Type);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return Redirect(paymentUrl);

        }


        //vnpay_return
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult VnpayReturn()
        {

            //log.InfoFormat("Begin VNPAY Return, URL={0}", Request.RawUrl);
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();
                var obj = new VNPAY_Custom();
                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }


                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: MD5 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toan thanh cong
                        //displayMsg.InnerText = "Thanh toán thành công";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        ViewBag.VnpaySuccess = "Thanh toan thanh cong OrderId =" + orderId + ", VNPAY TranId = " + vnpayTranId;
                    }
                    else
                    {
                        ViewBag.VnpaySuccess = "Thanh toan khong thanh cong. Ma loi:" + vnp_ResponseCode;
                        //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                }
                else
                {
                    ViewBag.VnpaySuccess = "Invalid signature, InputData=" + Request.RawUrl;
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý";                       
                    //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý";
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                }
            }


            return View();
        }

        public ActionResult VnpayIpn()
        {
            string returnContent = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret key
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();


                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //Lay danh sach tham so tra ve tu VNPAY
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: MD5 cua du lieu tra ve

                var orderId = vnpay.GetResponseData("vnp_TxnRef");
                var vnpayTranId = vnpay.GetResponseData("vnp_TransactionNo");
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                if (checkSignature)
                {
                    //Cap nhat ket qua GD
                    //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                    //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                    OrderInfo order = new OrderInfo();//get from DB
                    order.OrderId = orderId;
                    order.PaymentTranId = vnpayTranId;
                    order.Status = "0"; //0: Cho thanh toan,1: da thanh toan,2: GD loi
                                        //Kiem tra tinh trang Order
                    if (order != null)
                    {
                        if (order.Status == "0")
                        {
                            if (vnp_ResponseCode == "00")
                            {
                                //Thanh toan thanh cong
                                //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId,
                                //    vnpayTranId);
                                order.Status = "1";
                            }
                            else
                            {
                                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                //  displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                                //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}",
                                //    orderId,
                                 //  vnpayTranId, vnp_ResponseCode);
                                order.Status = "2";
                            }

                            //Thêm code Thực hiện cập nhật vào Database 
                            //Update Database

                            returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                        }
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                    }
                }
                else
                {
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            else
            {
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"Input data required\"}";
            }


            //Response.ClearContent();
            //Response.Write(returnContent);
            //Response.End();

            return View();
        }



        public ActionResult VnpayRefund()
        {
            return View();
        }

    }
}
