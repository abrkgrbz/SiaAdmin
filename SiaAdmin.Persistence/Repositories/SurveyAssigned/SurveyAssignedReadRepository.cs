using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.SqlClient;
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

        public bool CheckDuplicatedRecordByUserGUID(int surveyId, List<Guid> userGuidList)
        {
            try
            {
                const int batchSize = 10000;
                var duplicateGuids = new List<Guid>();

                var batches = SplitList(userGuidList, batchSize);

                foreach (var batch in batches)
                {
                    var batchDuplicates = _surveyAssigneds
                        .AsNoTracking() 
                        .Where(x => batch.Contains(x.InternalGuid) && x.SurveyId == surveyId)
                        .GroupBy(x => x.InternalGuid)
                        .Where(g => g.Count() >= 1)
                        .Select(g => g.Key)
                        .ToList();

                    duplicateGuids.AddRange(batchDuplicates);
                }

                var data = duplicateGuids.ToList();
                return duplicateGuids.Any();
            }
            catch (Exception e)
            { 
                return false;
            }
        }

        private static IEnumerable<List<T>> SplitList<T>(List<T> list, int batchSize)
        {
            for (int i = 0; i < list.Count; i += batchSize)
            {
                yield return list.GetRange(i, Math.Min(batchSize, list.Count - i));
            }
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
