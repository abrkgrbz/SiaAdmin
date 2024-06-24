using SiaAdmin.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.ProcedureRepositories.TanitimAnketiDolduran;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.ProcedureRepositories.TanitimAnketiDolduran
{
    public class GetNumberOfFillingSurveyIntro: StoredProcedureRepository<Domain.Entities.Procedure.TanitimAnketiDolduran>,IGetNumberOfFillingSurveyIntro
    {
        public GetNumberOfFillingSurveyIntro(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
