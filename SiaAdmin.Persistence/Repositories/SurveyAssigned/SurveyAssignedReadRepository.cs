using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SurveyAssignedReadRepository : ReadRepository<SurveyAssigned>, ISurveyAssignedReadRepository
    {
        private readonly DbSet<SurveyAssigned> _surveyAssigneds;
        private readonly DbSet<MukerreKayit> _mukerreKayit;

        public SurveyAssignedReadRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyAssigneds = context.Set<SurveyAssigned>();
            _mukerreKayit = context.Set<MukerreKayit>();
        }

        public bool IsDuplicatedGuid(int surveyId, Guid internalGuid)
        {
            bool isDuplicated = _surveyAssigneds.All(x => x.InternalGuid != internalGuid && x.SurveyId != surveyId);
            return isDuplicated;
        }

        public async Task<List<MukerreKayit>> GetDuplicatedRecordList(int surveyId)
        {
            string sql = $"Select InternalGUID,SurveyId\r\nFrom SurveysAssigned\r\nwhere SurveyId={surveyId}\r\nGroup By InternalGUID ,SurveyId \r\nHaving Count (InternalGUID) > 1\r\n";
            var result = await _mukerreKayit.FromSqlRaw(sql).ToListAsync();
            return result;
        }
    }
}
