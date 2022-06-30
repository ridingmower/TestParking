using System;
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_Apartment_ReceiptRepository : RepositoryBase<BM_Apartment_Receipt>, IBM_Apartment_ReceiptRepository
    {
        public BM_Apartment_ReceiptRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_Apartment_ReceiptRepository : IRepository<BM_Apartment_Receipt>
    {
    }
}
