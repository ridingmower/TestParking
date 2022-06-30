using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class ExcelColumnRepository : RepositoryBase<ExcelColumn>, IExcelColumnRepository
    {
        public ExcelColumnRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IExcelColumnRepository : IRepository<ExcelColumn>
    {
    }
}
