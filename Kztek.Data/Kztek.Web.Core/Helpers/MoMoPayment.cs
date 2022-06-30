using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Helpers
{
    public class MoMoPayment
    {
        public static string GetPayUrl_Website(string orderid)
        {
            string endPoint = ConfigurationManager.AppSettings["EndPoint"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["PartnerCode"].ToString();
            string accessKey = ConfigurationManager.AppSettings["AccessKey"].ToString();
            string secretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"].ToString();
            string notifyUrl = ConfigurationManager.AppSettings["NotifyUrl"].ToString();
            string orderInfo = "DH" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string amount = "10000";
            string orderId = orderid;
            string requestId = "00000900000";
            string extraData = "";

            //Before sign HMAC SHA256 signature
            //thông tin đơn hàng
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderId + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyUrl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();

            //ký đơn hàng và gửi thông tin đơn kèm theo
            //sign signature SHA256
            //chữ ký
            string signature = crypto.signSHA256(rawHash, secretKey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            //gửi request sang momo
            string responseFromMomo = MoMoPaymentRequest.sendPaymentRequest(endPoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            //đường link thanh toán momo trả về
            return jmessage.GetValue("payUrl").ToString();
        }

        public static string Refund()
        {
            string endPoint = ConfigurationManager.AppSettings["EndPointRefund"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["PartnerCode"].ToString();
            string publicKey = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", ConfigurationManager.AppSettings["PublicKey"].ToString());
            string requestId = Guid.NewGuid().ToString();
            string version = "2.0";
            string partnerRefId = "11111911119";
            string momoTransId = "137489899";           
            long amount = 10000;
            //get hash
            MoMoSecurity momoCrypto = new MoMoSecurity();

            string hash = momoCrypto.buildRefundHash(partnerCode, partnerRefId, momoTransId, amount,
                "Hoan tien Momo",
                publicKey);

            //request to MoMo
            string jsonRequest = "{\"partnerCode\":\"" +
                partnerCode + "\",\"requestId\":\"" +
                requestId + "\",\"version\":" +
                version + ",\"hash\":\"" +
                hash + "\"}";
          
            //response from MoMo
            string responseFromMomo = MoMoPaymentRequest.sendPaymentRequest(endPoint, jsonRequest.ToString(),20000);

            JObject jmessage = JObject.Parse(responseFromMomo);

            return jmessage.GetValue("message").ToString();
        }
    }
}
