using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class DynamicCardRepository : RepositoryBase<DynamicCard>, IDynamicCardRepository
	{
		public DynamicCardRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}
	}
	public interface IDynamicCardRepository : IRepository<DynamicCard>
	{
	}
}
