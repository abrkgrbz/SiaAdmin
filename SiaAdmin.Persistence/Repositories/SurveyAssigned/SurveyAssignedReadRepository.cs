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
    public class SurveyAssignedReadRepository:ReadRepository<SurveyAssigned>,ISurveyAssignedReadRepository
    {
        private readonly DbSet<SurveyAssigned> _surveyAssigneds;
         
        public SurveyAssignedReadRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyAssigneds = context.Set<SurveyAssigned>();
        }

        public bool IsDuplicatedGuid(int surveyId, Guid internalGuid)
        {
            bool isDuplicated = _surveyAssigneds.All(x => x.InternalGuid != internalGuid && x.SurveyId != surveyId);
            return isDuplicated;
        }
    }
}
