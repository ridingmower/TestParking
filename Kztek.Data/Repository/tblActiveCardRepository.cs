using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblActiveCardRepository : RepositoryBase<tblActiveCard>, ItblActiveCardRepository
	{
		public tblActiveCardRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblActiveCardRepository : IRepository<tblActiveCard>
	{
	}
}
