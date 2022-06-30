using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ApartmentRepository : RepositoryBase<BM_Apartment>, IBM_ApartmentRepository
    {
        public BM_ApartmentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ApartmentRepository : IRepository<BM_Apartment>
    {
    }
}
