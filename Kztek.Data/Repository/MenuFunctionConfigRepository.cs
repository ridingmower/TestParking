using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class MenuFunctionConfigRepository : RepositoryBase<MenuFunctionConfig>, IMenuFunctionConfigRepository
	{
		public MenuFunctionConfigRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IMenuFunctionConfigRepository : IRepository<MenuFunctionConfig>
	{
	}
}
