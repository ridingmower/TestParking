using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblVehicleGroupRepository : RepositoryBase<tblVehicleGroup>, ItblVehicleGroupRepository
	{
		public tblVehicleGroupRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblVehicleGroupRepository : IRepository<tblVehicleGroup>
	{
	}
}
