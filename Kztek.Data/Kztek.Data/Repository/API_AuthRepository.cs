using Kztek.Data.Infrastructure;
using Kztek.Model.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class API_AuthRepository : RepositoryBase<API_Auth>, IAPI_AuthRepository
    {
        public API_AuthRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IAPI_AuthRepository : IRepository<API_Auth>
    {
    }
}
