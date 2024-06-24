using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SurveyReadRepository:ReadRepository<Survey>,ISurveyReadRepository
    {
        private readonly DbSet<Survey> _surveys;
        public SurveyReadRepository(SiaAdminDbContext context) : base(context)
        {
            _surveys = context.Set<Survey>();
        }

        public bool  IsUniqueSurvey(int surveyId)
        {
            return _surveys.All(x=>x.Id!=surveyId);
        }
    }
}
