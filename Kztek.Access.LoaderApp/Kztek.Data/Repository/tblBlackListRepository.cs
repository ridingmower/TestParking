using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblBlackListRepository : RepositoryBase<tblBlackList>, ItblBlackListRepository
	{
		public tblBlackListRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblBlackListRepository : IRepository<tblBlackList>
	{
	}
}
