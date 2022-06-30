using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblSubCardRepository : RepositoryBase<tblSubCard>, ItblSubCardRepository
	{
		public tblSubCardRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblSubCardRepository : IRepository<tblSubCard>
	{
	}
}
