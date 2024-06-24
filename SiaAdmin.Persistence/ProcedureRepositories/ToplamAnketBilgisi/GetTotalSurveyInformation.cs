using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.ProcedureRepositories.ToplamAnketBilgisi;
using SiaAdmin.Persistence.Contexts;
using SiaAdmin.Persistence.Repositories;

namespace SiaAdmin.Persistence.ProcedureRepositories.ToplamAnketBilgisi
{
    public class GetTotalSurveyInformation:StoredProcedureRepository<Domain.Entities.Procedure.ToplamAnketBilgisi>,IGetTotalSurveyInformation
    {
        public GetTotalSurveyInformation(SiaAdminDbContext context) : base(context)
        {
        }

    }
}
