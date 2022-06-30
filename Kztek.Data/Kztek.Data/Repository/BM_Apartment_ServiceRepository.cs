using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_Apartment_ServiceRepository : RepositoryBase<BM_Apartment_Service>, IBM_Apartment_ServiceRepository
    {
        public BM_Apartment_ServiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_Apartment_ServiceRepository : IRepository<BM_Apartment_Service>
    {
    }
}
