using Kztek.Data.Event.Repository;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models.API;
using Kztek.Model.Models.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Payment
{
    public interface ItblEventPaymentService
    {
        Task<MessageReport> Create(API_QRCodeResponseModel model, string evenid, string timein, string timeout, string plate);

        Task<MessageReport> Update(API_QRCodeCheckResponse model, string evenid);
    }

    public class tblEventPaymentService : ItblEventPaymentService
    {
        private ItblEventPaymentRepository _tblEventPaymentRepository;

        public tblEventPaymentService(ItblEventPaymentRepository _tblEventPaymentRepository)
        {
            this._tblEventPaymentRepository = _tblEventPaymentRepository;
        }

        public async Task<MessageReport> Create(API_QRCodeResponseModel model, string evenid, string timein, string timeout, string plate)
        {
            var query = new StringBuilder();
            query.AppendLine("INSERT INTO tblEventPayment (EventId, DateCreated, TimeIn, TimeOut, Plate, Money, OrderId, PaymentStatus, isSuccessQRCode, isSuccessPay, ResponseContentQRCode, ResponseContentPay) VALUES (");

            query.AppendLine(string.Format("'{0}'", evenid));
            query.AppendLine(", GETDATE()");
            query.AppendLine(string.Format(", '{0}'", timein));
            query.AppendLine(string.Format(", '{0}'", timeout));
            query.AppendLine(string.Format(", '{0}'", plate));
            query.AppendLine(string.Format(",  {0}", model.qrCodeData.AMOUNT));
            query.AppendLine(string.Format(", '{0}'", model.qrCodeData.ORDER_ID));

            query.AppendLine(", 0");

            query.AppendLine(", 1");
            query.AppendLine(", 0");

            query.AppendLine(string.Format(", '{0}'", JsonConvert.SerializeObject(model.qrCodeData)));
            query.AppendLine(", ''");

            query.AppendLine(")");

            var result = new MessageReport(false, "error");

            try
            {
                Kztek.Data.Event.SqlHelper.ExcuteSQLEvent.Execute(query.ToString());

                result = new MessageReport(true, "success");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return await Task.FromResult(result);
        }

        public async Task<MessageReport> Update(API_QRCodeCheckResponse model, string evenid)
        {
            var query = new StringBuilder();
            query.AppendLine("UPDATE tblEventPayment SET");
            query.AppendLine(string.Format(" PaymentStatus = {0} ", model.paymentStatus));
            query.AppendLine(string.Format(", isSuccessPay = {0} ", model.paymentStatus == 200 ? "1" : "0" ));
            query.AppendLine(string.Format(", ResponseContentPay = '{0}' ", JsonConvert.SerializeObject(model)));
            query.AppendLine(string.Format("WHERE EventId = '{0}'", evenid));

            var result = new MessageReport(false, "error");

            try
            {
                Kztek.Data.Event.SqlHelper.ExcuteSQLEvent.Execute(query.ToString());

                result = new MessageReport(true, "success");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
