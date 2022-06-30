using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCardGroupRepository : RepositoryBase<tblCardGroup>, ItblCardGroupRepository
	{
		public tblCardGroupRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCardGroupRepository : IRepository<tblCardGroup>
	{
	}
}
