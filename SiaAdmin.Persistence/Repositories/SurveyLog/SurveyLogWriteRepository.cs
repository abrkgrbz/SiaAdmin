using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SurveyLogWriteRepository : WriteRepository<SurveyLog>, ISurveyLogWriteRepository
    {
        private readonly DbSet<SurveyLog> _surveyLogSet;
        private SiaAdminDbContext _context;
        public SurveyLogWriteRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyLogSet = context.Set<SurveyLog>();
            _context=context;
        }

        public async Task<bool> AddOrUpdateAsync(List<SurveyLog> list)
        {
            foreach (var item in list)
            {
                var exist = _surveyLogSet.SingleOrDefault(x =>
                    x.SurveyId == item.SurveyId && x.SurveyUserGuid == item.SurveyUserGuid);
                if (exist == null)
                {
                    await _surveyLogSet.AddAsync(item);
                }
                else
                {
                    exist.UpdateTime = DateTime.Now;
                    _surveyLogSet.Update(exist);
                }
              

            }

            return true;
        }

        public async Task<int> CreateSurveyLogByUserIncentive(int incentiveId, Guid internalGuid)
        {
            string sql =
                " \r\ndeclare @myGOAL as integer = 0;\r\ndeclare @myPOINTS as integer = 0;\r\ndeclare @myINCENTIVETEXT as nvarchar(MAX) = ''; \r\nSELECT  @myGOAL = ISNULL(sum([SurveyPoints]),0) FROM [SiaLive].[dbo].[SurveyLog] where SurveyUserGUID IN (SELECT [SurveyUserGUID] FROM [SiaLive].[dbo].[Users] where Active = 1 and [InternalGUID] = @myGUID) \r\nand  Active = 1 and Approved = 1\r\nSELECT @myINCENTIVETEXT = [IncentiveText], @myPOINTS = [Points] FROM [SiaLive].[dbo].[Incentives] where [Active] = 1 and [Points]<= @myGOAL and Id = @myID and @myID <> 0 \r\nIF @myPOINTS > 0\r\n    BEGIN\r\n        Insert into [SiaLive].[dbo].[SurveyLog]([SurveyUserGUID],[SurveyId],[SurveyPoints],[Active],[Approved],[Text]) VALUES ((SELECT TOP 1 [SurveyUserGUID] FROM [SiaLive].[dbo].[Users]\r\n\t\twhere Active = 1 and [InternalGUID] = @myGUID) ,0,@myPOINTS*-1,1,1,@myINCENTIVETEXT);\r\n end";

            object[] paramItems = new object[]
            {
                new SqlParameter("@myID", incentiveId),
                new SqlParameter("@myGUID", internalGuid), 

            };
            int result=await _context.Database.ExecuteSqlRawAsync(sql, paramItems);
            return result;
        }
    }
}
