using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SurveyLogWriteRepository : WriteRepository<SurveyLog>, ISurveyLogWriteRepository
    {
        private readonly DbSet<SurveyLog> _surveyLogSet;
        public SurveyLogWriteRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyLogSet = context.Set<SurveyLog>();
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
    }
}
