using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IUserRepository : IRepository<User>
	{
	}
}
