using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCardRepository : RepositoryBase<tblCard>, ItblCardRepository
	{
		public tblCardRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCardRepository : IRepository<tblCard>
	{
	}
}
