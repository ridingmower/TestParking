using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessLevelDetailRepository : RepositoryBase<tblAccessLevelDetail>, ItblAccessLevelDetailRepository
	{
		public tblAccessLevelDetailRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessLevelDetailRepository : IRepository<tblAccessLevelDetail>
	{
	}
}
