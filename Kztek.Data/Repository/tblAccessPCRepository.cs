using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessPCRepository : RepositoryBase<tblAccessPC>, ItblAccessPCRepository
	{
		public tblAccessPCRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessPCRepository : IRepository<tblAccessPC>
	{
	}
}
