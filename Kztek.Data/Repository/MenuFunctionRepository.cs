using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class MenuFunctionRepository : RepositoryBase<MenuFunction>, IMenuFunctionRepository
	{
		public MenuFunctionRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IMenuFunctionRepository : IRepository<MenuFunction>
	{
	}
}
