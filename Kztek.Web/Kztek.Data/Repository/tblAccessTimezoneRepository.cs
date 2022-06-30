using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessTimezoneRepository : RepositoryBase<tblAccessTimezone>, ItblAccessTimezoneRepository
	{
		public tblAccessTimezoneRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessTimezoneRepository : IRepository<tblAccessTimezone>
	{
	}
}
