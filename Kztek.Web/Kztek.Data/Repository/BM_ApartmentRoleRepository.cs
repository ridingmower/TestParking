using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ApartmentRoleRepository : RepositoryBase<BM_ApartmentRole>, IBM_ApartmentRoleRepository
    {
        public BM_ApartmentRoleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ApartmentRoleRepository : IRepository<BM_ApartmentRole>
    {
    }
}
