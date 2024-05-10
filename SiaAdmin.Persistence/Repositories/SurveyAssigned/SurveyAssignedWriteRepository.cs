using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.SurveyAssigned;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SurveyAssignedWriteRepository : WriteRepository<SurveyAssigned>, ISurveyAssignedWriteRepository
    {
        private readonly DbSet<SurveyAssigned> _surveyAssigneds;
        public SurveyAssignedWriteRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyAssigneds = context.Set<SurveyAssigned>();

        }


        public bool IsDuplicatedGuid(int surveyId, Guid internalGuid)
        { 
            bool isDuplicated = _surveyAssigneds.FirstOrDefault(x => x.InternalGuid == internalGuid && x.SurveyId == surveyId) == null ? true : false;
            return isDuplicated;
        }
    }
}
