using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
	{
		public UserRoleRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IUserRoleRepository : IRepository<UserRole>
	{
	}
}
