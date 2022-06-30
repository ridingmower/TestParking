using System;
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_Apartment_MemberRepository : RepositoryBase<BM_Apartment_Member>, IBM_Apartment_MemberRepository
    {
        public BM_Apartment_MemberRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_Apartment_MemberRepository : IRepository<BM_Apartment_Member>
    {
    }
}
