using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
    public class tblAccessControllerGroupRepository : RepositoryBase<tblAccessControllerGroup>, ItblAccessControllerGroupRepository
    {
        public tblAccessControllerGroupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
    public interface ItblAccessControllerGroupRepository : IRepository<tblAccessControllerGroup>
    {
    }
}
