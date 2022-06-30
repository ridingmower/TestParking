using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblSubSystemRepository : RepositoryBase<tblSubSystem>, ItblSubSystemRepository
	{
		public tblSubSystemRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblSubSystemRepository : IRepository<tblSubSystem>
	{
	}
}
