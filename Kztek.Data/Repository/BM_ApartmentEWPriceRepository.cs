using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ApartmentEWPriceRepository : RepositoryBase<BM_ApartmentEWPrice>, IBM_ApartmentEWPriceRepository
    {
        public BM_ApartmentEWPriceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ApartmentEWPriceRepository : IRepository<BM_ApartmentEWPrice>
    {
    }
}
