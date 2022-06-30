using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class API_QRCodeRequest
    {
        public string EventId { get; set; }

        public int Money { get; set; }

        public string Plate { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }
    }

    public class API_QRCodeResponse
    {
        public string TYPE_QR { get; set; }
        public string PRIORITY { get; set; }
        public string VERSION { get; set; }
        public string TYPE { get; set; }
        public string MERCHANT_CODE { get; set; }
        public string SOURCE { get; set; }
        public string BILLCODE { get; set; }
        public string AMOUNT { get; set; }
        public string ORDER_ID { get; set; }
    }

    public class API_QRCodeResponseModel
    {
        public string statusCode { get; set; }

        public string message { get; set; }

        public string keyMessage { get; set; }

        public string currentTime { get; set; }

        public API_QRCodeResponse qrCodeData { get; set; }

        public string linkData { get; set; }

        public bool success { get; set; }
    }

    public class API_QRCodeCheckResponse
    {
        public int paymentStatus { get; set; }

        public int parkingId { get; set; }

        public string reqPaymentId { get; set; }

        public string orderId { get; set; }

        public int totalAmount { get; set; }
    }

    public class PcResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class LaneResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class API_BookInRequest
    {
        public int CardNumber { get; set; }

        public string EventId { get; set; }

        public string Plate { get; set; }

        public string TimeIn { get; set; }
    }

    public class API_BookInResponse
    {
        public int statusCode { get; set; }

        public int parkingId { get; set; }

        public string reqPaymentId { get; set; }

        public string bookingId { get; set; }
    }

    public class API_BookOutRequest
    {
        public string EventId { get; set; }

        public string Plate { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }

        public int Money { get; set; }
    }

    public class API_BookOutResponse
    {
        public int statusCode { get; set; }

        public API_QRCodeResponse qrCodeData { get; set; }

        public string linkData { get; set; }
    }

    public class API_UserQrCode
    {
        public string address { get; set; }

        public string firstName { get; set; }

        public string gender { get; set; }

        public string lastName { get; set; }

        public string phone { get; set; }

        public string userId { get; set; }
    }
}
