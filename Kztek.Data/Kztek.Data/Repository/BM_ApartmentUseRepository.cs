using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ApartmentUseRepository : RepositoryBase<BM_ApartmentUse>, IBM_ApartmentUseRepository
    {
        public BM_ApartmentUseRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ApartmentUseRepository : IRepository<BM_ApartmentUse>
    {
    }
}
