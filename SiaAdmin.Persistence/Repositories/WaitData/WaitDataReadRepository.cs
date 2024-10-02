using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class WaitDataReadRepository:ReadRepository<WaitData>,IWaitDataReadRepository
    {
        private readonly DbSet<CustomWaitData> _customWaitDataSet;
        public WaitDataReadRepository(SiaAdminDbContext context) : base(context)
        {
            _customWaitDataSet = context.Set<CustomWaitData>();

        }

        public  async Task<List<CustomWaitData>> GetAllWaitData()
        {
            string sql =
                "SELECT   [SurveyId] \r\n,CONVERT(date, [Tarih]) as Tarih\r\n,COUNT(*) as Adet\r\n  FROM [SiaLive].[dbo].[WaitData]\r\n  group by [SurveyId] ,CONVERT(date, [Tarih])\r\n   order by CONVERT(date, [Tarih])\r\n";
            var result =await _customWaitDataSet.FromSqlRaw(sql).ToListAsync();
            return result;
        }
    }
}
