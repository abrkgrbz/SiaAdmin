using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.ProcedureRepositories.PanelistSaatKullanimi;
using SiaAdmin.Persistence.Contexts;
using SiaAdmin.Persistence.Repositories;

namespace SiaAdmin.Persistence.ProcedureRepositories.PanelistSaatKullanimi
{
    public class GetPerformancePanelistPerformance:StoredProcedureRepository<Domain.Entities.Procedure.PanelistSaatKullanimi>,IGetPerformancePanelistPerformance
    {
        public GetPerformancePanelistPerformance(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
