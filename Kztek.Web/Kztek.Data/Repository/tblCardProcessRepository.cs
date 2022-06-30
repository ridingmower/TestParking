using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCardProcessRepository : RepositoryBase<tblCardProcess>, ItblCardProcessRepository
	{
		public tblCardProcessRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCardProcessRepository : IRepository<tblCardProcess>
	{
	}
}
