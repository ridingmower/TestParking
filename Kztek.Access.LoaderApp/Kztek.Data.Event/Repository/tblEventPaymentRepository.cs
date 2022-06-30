using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblEventPaymentRepository : RepositoryBase<tblEventPayment>, ItblEventPaymentRepository
    {
        public tblEventPaymentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface ItblEventPaymentRepository : IRepository<tblEventPayment>
    {
    }
}
