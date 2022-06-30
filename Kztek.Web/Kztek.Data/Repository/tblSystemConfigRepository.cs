using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblSystemConfigRepository : RepositoryBase<tblSystemConfig>, ItblSystemConfigRepository
	{
		public tblSystemConfigRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblSystemConfigRepository : IRepository<tblSystemConfig>
	{
	}
}
