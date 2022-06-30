using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class ReportRepository : RepositoryBase<Report>, IReportRepository
	{
		public ReportRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IReportRepository : IRepository<Report>
	{
	}
}
