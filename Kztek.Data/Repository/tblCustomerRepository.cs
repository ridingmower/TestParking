using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCustomerRepository : RepositoryBase<tblCustomer>, ItblCustomerRepository
	{
		public tblCustomerRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCustomerRepository : IRepository<tblCustomer>
	{
	}
}
