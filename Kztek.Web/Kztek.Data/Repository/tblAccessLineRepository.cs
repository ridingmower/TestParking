using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessLineRepository : RepositoryBase<tblAccessLine>, ItblAccessLineRepository
	{
		public tblAccessLineRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessLineRepository : IRepository<tblAccessLine>
	{
	}
}
