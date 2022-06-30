using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCustomerGroupRepository : RepositoryBase<tblCustomerGroup>, ItblCustomerGroupRepository
	{
		public tblCustomerGroupRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCustomerGroupRepository : IRepository<tblCustomerGroup>
	{
	}
}
