using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.ProcedureRepositories.PanelistSaatKullanimi
{
    public interface IGetPerformancePanelistPerformance:IStoredProcedureRepository<Domain.Entities.Procedure.PanelistSaatKullanimi>
    {
    }
}
