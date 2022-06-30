using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessLevelRepository : RepositoryBase<tblAccessLevel>, ItblAccessLevelRepository
	{
		public tblAccessLevelRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessLevelRepository : IRepository<tblAccessLevel>
	{
	}
}
