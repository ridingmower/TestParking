using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCompartmentRepository : RepositoryBase<tblCompartment>, ItblCompartmentRepository
	{
		public tblCompartmentRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCompartmentRepository : IRepository<tblCompartment>
	{
	}
}
